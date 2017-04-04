using calcevent.progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace calcevent
{
    public class MessageRecipient
    {
        TransportMonitor _tm = new TransportMonitor();
        public TransportMonitor TransportMonitor { get { return _tm; } }

        enum Device { none, android, fortold, fort };

        string pattern_fort = "deviceID:[\"'][\\w\\d_-]+[\"'],timestamp:\\d+,statusCode:[\\w\\d_-]+,latitude:\\d+\\.?\\d+,longitude:\\d+\\.?\\d+,speedKPH:\\d+\\.?\\d*,heading:\\d+\\.?\\d*,altitude:\\d+\\.?\\d*";
        string pattern_tabl = "(deviceID|transportID):[\"'][\\w\\d_-]+[\"'],timestamp:\\d+,(driverMessage|transportStatus):\\d{4},oreType:\\d+";
        
        public void addTransportList(List<TransportItem> trp)
        {
            _tm.AddTransport(trp);
        }
        public void addZoneList(List<ZoneItem> znp)
        {
            _tm.AddZone(znp);
        }

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
            
            _tm.AddMessage(deviceID, timestamp, driverMessage, oreType);

            return result;
        }
        string fromFortDevice(string input)
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
            string statusCode = a[2].Split(':')[1];
            double latitude = double.Parse(a[3].Split(':')[1].Replace('.', ','));
            double longitude = double.Parse(a[4].Split(':')[1].Replace('.', ','));
            double speedKPH = double.Parse(a[5].Split(':')[1].Replace('.', ','));
            double heading = double.Parse(a[6].Split(':')[1].Replace('.', ','));
            double altitude = double.Parse(a[7].Split(':')[1].Replace('.', ','));

            _tm.AddMessage(deviceID, timestamp, statusCode, latitude, longitude, speedKPH, heading, altitude);

            return result;
        }
    }
}
