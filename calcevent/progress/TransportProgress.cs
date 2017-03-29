using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.progress
{
    public class TransportProgress
    {
        List<TransportItem> _transports = new List<TransportItem>();
        public List<TransportItem> Transports { get { return _transports; } }
        public TransportItem this[string transportId] { get { return _transports.Where(x => x.TransportId == transportId).FirstOrDefault(); } }
                
    }
    
    public class TransportItem
    {
        string _transportid = "";
        string _parknumber = "";
        string _modelid = "";
        string _typeid = "";

        double _lastlongitude = 0;
        double _lastlatitude = 0;
        int _lasttimestamp = 0;
        string _lastzone = "";

        string _lasteventid = "";
        TransportEventKey _lastkeyevent = new TransportEventKey();

        public string TransportId { get { return _transportid; } set { if (_transportid == "") _transportid = value; } }
        public string ParkNumber { get { return _parknumber; } set { if (_parknumber == "") _parknumber = value; } }
        public string ModelId { get { return _modelid; } set { if (_modelid == "") _modelid = value; } }
        public string TypeId { get { return _typeid; } set { if (_typeid == "") _typeid = value; } }

        public double LastLongitude { get { return _lastlongitude; } set { _lastlongitude = value; } }
        public double LastLatitude { get { return _lastlatitude; } set { _lastlatitude = value; } }
        public int LastTimeStamp { get { return _lasttimestamp; } set { _lasttimestamp = value; } }
        public string LastZone { get { return _lastzone; } set { _lastzone = value; } }

        public string LastEventId { get { return _lasteventid; } set { _lasteventid = value; } }
        public TransportEventKey LastKeyEvent { get { return _lastkeyevent; } }

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
