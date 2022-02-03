using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Parser
{
    public class LineDataModel
    {
        public DateTime Timestamp { get; set; }
        public string Sender { get; set; }
        public string Language { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
