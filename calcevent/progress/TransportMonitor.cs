using calcevent.status;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.progress
{
    class TransportMonitor
    {
        TransportList _transports = new TransportList();
        ZoneList _zones = new ZoneList();
        public TransportList Transports { get { return _transports; } }        
        public ZoneList Zones { get { return _zones; } }
        //for init
        public void AddTransport(List<TransportItem> dataList)
        {            
            _transports.AddRange(dataList);
        }
        public void AddZone(List<ZoneItem> dataList)
        {            
            _zones.AddRange(dataList);
        }
        //add message
        public void AddMessage(string deviceId, string timestamp, string statuscode, string oreType)
        {
            TransportItem _ti = _transports.Where(x => x.TransportId == deviceId).FirstOrDefault();
            if (_ti == null)
                return;
            _ti.CurrentTimeStamp = timestamp;
            _ti.CurrentOreType = oreType;
            //ChangeStatusCode();
        }
        public void AddMessage(string deviceId, string timestamp, string statuscode,
            double latitude, double longitude, double speedKPH, double heading, double altitude)
        {
            TransportItem _ti = _transports.Where(x => x.TransportId == deviceId).FirstOrDefault();
            if (_ti == null)
                return;
            _ti.CurrentTimeStamp = timestamp;
            _ti.CurrentLocation.Latitude = latitude;
            _ti.CurrentLocation.Longitude = longitude;
            _ti.CurrentSpeed = speedKPH;
            
            if(_ti.TypeId == "1")
                CalcTruck(_ti);
        }
        //calc
        void CalcTruck(TransportItem transport)
        {
            GeoCoordinate location = transport.CurrentLocation;
            string deviceId = transport.TransportId;
            string oldState = transport.CurrentState.GetCurrentState();
            if (location == null)
                return;

            Dictionary<string, string> checkZone = _zones.GetIntersect(location);
            if (checkZone["isIntersect"] == "no")
                return;
            if(checkZone["type"] == "1")
            {
                if(checkZone["zonelocation"] == "target")
                {                    
                    (transport.CurrentState as ILoaderState).OnLoad();
                }
                if (checkZone["zonelocation"] == "zone")
                {
                    (transport.CurrentState as ILoaderState).OnLoadingZone();
                }

                if(transport.CurrentState.GetCurrentState() != oldState)
                {
                    TransportEventKey _tek = transport.LastKeyEvent;
                    _tek.Fill(transport.CurrentState.GetCurrentState(), checkZone["zoneId"], checkZone["excavatorId"], transport.CurrentOreType);

                    saveLoadEvent(deviceId, checkZone["excavatorId"], checkZone["zoneId"], transport.CurrentOreType);
                }
            }
                
            if (checkZone["type"] == "3")
            {
                (transport.CurrentState as IUnloaderState).OnUnload();
                if (transport.CurrentState.GetCurrentState() != oldState)
                {
                    TransportEventKey _tek = transport.LastKeyEvent;
                    _tek.Fill(transport.CurrentState.GetCurrentState(), checkZone["zoneId"], "-", transport.CurrentOreType);

                    saveUnloadEvent(deviceId, checkZone["zoneId"], transport.CurrentOreType);
                }
            }
                
        }
        void saveLoadEvent(string truckid, string excavatorid, string zoneid, string oretype)
        {
            //save load
        }
        void saveUnloadEvent(string truckid, string zoneid, string oretype)
        {
            //save unload
        }
    }
    class TransportList : List<TransportItem>
    {
        public TransportList()
        {
            foreach (var item in this)
            {
                switch (item.TypeId)
                {
                    case "1": item.CurrentState = new TruckInterface(); break;
                    case "2": item.CurrentState = new ExcavatorInterface(); break;
                    default: item.CurrentState = new StateInterface(); break;
                }
            }
        }
    }
    public class ZoneList : List<ZoneItem>
    {
        enum ZoneLocation {zone, target}
        Dictionary<ZoneLocation, double> _zoneradius = new Dictionary<ZoneLocation, double>()
        {
            { ZoneLocation.zone, 50}, {ZoneLocation.target, 20 }
        };
        public Dictionary<string, string> GetIntersect(GeoCoordinate coordinate)
        {
            Dictionary<string, string> _result = new Dictionary<string, string>();
            _result["isIntersect"] = "yes";
            foreach (var item in this)
            {
                switch (item.Type)
                {
                    case 1:
                        _result["zoneId"] = item.Id;
                        _result["type"] = item.Type.ToString();
                        _result["excavatorId"] = item.ExcavatorId;
                        if (IsSphereIntersect(item.Id, coordinate, ZoneLocation.target))
                        {
                            _result["zonelocation"] = "target";
                            return _result; 
                        }
                        if (IsSphereIntersect(item.Id, coordinate, ZoneLocation.zone))
                        {                            
                            _result["zonelocation"] = "zone";
                            return _result;
                        }
                        _result.Remove("zoneId");
                        _result.Remove("type");
                        _result.Remove("excavatorId");
                        break;
                    case 2:
                    case 3:
                        if (IsPolygonIntersect(item.Id, coordinate))
                        {
                            _result["zoneId"] = item.Id;
                            _result["type"] = item.Type.ToString();
                            return _result; 
                        }
                        break;
                }
            }
            _result["isIntersect"] = "no";
            return _result;
        }
        bool IsSphereIntersect(string idZone, GeoCoordinate testcoord, ZoneLocation zonelocation)
        {
            ZoneItem zone = this.Where(x => x.Id == idZone).SingleOrDefault();
            if (testcoord == null || zone == null)
                return false;

            double _distance = testcoord.GetDistanceTo(zone.Points[0]);
            if (_distance < _zoneradius[zonelocation])
                return true;

            return false;
        }
        bool IsPolygonIntersect(string idZone, GeoCoordinate testcoord)
        {
            ZoneItem zone = this.Where(x => x.Id == idZone).SingleOrDefault();
            if (testcoord == null || zone == null || zone.Points.Count < 3)
                return false;

            bool total_term = true;
            bool term_a = Area(testcoord, zone[3], zone[0]) < 0.0;
            GeoCoordinate _focuscoord = zone[0];
            foreach (var coord in zone.Points)
            {
                if (_focuscoord == coord)
                    continue;

                bool term_b = Area(testcoord, _focuscoord, coord) < 0.0;
                total_term &= term_a == term_b;

                _focuscoord = coord;
            }

            return total_term;
        }
        double Area(GeoCoordinate test, GeoCoordinate a, GeoCoordinate b)
        {
            return ((a.Longitude - test.Longitude) * (b.Latitude - test.Latitude) - 
                (b.Longitude - test.Longitude) * (a.Latitude - test.Latitude));
        }
    }
}
