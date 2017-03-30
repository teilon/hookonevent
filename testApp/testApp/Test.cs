using calcevent;
using calcevent.dump;
using calcevent.progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testApp
{
    class Test
    {
        GeozoneList _gzl;
        TransportList _tl;
        TransportDict _td;
        public GeozoneList ZoneList { get { return _gzl; } }
        public TransportList TransportList { get { return _tl; } }
        public TransportDict TransportDict { get { return _td; } }

        public Test()
        {
            //addZoneList(GetFromDB.Zones());
            //addTransportList(GetFromDB.Transports());
            addTransportDict(GetFromDB.TransportsDict());
        }
        public void addZoneList(GeozoneList gzl)
        {
            _gzl = gzl;
        }
        public void addTransportList(TransportList tl)
        {
            _tl = tl;
        }
        public void addTransportDict(Dictionary<string, Dictionary<string, Dictionary<string, string>>> tl)
        {
            _td = new TransportDict();
            _td.Items = tl;
        }
        public void TestLists()
        {
            foreach (var z in ZoneList.Zones)
            {
                Console.WriteLine("id:{0}, name:{1}, type:{2}, count of point:{3}", z.Id, z.DisplayName, z.Type, z.Points.Count);
            }
            Console.WriteLine("\n");
            foreach (var t in TransportList.Transports)
            {
                Console.WriteLine("id:{0}, park number:{1}, type id:{2}, model id:{3}, last longitude:{4}, last latitude:{5}", t.Id, t.ParkNumber, t.TypeId, t.ModelId, t.LastLongitude, t.LastLatitude);
            }
        }
        public void TestDict()
        {
            foreach(var i in TransportDict.Items)
            {
                Console.WriteLine("device: {0}", i.Key);
                Dictionary<string, Dictionary<string, string>> _item = i.Value;
                foreach (var b in _item.Keys)
                {
                    if (b == "base")
                        Console.Write("{0}; {1}; {2}; {3};", _item[b]["id"], _item[b]["park"], _item[b]["type"], _item[b]["model"]);
                    if (b == "curdata")
                        Console.WriteLine("{0}; {1};", _item[b]["lat"], _item[b]["lon"]);
                }
            }
        }
    }
}
