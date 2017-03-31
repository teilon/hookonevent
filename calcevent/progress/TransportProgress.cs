using calcevent.status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.progress
{
    public class TransportProgress
    {
        List<TransportItem> _items;
        public List<TransportItem> Items { get { return _items; } }
        //public TransportItem this[string transportId] { get { return _items.Where(x => x.TransportId == transportId).FirstOrDefault(); } }

        //

        Dictionary<string, StateInterface> _transportStates = new Dictionary<string, StateInterface>();
        public StateInterface this[string transportId] { get { return _transportStates[transportId]; } }

        public TransportProgress(List<TransportItem> list)
        {
            _items = list;
            foreach(var item in _items)
            {
                switch (item.TypeId)
                {
                    case "1": _transportStates[item.TransportId] = new TruckInterface();break;
                    case "2": _transportStates[item.TransportId] = new ExcavatorInterface(); break;
                    default: _transportStates[item.TransportId] = new StateInterface(); break;
                }
            }
                
        }
        public string GetCurrentStates()
        {
            string result = string.Empty;
            foreach(var item in _items)
            {
                string id = item.TransportId;
                result += string.Format("{0}: {1}\n", id, _transportStates[id].GetCurrentState());
            }
            return result;
        }

        public void AddMessage(string deviceId, string timestamp, string statuscode, string oreType)
        {
            if (Items.Select(x => x.TransportId = deviceId).FirstOrDefault() == null)
                return;
            TransportItem _ti = _items.Where(x => x.TransportId == deviceId).FirstOrDefault();
            _ti.CurrentTimeStamp = timestamp;
            ChangeStatusCode(statuscode);
        }
        public void AddMessage(string deviceId, string timestamp, string statuscode, double latitude, double longitude, double speedKPH, double heading, double altitude)
        {
            if (Items.Select(x => x.TransportId = deviceId).FirstOrDefault() == null)
                return;
            TransportItem _ti = _items.Where(x => x.TransportId == deviceId).FirstOrDefault();
            _ti.CurrentTimeStamp = timestamp;
            ChangeStatusCode(statuscode);
        }
        void ChangeStatusCode(string statuscode)
        {

        }

    }
    
    public class TransportItem
    {
        string _transportid = "";
        string _parknumber = "";
        string _modelid = "";
        string _typeid = "";

        double _currentlongitude = 0;
        double _currentlatitude = 0;
        string _currenttimestamp = "0";
        string _currentzone = "";
        string _currentoretype = "";

        string _currenteventid = "";
        TransportEventKey _lastkeyevent = new TransportEventKey();

        public string TransportId { get { return _transportid; } set { if (_transportid == "") _transportid = value; } }
        public string ParkNumber { get { return _parknumber; } set { if (_parknumber == "") _parknumber = value; } }
        public string ModelId { get { return _modelid; } set { if (_modelid == "") _modelid = value; } }
        public string TypeId { get { return _typeid; } set { if (_typeid == "") _typeid = value; } }

        public double CurrentLongitude { get { return _currentlongitude; } set { _currentlongitude = value; } }
        public double CurrentLatitude { get { return _currentlatitude; } set { _currentlatitude = value; } }
        public string CurrentTimeStamp { get { return _currenttimestamp; } set { _currenttimestamp = value; } }
        public string CurrentZone { get { return _currentzone; } set { _currentzone = value; } }
        public string CurrentOreType { get { return _currentoretype; } set { _currentoretype = value; } }

        public string LastEventId { get { return _currenteventid; } set { _currenteventid = value; } }
        public TransportEventKey LastKeyEvent { get { return _lastkeyevent; } }

        public TransportItem(string transportid)
        {
            _transportid = transportid;
        }
    }

    public class TransportEventKey
    {
        string _eventid = "";
        string _zoneid = "";
        string _excavatorid = "";
        string _oretypeid = "";
        double _oreweight = 0;

        public string EventId { get { return _eventid; } set { _eventid = value; } }
        public string ZoneId { get { return _zoneid; } set { _zoneid = value; } }
        public string ExcavatorId { get { return _excavatorid; } set { _excavatorid = value; } }
        public string OreTypeId { get { return _oretypeid; } set { _oretypeid = value; } }
        public double OreWeight { get { return _oreweight; } set { _oreweight = value; } }
    }

}
