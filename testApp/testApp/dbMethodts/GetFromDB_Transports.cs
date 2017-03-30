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
        public static Dictionary<string, Dictionary<string, Dictionary<string, string>>> TransportsDict()
        {
            Dictionary<string, string> basedata = new Dictionary<string, string>();
            Dictionary<string, string> curdata = new Dictionary<string, string>();
            Dictionary<string, Dictionary<string, Dictionary<string, string>>> result = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

            using (var db = new dbEntities())
            {
                foreach (var t in db.vDumps)
                {
                    if (!result.ContainsKey(t.TransportId))
                        result[t.TransportId] = new Dictionary<string, Dictionary<string, string>>();
                    if (!result[t.TransportId].ContainsKey("base"))
                        result[t.TransportId]["base"] = new Dictionary<string, string>();
                    if (!result[t.TransportId].ContainsKey("curdata"))
                        result[t.TransportId]["curdata"] = new Dictionary<string, string>();

                    //result[t.TransportId]["base"] = basedata;
                    //result[t.TransportId]["curdata"] = curdata;

                    result[t.TransportId]["base"]["id"] = t.TransportId;
                    result[t.TransportId]["base"]["park"] = t.ParkNumber;
                    result[t.TransportId]["base"]["type"] = t.TypeId;
                    result[t.TransportId]["base"]["model"] = t.ModelId;

                    result[t.TransportId]["curdata"]["lon"] = t.LastLongitude.ToString();
                    result[t.TransportId]["curdata"]["lat"] = t.LastLatitude.ToString();
                }
            }
            return result;
        }
    }
}
