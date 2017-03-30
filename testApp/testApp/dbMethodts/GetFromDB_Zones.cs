using calcevent.dump;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testApp
{
    public static partial class GetFromDB
    {
        public static GeozoneList Zones()
        {
            List<_Zone> _zonelist = new List<_Zone>();
            using (var db = new dbEntities())
            {
                _zonelist = db.vZones.Select(x => new _Zone()
                {
                    ZoneId = x.ZoneId,
                    ZoneName = x.ZoneName,
                    ZoneTypeId = (int)x.ZoneTypeId,
                    //ZoneTypeName = x.ZoneTypeName,
                    x = (double)x.x,
                    y = (double)x.y,
                    //z = (double)x.z
                }).ToList();
            }
            
            string currentzone = string.Empty;
            GeozoneList gzl = new GeozoneList();
            Geozone zone = new Geozone();
            foreach (var z in _zonelist)
            {                
                if (z.ZoneId != currentzone && currentzone != string.Empty)
                {
                    gzl.addZone(zone);
                    currentzone = z.ZoneId;
                    zone = new Geozone(z.ZoneId);
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
                zone.Points.Add(new Geopoint(z.x, z.y));
            }
            
            return gzl;
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
