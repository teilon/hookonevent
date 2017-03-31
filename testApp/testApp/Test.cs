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
        TransportProgress _trp;
        public TransportProgress TransportProgress { get { return _trp; } }
        ZoneProgress _znp;
        public ZoneProgress ZoneProgress { get { return _znp; } }

        public Test()
        {
            MessageRecipient recipient = new MessageRecipient();
            recipient.addTransportList(GetFromDB.ToTransportProgress());
            _trp = recipient.TransportProgress;
            recipient.addZoneList(GetFromDB.ToZoneProgress());
            _znp = recipient.ZoneProgress;
        }
        public void Start()
        {
            
            TestTransports();
            
            TestZones();
            
            TestStates();

            TransportProgress["804"].ToMove();
            TransportProgress["806"].ToMove();
            TransportProgress["809"].ToMove();
            TransportProgress["804"].ToMove();
            TransportProgress["10"].ToMove();
            TransportProgress["806"].ToStop();
            TransportProgress["804"].ToStop();

            TestStates();
        }
        void TestTransports()
        {
            System.Diagnostics.Debug.WriteLine("\nTransports:");
            foreach (var t in TransportProgress.Items)
            {
                System.Diagnostics.Debug.WriteLine("TransportId:{0}, ParkNumber:{1}, ModelId:{2}, TypeId:{3}, LastLatitude:{4}, LastLongitude:{5}, LastTimeStamp:{6}, LastZone:{7}", 
                    t.TransportId, t.ParkNumber, t.ModelId, t.TypeId, t.CurrentLatitude, t.CurrentLongitude, t.CurrentTimeStamp, t.CurrentZone);                
            }
        }
        void TestZones()
        {
            System.Diagnostics.Debug.WriteLine("\nZones:");
            foreach (var z in ZoneProgress.Items)
            {
                System.Diagnostics.Debug.WriteLine("ZoneId:{0}, DisplayName:{1}, Type:{2}",
                    z.Id, z.DisplayName, z.Type);
            }
        }
        void TestStates()
        {
            System.Diagnostics.Debug.WriteLine("\nTransport states:");
            string output = TransportProgress.GetCurrentStates();
            System.Diagnostics.Debug.WriteLine(output);
        }
    }
}
