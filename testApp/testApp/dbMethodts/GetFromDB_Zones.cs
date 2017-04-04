using calcevent.progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testApp
{
    public static partial class GetFromDB
    {
        public static List<ZoneItem> ToZoneProgress()
        {
            List<_Zone> _zonelist = new List<_Zone>();
            using (var db = new dbEntities())
            {
                _zonelist = db.vZones.Select(x => new _Zone()
                {
                    ZoneId = x.ZoneId,
                    ZoneName = x.ZoneName,
                    ZoneTypeId = (int)x.ZoneTypeId,
                    x = (double)x.x,
                    y = (double)x.y,
                }).ToList();
            }

            string currentzone = string.Empty;
            List<ZoneItem> znplist = new List<ZoneItem>();
            ZoneItem zone = new ZoneItem();
            foreach (var z in _zonelist)
            {
                if (z.ZoneId != currentzone && currentzone != string.Empty)
                {
                    znplist.Add(zone);
                    currentzone = z.ZoneId;
                    zone = new ZoneItem(z.ZoneId);
                    zone.DisplayName = z.ZoneName;
                    zone.Type = z.ZoneTypeId;
                }
                if (currentzone == string.Empty)
                {
                    currentzone = z.ZoneId;
                    zone.Id = z.ZoneId;
                    zone.DisplayName = z.ZoneName;
                    zone.Type = z.ZoneTypeId;
                }
                zone.AddPoint(z.x, z.y);
            }

            return znplist;
        }
        class _Zone
        {
            public string ZoneId;
            public string ZoneName;
            public int ZoneTypeId;
            public double x;
            public double y;
        }
    }
}
