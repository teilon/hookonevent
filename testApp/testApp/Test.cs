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
            recipient.AddTransportList(GetFromDB.ToTransportProgress());
            recipient.AddZoneList(GetFromDB.ToZoneProgress());
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

            List<string> messages = new List<string>();
            messages.Add("deviceID:'804',timestamp:1654892,statusCode:60012,latitude:43.568945,longitude:72.35687,speedKPH:2,heading:12.0,altitude:120");
            messages.Add("deviceID:'804',timestamp:1654902,statusCode:60012,latitude:43.568966,longitude:72.35632,speedKPH:2,heading:12.0,altitude:120");
            messages.Add("deviceID:'804',timestamp:1654912,statusCode:60012,latitude:43.568966,longitude:72.35632,speedKPH:0,heading:12.0,altitude:120");
            messages.Add("deviceID:'804',timestamp:1654922,statusCode:60012,latitude:43.569002,longitude:72.35655,speedKPH:2,heading:12.0,altitude:120");
            messages.Add("deviceID:'804',timestamp:1654932,statusCode:60012,latitude:43.569123,longitude:72.35601,speedKPH:2,heading:12.0,altitude:120");

            TestSyncZoneExcv();
            foreach(var s in messages)
                recipient.AddMessage(s);
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
