using calcevent.status;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.progress
{
    public class TransportProgress
    {
        List<TransportItem> _items;
        public List<TransportItem> Items { get { return _items; } }

        //Dictionary<string, StateInterface> _transportStates = new Dictionary<string, StateInterface>();
        //public StateInterface this[string transportId] { get { return _transportStates[transportId]; } }

        Dictionary<string, TransportItem> _transportStates = new Dictionary<string, TransportItem>();
        public TransportItem this[string transportId] { get { return _transportStates[transportId]; } }

        public TransportProgress(List<TransportItem> list)
        {
            _items = list;
            foreach(var item in _items)
            {
                _transportStates[item.TransportId] = item;
                switch (item.TypeId)
                {
                    case "1": _transportStates[item.TransportId].CurrentState = new TruckInterface();break;
                    case "2": _transportStates[item.TransportId].CurrentState = new ExcavatorInterface(); break;
                    default: _transportStates[item.TransportId].CurrentState = new StateInterface(); break;
                }
            }                
        }        

        // add message

        // message from tablet
        public void AddMessage(string deviceId, string timestamp, string statuscode, string oreType)
        {
            if (Items.Select(x => x.TransportId = deviceId).FirstOrDefault() == null)
                return;
            TransportItem _ti = _items.Where(x => x.TransportId == deviceId).FirstOrDefault();
            _ti.CurrentTimeStamp = timestamp;
            _ti.CurrentOreType = oreType;
            //ChangeStatusCode();
        }
        // message from tracker
        public void AddMessage(string deviceId, string timestamp, string statuscode, 
            double latitude, double longitude, double speedKPH, double heading, double altitude)
        {
            if (Items.Select(x => x.TransportId = deviceId).FirstOrDefault() == null)
                return;
            TransportItem _ti = _items.Where(x => x.TransportId == deviceId).FirstOrDefault();
            _ti.CurrentTimeStamp = timestamp;
            _ti.CurrentLocation.Latitude = latitude;
            _ti.CurrentLocation.Longitude = longitude;
            ChangeStatusCode(deviceId);
        }
        //calc
        void ChangeStatusCode(string deviceId)
        {
            if (checkLoad())
            {
                (_transportStates[deviceId] as ILoaderState).OnLoad();
                
            }
            if (checkUnload())
            {
                //unload
            }
        }
        public TransportItem GetTransportItem(string transportId)
        {
            return _items.Where(x => x.TransportId == transportId).FirstOrDefault();
        }
        void SetLoadKey(string transportid, string _zoneid, string _excavatorid, string _oretypeid, double _oreweight)
        {
            TransportEventKey _key = GetTransportItem(transportid).LastKeyEvent;
            _key.EventId = "1011";
            _key.ZoneId = _zoneid;
            _key.ExcavatorId = _excavatorid;
            _key.OreTypeId = _oretypeid;
            _key.OreWeight = _oreweight;
        }
        bool checkLoad()
        {            
            return true;
        }
        bool checkUnload()
        {
            return true;
        }

    }
    
    public class TransportItem
    {
        string _transportid = "";
        string _parknumber = "";
        string _modelid = "";
        string _typeid = "";

        GeoCoordinate _location;
        string _currenttimestamp = "0";
        string _currentoretype = "";
        double _currentspeed = 0;

        TransportEventKey _lastkeyevent = new TransportEventKey();
        StateInterface _currentstate;

        public string TransportId { get { return _transportid; } set { if (_transportid == "") _transportid = value; } }
        public string ParkNumber { get { return _parknumber; } set { if (_parknumber == "") _parknumber = value; } }
        public string ModelId { get { return _modelid; } set { if (_modelid == "") _modelid = value; } }
        public string TypeId { get { return _typeid; } set { if (_typeid == "") _typeid = value; } }
        
        public string CurrentTimeStamp { get { return _currenttimestamp; } set { _currenttimestamp = value; } }
        public string CurrentOreType { get { return _currentoretype; } set { _currentoretype = value; } }
        public GeoCoordinate CurrentLocation { get { return _location; } set { _location = value; } }
        public double CurrentSpeed { get { return _currentspeed; } set { _currentspeed = value; } }

        public string LastEventId { get { return _currentstate.GetCurrentState(); } }
        public TransportEventKey LastKeyEvent { get { return _lastkeyevent; } }
        public StateInterface CurrentState { get { return _currentstate; } set { _currentstate = value; } }        

        public TransportItem(string transportid)
        {
            _transportid = transportid;
            _location = new GeoCoordinate();
        }
    }

    public class TransportEventKey
    {
        string _TEMPORETYPEID = "5";
        double _TEMPOREWEIGHT = 130;

        string _eventid = "";
        string _zoneid = "";
        string _excavatorid = "";
        string _oretypeid = "";
        double _oreweight = 0;

        public string EventId { get { return _eventid; } set { _eventid = value; } }
        public string ZoneId { get { return _zoneid; } set { _zoneid = value; } }
        public string ExcavatorId { get { return _excavatorid; } set { _excavatorid = value; } }
        public string OreTypeId { get {
                return (_oretypeid == "") ? _TEMPORETYPEID : _oretypeid;
            } set { _oretypeid = value; } }
        public double OreWeight { get {
                return (_oretypeid == "") ? _TEMPOREWEIGHT : _oreweight;
            } set { _oreweight = value; } }

        public void Fill(string eventid, string zoneid, string excavatorid, string oretypeid)
        {
            _eventid = eventid;
            _zoneid = zoneid;
            _excavatorid = excavatorid;
            _oretypeid = oretypeid;
        }
    }

}
