using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.dump
{
    public class TransportList
    {
        List<Transport> _transports = new List<Transport>();
        public List<Transport> Transports { get { return _transports; } }
        public Transport this[string transportId] { get { return _transports.Where(x => x.Id == transportId).FirstOrDefault(); } }
        public void AddTransport(Transport t)
        {
            _transports.Add(t);
        }
    }
    public class Transport
    {
        string _id = "";
        string _parknumber = "";
        string _typeid = "";
        string _modelid = "";
        double _lastlongitude = 0;
        double _lastlatitude = 0;
        public string Id { get { return _id; } set { if (_id == "") _id = value; } }
        public string ParkNumber { get { return _parknumber; } set { _parknumber = value; } }
        public string TypeId { get { return _typeid; } set { _typeid = value; } }
        public string ModelId { get { return _modelid; } set { _modelid = value; } }
        public double LastLongitude { get { return _lastlongitude; } set { _lastlongitude = value; } }
        public double LastLatitude { get { return _lastlatitude; } set { _lastlatitude = value; } }

        public Transport(string id)
        {
            _id = id;
        }
    }
}
