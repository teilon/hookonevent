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
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> Items;// = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
        enum Device { none, android, fortold, fort };

        string pattern_fort = "deviceID:[\"'][\\w\\d_-]+[\"'],timestamp:\\d+,statusCode:[\\w\\d_-]+,latitude:\\d+\\.?\\d+,longitude:\\d+\\.?\\d+,speedKPH:\\d+\\.?\\d*,heading:\\d+\\.?\\d*,altitude:\\d+\\.?\\d*";
        string pattern_tabl = "(deviceID|transportID):[\"'][\\w\\d_-]+[\"'],timestamp:\\d+,(driverMessage|transportStatus):\\d{4},oreType:\\d+";
        
        public void AddMessage(string input)
        {
            string result = string.Empty;

            string pattern = string.Format("{0}|{1}", pattern_tabl, pattern_fort);

            foreach (Match m in Regex.Matches(input, pattern))
            {
                if (result != string.Empty) result += ",";
                Device _device = deviceIs(m.Value);
                switch (_device)
                {
                    case Device.android:
                        result += fromAndroidDevice(m.Value); break;
                    case Device.fort:
                        result += fromFortDevice(m.Value); break;
                }
            }
        }
        Device deviceIs(string input)
        {
            Regex reg;
            reg = new Regex(pattern_tabl);
            if (reg.IsMatch(input))
                return Device.android;
            reg = new Regex(pattern_fort);
            if (reg.IsMatch(input))
                return Device.fort;
            return Device.none;
        }
        string fromAndroidDevice(string input)
        {
            string result = string.Empty;
            if (input == null)
                return result;

            var a = input.Split(',');
            string deviceID = a[0].Split(':')[1];
            if (deviceID.Contains("\'"))
                deviceID = deviceID.Split('\'')[1];
            else
                deviceID = deviceID.Split('\"')[1];
            string timestamp = a[1].Split(':')[1];
            string driverMessage = a[2].Split(':')[1];
            string oreType = a[3].Split(':')[1];

            if (!Items.ContainsKey(deviceID))
                Items[deviceID] = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, Dictionary<string, string>> _item;
            _item = Items[deviceID];
            if (!_item.ContainsKey("base"))
                _item["base"] = new Dictionary<string, string>();
            if (!_item.ContainsKey("curdata"))
                _item["curdata"] = new Dictionary<string, string>();
            if (!_item.ContainsKey("eventkey"))
                _item["eventkey"] = new Dictionary<string, string>();

            Dictionary<string, string> _curdata = _item["curdata"];
            _curdata["timestamp"] = timestamp;
            _curdata["event"] = driverMessage;





            //result = _state.SetHardMessage(deviceID, int.Parse(timestamp), driverMessage, int.Parse(oreType));
            return result;
        }
        static string fromFortDevice(string input)
        {
            string result = string.Empty;
            if (input == null)
                return result;

            var a = input.Split(',');
            string deviceID = a[0].Split(':')[1];
            if (deviceID.Contains("\'"))
                deviceID = deviceID.Split('\'')[1];
            else
                deviceID = deviceID.Split('\"')[1];
            int timestamp = int.Parse(a[1].Split(':')[1]);
            string statusCode = a[2].Split(':')[1];
            double latitude = double.Parse(a[3].Split(':')[1].Replace('.', ','));
            double longitude = double.Parse(a[4].Split(':')[1].Replace('.', ','));
            double speedKPH = double.Parse(a[5].Split(':')[1].Replace('.', ','));
            double heading = double.Parse(a[6].Split(':')[1].Replace('.', ','));
            double altitude = double.Parse(a[7].Split(':')[1].Replace('.', ','));

            //string _device = devices.ContainsKey(deviceID) ? devices[deviceID] : deviceID.Substring(0, 3);
            //result = _state.SetMessage_plus(_device, timestamp, statusCode, latitude, longitude, timestamp, (int)speedKPH);
            //
            var nowtime = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

            return result;
        }
    }
}
