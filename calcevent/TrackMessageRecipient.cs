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

        bool _saveit = false;
        Dictionary<string, string> _tosave = new Dictionary<string, string>();
        public bool SaveIt { get { return GetSaveCheck(); } }
        public Dictionary<string, string> OutputDict { get { return _tosave; } }

        string pattern_fort = "deviceID:[\"\'][\\w\\d_-]+[\"\'],timestamp:\\d+,statusCode:[\\w\\d_-]+,latitude:\\d+\\.?\\d+,longitude:\\d+\\.?\\d+,speedKPH:\\d+\\.?\\d*,heading:\\d+\\.?\\d*,altitude:\\d+\\.?\\d*";
        //[{                   deviceID:'804',                timestamp:1492172034,statusCode:63646,latitude:43.236474609375,longitude:76.8740234375,speedKPH:1.2,      heading:345.0,      altitude:804.0}]
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
            if (input == null)
                return "";

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
                                        
            return GetResult(deviceID);
        }
        string GetResult(string deviceID)
        {
            TransportItem _ti;
            try
            {
                _ti = _tm[deviceID];
            }
            catch (ArgumentNullException e)
            {
                TXTWriter.Write(string.Format("on deviceId:{0} catch {1}\n", deviceID, e.Message));
                return string.Format("{{transportId:,statuscode:,timestamp:,oretype:}}");
            }
            
            if (_ti.SaveIt)
            {
                TransportEventKey _ek = _ti.LastKeyEvent;
                PreSave(_ek.TruckId, Translator.KeyToDBKey[_ek.EventId], _ek.ZoneId, _ek.ExcavatorId, _ek.OreTypeId, _ek.OreWeight, _ek.Timestamp);
            }

            string _result = string.Format("\"timestamp\":{0},\"transportID\":\"{1}\",\"transportStatus\":{2},\"oreType\":{3},\"rockMass\":{4}",
                _ti.CurrentTimeStamp, _ti.TransportId, Translator.KeyToDBKey[_ti.LastEventId], _ti.CurrentOreType, _ti.OreWeight);

            return "{" + _result + "}";
        }


        bool GetSaveCheck()
        {
            bool _result = _saveit;
            _saveit = false;
            return _result;
        }
        void PreSave(string truckid, string eventid, string zoneid, string excavatorid, string oretypeid, double oreweight, string timestamp)
        {            
            _tosave["truckid"] = truckid;
            _tosave["eventid"] = eventid;
            _tosave["zoneid"] = zoneid;
            _tosave["excavatorid"] = excavatorid;
            _tosave["oretypeid"] = oretypeid;
            _tosave["oreweight"] = oreweight.ToString();
            _tosave["timestamp"] = timestamp;
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
            double latitude = double.Parse(a[3].Split(':')[1].Replace('.', ','));
            double longitude = double.Parse(a[4].Split(':')[1].Replace('.', ','));
            double speedKPH = double.Parse(a[5].Split(':')[1].Replace('.', ','));
            double heading = double.Parse(a[6].Split(':')[1].Replace('.', ','));
            double altitude = double.Parse(a[7].Split(':')[1].Replace('.', ','));
            
            _tm.AddMessage(deviceID, timestamp, statusCode, latitude, longitude, speedKPH, heading, altitude);

            return GetResult(deviceID);
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
            
            return GetResult(deviceID);
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
