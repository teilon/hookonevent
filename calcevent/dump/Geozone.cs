using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.dump
{
    public class GeozoneList
    {
        private List<Geozone> _zones = new List<Geozone>();
        public List<Geozone> Zones { get { return _zones; } }
        public Geozone this[string zoneID] { get { return _zones.Where(x => x.Id == zoneID).FirstOrDefault(); } }
        public GeozoneList()
        {
            //_zones = new List<Geozone>();
        }
        public void addZone(Geozone z)
        {
            _zones.Add(z);
        }
    }
    public class Geozone
    {
        string _id = "";
        string _displayName = "";
        int _type = -1;
        List<Geopoint> _points;
        public string Id { get { return _id; } set { if (_id == "") _id = value; } }
        public string DisplayName { get { return _displayName; } set { _displayName = value; } }
        public int Type { get { return _type; } set { _type = value; } }
        public List<Geopoint> Points { get { return _points; } }
        
        public Geozone()
        {
            _points = new List<Geopoint>();
        }
        public Geozone(string id)
            :this()
        {
            _id = id;
        }
    }
    public class Geopoint
    {
        double _x;
        double _y;
        public double X { get { return _x; } }
        public double Y { get { return _y; } }
        public Geopoint(double x, double y)
        {
            _x = x;
            _y = y;
        }
    }
}
