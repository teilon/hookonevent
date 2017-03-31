using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.progress
{
    public class ZoneProgress
    {
        List<ZoneItem> _items;
        public List<ZoneItem> Items { get { return _items; } }
        public ZoneItem this[string zoneId] { get { return _items.Where(x => x.Id == zoneId).FirstOrDefault(); } }

        public ZoneProgress(List<ZoneItem> _list)
        {
            _items = _list;
        }
    }

    public class ZoneItem
    {
        string _id = "";
        string _displayName = "";
        int _type = -1;
        List<PointItem> _points;

        string _excavatorid = "";

        public string Id { get { return _id; } set { if (_id == "") _id = value; } }
        public string DisplayName { get { return _displayName; } set { _displayName = value; } }
        public int Type { get { return _type; } set { _type = value; } }
        public List<PointItem> Points { get { return _points; } }

        public string ExcavatorId { get { return _excavatorid; } set { if (_excavatorid == "") _excavatorid = value; } }

        public ZoneItem()
        {
            _points = new List<PointItem>();
        }
        public ZoneItem(string id)
            :this()
        {
            _id = id;
        }
        public void AddPoint(double x, double y)
        {
            _points.Add(new PointItem(x, y));
        }
        public class PointItem
        {
            double _x;
            double _y;
            public double X { get { return _x; } }
            public double Y { get { return _y; } }
            public PointItem(double x, double y)
            {
                _x = x;
                _y = y;
            }
        }
    }
    public class ZoneEventKey
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
