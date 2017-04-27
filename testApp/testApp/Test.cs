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
            List<string> messages = new List<string>();

            

            messages.Add("deviceID:'804',timestamp:1492662332,statusCode:63645,latitude:43.235660807291666,longitude:76.87373860677083,speedKPH:0.0,heading:0.0,altitude:822.0");
            messages.Add("deviceID:'804',timestamp:1492662337,statusCode:63645,latitude:43.235742187499994,longitude:76.87369791666667,speedKPH:13.1,heading:0.0,altitude:822.0");
            messages.Add("deviceID:'804',timestamp:1492662347,statusCode:63645,latitude:43.235449218750006,longitude:76.87360026041668,speedKPH:19.5,heading:0.0,altitude:821.0");
            messages.Add("deviceID:'804',timestamp:1492758514,statusCode:63645,latitude:43.235660807291666,longitude:76.87373860677083,speedKPH:0.0,heading:0.0,altitude:822.0");
            messages.Add("deviceID:'111',timestamp:1492758510,transportStatus:1011,oreType:2");
            messages.Add("deviceID:'804',timestamp:1492662357,statusCode:63645,latitude:43.23486328125001,longitude:76.87309570312499,speedKPH:51.3,heading:0.0,altitude:823.0");
            messages.Add("deviceID:'804',timestamp:1492662378,statusCode:63645,latitude:43.231127929687496,longitude:76.86805826822918,speedKPH:59.3,heading:0.0,altitude:832.0");
            messages.Add("deviceID:'804',timestamp:1492662404,statusCode:63645,latitude:43.2277587890625,longitude:76.863623046875,speedKPH:1.4,heading:0.0,altitude:844.0");
            messages.Add("deviceID:'804',timestamp:1492662414,statusCode:63645,latitude:43.229101562500006,longitude:76.86548665364585,speedKPH:72.0,heading:0.0,altitude:837.0");

            //TestSyncZoneExcv();
            System.Diagnostics.Debug.WriteLine("\nstart()\n");
            string responce = string.Empty;
            foreach(var s in messages)
            {
                responce = recipient.AddMessage(s);
                System.Diagnostics.Debug.WriteLine(responce);
            }
                
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
