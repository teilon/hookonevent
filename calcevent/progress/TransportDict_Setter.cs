using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace calcevent.progress
{
    public partial class TransportDict
    {
        public void AddTransports(Dictionary<string, Dictionary<string, Dictionary<string, string>>> transportsdict)
        {
            Items = transportsdict;
        }
    }
}
