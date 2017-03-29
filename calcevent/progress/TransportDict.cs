using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.progress
{
    public class TransportDict
    {        
        public Dictionary<string, Dictionary<string, string>> Item = new Dictionary<string, Dictionary<string, string>>();


        string pattern_fort = "deviceID:[\"'][\\w\\d_-]+[\"'],timestamp:\\d+,statusCode:[\\w\\d_-]+,latitude:\\d+\\.?\\d+,longitude:\\d+\\.?\\d+,speedKPH:\\d+\\.?\\d*,heading:\\d+\\.?\\d*,altitude:\\d+\\.?\\d*";
        string pattern_tabl = "(deviceID|transportID):[\"'][\\w\\d_-]+[\"'],timestamp:\\d+,(driverMessage|transportStatus):\\d{4},oreType:\\d+";
        public void AddMessage(string input)
        {

        }
    }
}
