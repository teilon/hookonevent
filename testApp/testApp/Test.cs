using calcevent;
using calcevent.dump;
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
        public GeozoneList ZoneList { get { return _gzl; } }
        public TransportList TransportList { get { return _tl; } }
        
        public Test()
        {
            addZoneList(GetFromDB.Zones());
            addTransportList(GetFromDB.Transports());
        }
        public void addZoneList(GeozoneList gzl)
        {
            _gzl = gzl;
        }
        public void addTransportList(TransportList tl)
        {
            _tl = tl;
        }
    }
}
