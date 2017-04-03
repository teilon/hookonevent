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

            TransportProgress["804"].CurrentState.ToMove();
            TransportProgress["806"].CurrentState.ToMove();
            TransportProgress["809"].CurrentState.ToMove();
            TransportProgress["804"].CurrentState.ToMove();
            TransportProgress["10"].CurrentState.ToMove();
            TransportProgress["806"].CurrentState.ToStop();
            TransportProgress["804"].CurrentState.ToStop();
            TransportProgress["806"].CurrentState.ToMove();
            

            TestStates();
        }
        void TestTransports()
        {
            System.Diagnostics.Debug.WriteLine("\nTransports:");
            foreach (var t in TransportProgress.Items)
            {
                System.Diagnostics.Debug.WriteLine("TransportId:{0}, ParkNumber:{1}, ModelId:{2}, TypeId:{3}, LastLatitude:{4}, LastLongitude:{5}, LastTimeStamp:{6}", 
                    t.TransportId, t.ParkNumber, t.ModelId, t.TypeId, t.CurrentLocation.Latitude, t.CurrentLocation.Longitude, t.CurrentTimeStamp);                
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
            string output = GetCurrentStates();
            System.Diagnostics.Debug.WriteLine(output);
        }
        string GetCurrentStates()
        {
            string result = string.Empty;
            foreach (var item in TransportProgress.Items)
            {
                string id = item.TransportId;
                result += string.Format("{0,3}: {1}\n", id, TransportProgress[id].CurrentState.GetCurrentState());
            }
            return result;
        }
    }
}
