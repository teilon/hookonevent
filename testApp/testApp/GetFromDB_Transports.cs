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
        public static TransportList Transports()
        {
            TransportList _tl = new TransportList();
            using (var db = new dbEntities())
            {
                foreach(var t in db.vDumps)
                {
                    _tl.AddTransport(new Transport(t.TransportId)
                    {
                        ParkNumber = t.ParkNumber,
                        TypeId = t.TypeId,
                        ModelId = t.ModelId,
                        LastLongitude = (double)t.LastLongitude,
                        LastLatitude = (double)t.LastLatitude
                    });
                }
            }                       
            return _tl;
        }
    }
}
