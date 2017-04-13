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
        string pattern_fort_old = "deviceID:[\"'][\\w\\d_-]+[\"'],\\s?timestamp:\\d+,\\s?latitude:\\d+\\.?\\d+,\\s?longitude:\\d+\\.?\\d+,\\s?speedKPH:\\d+\\.?\\d*,\\s?heading:\\d+\\.?\\d*,\\s?altitude:\\d+\\.?\\d*";
        //[{                       deviceID:'353218074563660', timestamp:1492062675, latitude:52.54724934895834, longitude:62.750284830729164, speedKPH:2.5, heading:218.0, altitude:202.0}]
        public void AddTransportList(List<TransportItem> trp)
        {
            _tm.AddTransport(trp);
        }
        public void AddZoneList(List<ZoneItem> znp)
        {
            _tm.AddZone(znp);
        }

        public string AddMessage(string input)
        {
            string result = string.Empty;

            string pattern = string.Format("{0}|{1}|{2}", pattern_tabl, pattern_fort, pattern_fort_old);

            foreach (Match m in Regex.Matches(input, pattern))
            {
                if (result != string.Empty) result += ",";
                Device _device = DeviceIs(m.Value);
                switch (_device)
                {
                    case Device.android:
                        result += FromAndroidDevice(m.Value); break;
                    case Device.fort:
                        result += FromFortDevice(m.Value); break;
                    case Device.fortold:
                        result += FromOldFortDevice(m.Value); break;
                    default:
                        TXTWriter.Write(string.Format("is {0} message pattern\n", "unknow")); break;
                }
            }            

            return result;
        }
        Device DeviceIs(string input)
        {
            Regex reg;
            reg = new Regex(pattern_tabl);
            if (reg.IsMatch(input))
                return Device.android;
            reg = new Regex(pattern_fort);
            if (reg.IsMatch(input))
                return Device.fort;
            reg = new Regex(pattern_fort_old);
            if (reg.IsMatch(input))
                return Device.fortold;
            return Device.none;
        }
        string FromAndroidDevice(string input)
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
            
            _tm.AddMessage(deviceID, timestamp, Translator.DBKeyToKey[driverMessage], oreType);

            TransportItem _ti = _tm[deviceID];
            result = string.Format("{{transportId:{0},statuscode:{1},timestamp:{2},oretype:{3}}}",
                _ti.TransportId, _ti.LastEventId, _ti.CurrentTimeStamp, _ti.CurrentOreType);

            return result;
        }
        string FromFortDevice(string input)
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
            double latitude = double.Parse(a[3].Split(':')[1]);//.Replace('.', ','));
            double longitude = double.Parse(a[4].Split(':')[1]);//.Replace('.', ','));
            double speedKPH = double.Parse(a[5].Split(':')[1]);//.Replace('.', ','));
            double heading = double.Parse(a[6].Split(':')[1]);//.Replace('.', ','));
            double altitude = double.Parse(a[7].Split(':')[1]);//.Replace('.', ','));

            _tm.AddMessage(deviceID, timestamp, statusCode, latitude, longitude, speedKPH, heading, altitude);

            TransportItem _ti = _tm[deviceID];
            result = string.Format("{{transportId:{0},statuscode:{1},timestamp:{2},oretype:{3}}}", 
                _ti.TransportId, _ti.LastEventId, _ti.CurrentTimeStamp, _ti.CurrentOreType);

            return result;
        }
        string FromOldFortDevice(string input)
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
            double latitude = double.Parse(a[2].Split(':')[1].Replace('.', ','));
            double longitude = double.Parse(a[3].Split(':')[1].Replace('.', ','));
            double speedKPH = double.Parse(a[4].Split(':')[1].Replace('.', ','));
            double heading = double.Parse(a[5].Split(':')[1].Replace('.', ','));
            double altitude = double.Parse(a[6].Split(':')[1].Replace('.', ','));

            string _statusCode = "-1";
            string device = (devices.ContainsKey(deviceID)) ? devices[deviceID] : deviceID;
            
            _tm.AddMessage(device, timestamp, _statusCode, latitude, longitude, speedKPH, heading, altitude);
            
            TransportItem _ti = _tm[device];
            result = string.Format("{{transportId:{0},statuscode:{1},timestamp:{2},oretype:{3}}}",
                _ti.TransportId, _ti.LastEventId, _ti.CurrentTimeStamp, _ti.CurrentOreType);
            
            return result;
        }
        Dictionary<string, string> devices = new Dictionary<string, string>()
        {
            { "354868056800875", "800"},
            { "354868056817531", "801"},
            { "354868053058162", "803"},
            { "354868052847854", "804"},
            { "354868056786017", "805"},    //354868056786017 805  
            { "354868053037521", "806"},    //354868053037521   806
            { "354868053058741", "807"},    //354868053058741   807
            { "354868053058097", "809"},
            { "354868053043008", "810"},

            { "354868053063915", "134"},
            { "354868056852009", "157"},
            { "354868056764980", "10"},     //354868056764980   10
            { "354868056660410", "5"},
            { "354868056785944", "11"},     //354868056785944   11
            { "354868054433877", "125"},
            { "354868056800966", "138"},
            { "354868056817085", "70"},
            { "354868053043107", "13"},
            { "354868053063956", "66"},     //354868053058204
            { "354868056851860", "12"},
            { "354868056786330", "71"},     //354868056786330
            { "354868053058337", "83"},
            { "354868052961648", "39"}      //354868052961648
                                            //354868053099877
                                            //354868052962117
                                            //354868053058287
                                            //354868053057776
                                            //354868053058071
                                            //353218074563660
                                            //354868053057933
        };
    }
}
