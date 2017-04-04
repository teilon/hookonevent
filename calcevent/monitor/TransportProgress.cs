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
