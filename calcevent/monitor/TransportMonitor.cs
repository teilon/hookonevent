using calcevent.status;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.progress
{
    public class TransportMonitor
    {
        enum Device { Tracker, Tablet };
        TransportList _transports = new TransportList();
        ZoneList _zones = new ZoneList();
        public TransportList Transports { get { return _transports; } }        
        public ZoneList Zones { get { return _zones; } }

        public TransportItem this[string id] { get { return Transports.Where(x => x.TransportId == id).FirstOrDefault(); } }  
        //for init
        public void AddTransport(List<TransportItem> dataList)
        {            
            _transports.AddRange(dataList);
            _transports.TransportInit();
            if (Zones.Count != 0)
                SyncZoneExcv();
        }
        public void AddZone(List<ZoneItem> dataList)
        {            
            _zones.AddRange(dataList);
            if (Transports.Count != 0)
                SyncZoneExcv();
        }
        //synchronization zone with excavotor
        void SyncZoneExcv()
        {
            foreach(var z in _zones.Where(x => x.Type == 1))
            {
                foreach (var e in _transports.Where(x => x.TypeId == "2"))
                {
                    if (z.Points[0] == e.CurrentLocation)
                    {
                        z.ExcavatorId = e.TransportId;
                        e.ZoneId = z.Id;
                        TXTWriter.Write(string.Format("{0, 10}[{1}|{2}] : {3, 10}[{4}|{5}]\n", 
                            z.DisplayName, z.Points[0].Latitude, z.Points[0].Longitude,
                            e.TransportId, e.CurrentLocation.Latitude, e.CurrentLocation.Longitude));
                    }
                        
                }
            }                
        }
        //add message
        public void AddMessage(string deviceId, string timestamp, string statuscode, string oreType)
        {
            TransportItem _ti = _transports.Where(x => x.TransportId == deviceId).FirstOrDefault();
            if (_ti == null)
                return;
            _ti.CurrentTimeStamp = timestamp;
            _ti.CurrentOreType = oreType;

            if (_ti.TypeId == "1")
                CalcTruck(_ti, Device.Tablet, statuscode);
            if (_ti.TypeId == "2")
                CalcExcv(_ti, Device.Tablet, statuscode);
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
                        
            if (_ti.TypeId == "1")
                CalcTruck(_ti);
            if (_ti.TypeId == "2")
                CalcExcv(_ti);
        }
        //calc
        void CalcTruck(TransportItem transport, Device d = Device.Tracker, string statuscode = "NN")
        {
            GeoCoordinate location = transport.CurrentLocation;
            string deviceId = transport.TransportId;
            string oldState = transport.CurrentState.GetCurrentState();
            if (location == null)
                return;
            
            Dictionary<string, string> checkZone = _zones.GetIntersect(location);
            bool isStoped = transport.CurrentSpeed == 0;
            if (checkZone["isIntersect"] == "no")
            {
                if (isStoped)
                {
                    (transport.CurrentState as IOutagerState).ToStop();

                    if (transport.CurrentState.GetCurrentState() != oldState)
                        transport.PreSave(deviceId, "", "-1");
                }
                else
                {
                    (transport.CurrentState as IMoverState).ToMove();

                    if (transport.CurrentState.GetCurrentState() != oldState)
                        transport.PreSave(deviceId, "", "-1");
                }
                return;
            }
            if(checkZone["type"] == "1")
            {
                if((checkZone["zonelocation"] == "target" && isStoped) || (d == Device.Tablet && statuscode == "LL"))
                {                    
                    (transport.CurrentState as ILoaderState).OnLoad();
                    //if (transport.CurrentOreType == "0") { }
                    if (checkZone.ContainsKey("excavatorId"))
                        transport.CurrentOreType = _transports[checkZone["excavatorId"]].CurrentOreType;
                }
                else if (checkZone["zonelocation"] == "zone")
                {
                    (transport.CurrentState as ILoaderState).OnLoadingZone();
                }

                if(transport.CurrentState.GetCurrentState() != oldState)
                {
                    transport.PreSave(deviceId, checkZone["excavatorId"], checkZone["zoneId"]);                    
                }
            }
                
            if ((checkZone["type"] == "3" && isStoped) || (d == Device.Tablet && statuscode == "UU"))
            {
                (transport.CurrentState as IUnloaderState).OnUnload();
                if (transport.CurrentState.GetCurrentState() != oldState)
                {
                    string excv = (!checkZone.ContainsKey("excavatorId")) ? "-1" : checkZone["excavatorId"];
                    transport.PreSave(deviceId, excv, checkZone["zoneId"]);
                    transport.CurrentOreType = "0";
                }
            }
        }
        void CalcExcv(TransportItem transport, Device d = Device.Tracker, string statuscode = "NN")
        {
            GeoCoordinate location = transport.CurrentLocation;
            string deviceId = transport.TransportId;
            
            string oldState = transport.CurrentState.GetCurrentState();
            if (location == null)
                return;

            string truckid = "-1";
            string zoneid = transport.ZoneId;
            int zonetype = _zones[zoneid].Type;

            if (zonetype == 1)
                foreach(var t in _transports.Where(x => x.TypeId == "1"))
                    if(_zones.IsIntersect(zoneid, t.CurrentLocation))
                        truckid = t.TransportId;

            if(truckid != "-1" || (d == Device.Tablet && statuscode == "LL"))
            {
                (transport.CurrentState as ITheLoaderState).OnLoad();
                /*
                if (transport.CurrentState.GetCurrentState() != oldState)
                {
                    transport.PreSave(truckid, deviceId, zoneid);
                }                
                */
                return;
            }
            if (transport.CurrentSpeed == 0)
            {
                (transport.CurrentState as IOutagerState).ToStop();
            }
            else
            {
                (transport.CurrentState as IMoverState).ToMove();
            }
        }
    }
    public class TransportList : List<TransportItem>
    {
        public TransportItem this[string transportid] { get { return this.Where(x => x.TransportId == transportid).FirstOrDefault(); } }
        public void TransportInit()
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
        public ZoneItem this[string zoneid] { get { return this.Where(x => x.Id == zoneid).FirstOrDefault(); } }
        
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
        public bool IsIntersect(string idZone, GeoCoordinate testcoord)
        {
            return IsSphereIntersect(idZone, testcoord, ZoneLocation.target);
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
