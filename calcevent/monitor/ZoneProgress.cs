using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.progress
{
    public class ZoneItem
    {
        string _id = "";
        string _displayName = "";
        int _type = -1;
        int _radius = 0;
        List<GeoCoordinate> _points;

        string _excavatorid = "";

        public string Id { get { return _id; } set { if (_id == "") _id = value; } }
        public string DisplayName { get { return _displayName; } set { _displayName = value; } }
        public int Type { get { return _type; } set { _type = value; } }
        public int Radius { get { return _radius; } set { _radius = value; } }
        public List<GeoCoordinate> Points { get { return _points; } }

        public string ExcavatorId { get { return _excavatorid; } set { if (_excavatorid == "") _excavatorid = value; } }
        public GeoCoordinate this[int index] { get { return _points[index]; } set { _points[index] = value; } }

        public ZoneItem()
        {
            _points = new List<GeoCoordinate>();
        }
        public ZoneItem(string id)
            :this()
        {
            _id = id;
        }
        public void AddPoint(double lon, double lat)
        {
            _points.Add(new GeoCoordinate(lat, lon));
        }
        
    }
}
