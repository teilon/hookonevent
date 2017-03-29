using calcevent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(TestEM.GetCurrentDate());

            Test test = new Test();
            
            
            foreach (var z in test.ZoneList.Zones)
            {
                Console.WriteLine("id:{0}, name:{1}, type:{2}, count of point:{3}", z.Id, z.DisplayName, z.Type, z.Points.Count);
            }
            Console.WriteLine("\n");
            foreach (var t in test.TransportList.Transports)
            {
                Console.WriteLine("id:{0}, park number:{1}, type id:{2}, model id:{3}, last longitude:{4}, last latitude:{5}", t.Id, t.ParkNumber, t.TypeId, t.ModelId, t.LastLongitude, t.LastLatitude);
            }

            //delay
            Console.WriteLine("\nend.");
            Console.ReadLine();
        }
        
    }
}
