using calcevent;
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
        MessageRecipient recipient;

        public Test()
        {
            recipient = new MessageRecipient();
            recipient.addTransportList(GetFromDB.ToTransportProgress());
            recipient.addZoneList(GetFromDB.ToZoneProgress());
        }
        public void Start()
        {
            /*
            TestTransports();
            
            TestZones();
            
            TestStates();

            TransportProgress["804"].CurrentState.ToMove();
            TransportProgress["806"].CurrentState.ToMove();
            TransportProgress["809"].CurrentState.ToMove();
            TransportProgress["804"].CurrentState.ToMove();
            TransportProgress["10"].CurrentState.ToMove();
            TransportProgress["806"].CurrentState.ToStop();
            TransportProgress["804"].CurrentState.ToStop();
            TransportProgress["806"].CurrentState.ToMove();
            
            TestStates();
            */

            TestSyncZoneExcv();
        }        
        void TestSyncZoneExcv()
        {
            System.Diagnostics.Debug.WriteLine("Test Sync Zones with Excavators");

            foreach (var z in recipient.TransportMonitor.Zones)
            {
                System.Diagnostics.Debug.WriteLine("zone id:{0}; display name:{1}, zone type:{2}, excavator id:{3}", z.Id, z.DisplayName, z.Type, z.ExcavatorId);
            }
        }
    }
}
