using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace calcevent.progress
{
    public partial class TransportDict
    {
        //string pattern_fort = "deviceID:[\"'][\\w\\d_-]+[\"'],timestamp:\\d+,statusCode:[\\w\\d_-]+,latitude:\\d+\\.?\\d+,longitude:\\d+\\.?\\d+,speedKPH:\\d+\\.?\\d*,heading:\\d+\\.?\\d*,altitude:\\d+\\.?\\d*";
        //string pattern_tabl = "(deviceID|transportID):[\"'][\\w\\d_-]+[\"'],timestamp:\\d+,(driverMessage|transportStatus):\\d{4},oreType:\\d+";

        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> Items;// = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
        
        public void AddMessage(string deviceId, string timestamp, string statuscode, string oreType)
        {
            if (!Items.ContainsKey(deviceId))
                Items[deviceId] = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> _item;
            _item = Items[deviceId];
            if (!_item.ContainsKey("base"))
                _item["base"] = new Dictionary<string, string>();
            if (!_item.ContainsKey("curdata"))
                _item["curdata"] = new Dictionary<string, string>();
            if (!_item.ContainsKey("eventkey"))
                _item["eventkey"] = new Dictionary<string, string>();

            Dictionary<string, string> _curdata = _item["curdata"];
            _curdata["timestamp"] = timestamp;
            _curdata["event"] = statuscode;

        }
        public void AddMessage(string deviceId, int timestamp, int statuscode, double latitude, double longitude, double speedKPH, double heading, double altitude)
        {

        }
    }
}