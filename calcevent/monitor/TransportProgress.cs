using calcevent.status;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.progress
{   
    public class TransportItem
    {
        const double _TEMPOREWEIGHT = 130;
        const double _OREWEIGHTISEMPTY = 0;
        const string _DEFAULTORETYPE = "0";

        string _transportid = "";
        string _parknumber = "";
        string _modelid = "";
        string _typeid = "";

        GeoCoordinate _location;
        string _currenttimestamp = "0";
        string _currentoretype = _DEFAULTORETYPE;
        double _currentspeed = 0;
        double _currentoreweight = _TEMPOREWEIGHT;

        TransportEventKey _lastkeyevent = new TransportEventKey();
        StateInterface _currentstate;

        bool _saveit = false;

        public string TransportId { get { return _transportid; } set { if (_transportid == "") _transportid = value; } }
        public string ParkNumber { get { return _parknumber; } set { if (_parknumber == "") _parknumber = value; } }
        public string ModelId { get { return _modelid; } set { if (_modelid == "") _modelid = value; } }
        public string TypeId { get { return _typeid; } set { if (_typeid == "") _typeid = value; } }
        public double OreWeight { get { return (_currentoretype == _DEFAULTORETYPE) ? _OREWEIGHTISEMPTY : _currentoreweight; } }

        public string CurrentTimeStamp { get { return _currenttimestamp; } set { _currenttimestamp = value; } }
        public string CurrentOreType { get { return _currentoretype; } set { _currentoretype = value; } }
        public GeoCoordinate CurrentLocation { get { return _location; } set { _location = value; } }
        public double CurrentSpeed { get { return _currentspeed; } set { _currentspeed = value; } }

        public string LastEventId { get { return _currentstate.GetCurrentState(); } }
        public TransportEventKey LastKeyEvent { get { return _lastkeyevent; } }
        public StateInterface CurrentState { get { return _currentstate; } set { _currentstate = value; } }      
        public bool SaveIt { get { return GetSaveCheck(); } }

        public TransportItem(string transportid)
        {
            _transportid = transportid;
            _location = new GeoCoordinate();
        }
        public void PreSave(string truckid, string excavatorid, string zoneid)
        {
            string eventid = _currentstate.GetCurrentState();
            string oretypeid = _currentoretype;
            string timestamp = _currenttimestamp;

            LastKeyEvent.Fill(truckid, eventid, zoneid, excavatorid, oretypeid, timestamp);
            _saveit = true;
        }
        bool GetSaveCheck()
        {
            bool _result = _saveit;
            _saveit = false;
            return _result;
        }
    }

    public class TransportEventKey
    {        
        const double _TEMPOREWEIGHT = 130;

        string _truckid = "";
        string _eventid = "";
        string _zoneid = "";
        string _excavatorid = "";
        string _oretypeid = "";
        double _oreweight = _TEMPOREWEIGHT;
        string _timestamp = "";

        public string TruckId { get { return _eventid; } }
        public string EventId { get { return _eventid; } }
        public string ZoneId { get { return _zoneid; } }
        public string ExcavatorId { get { return _excavatorid; } }
        public string OreTypeId { get { return _oretypeid; } }
        public double OreWeight { get { return (_oretypeid == "") ? 0 : _oreweight; } }
        public string Timestamp { get { return _timestamp; } }

        public void Fill(string truckid, string eventid, string zoneid, string excavatorid, string oretypeid, string timestamp)
        {
            _truckid = truckid;
            _eventid = eventid;
            _zoneid = zoneid;
            _excavatorid = excavatorid;
            _oretypeid = oretypeid;
            _timestamp = timestamp;
        }
    }

}
