using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent
{
    public static class Translator
    {
        public static Dictionary<string, string> DBKeyToKey = new Dictionary<string, string>()
        {
            { "1001", "UU"},
            { "1002", "PP"},
            { "1003", "LM"},
            { "1004", "UM"},
            { "1005", "PM"},
            { "1006", "UZ"},
            { "1007", "NN"},
            { "1008", "OO"},
            { "1009", "OM"},
    
            { "1010", "NM"},
            { "1011", "LL"},
            { "1012", "LZ"},
            { "1099", "NH"}
        };
        public static Dictionary<string, string> KeyToDBKey = new Dictionary<string, string>()
        {
            { "UU", "1001"},
            { "PP", "1002"},
            { "LM", "1003"},
            { "UM", "1004"},
            { "PM", "1005"},
            { "UZ", "1006"},
            { "NN", "1007"},
            { "OO", "1008"},
            { "OM", "1009"},

            { "NM", "1010"},
            { "LL", "1011"},
            { "LZ", "1012"},
            { "NH", "1099"}
        };
    }    
}
