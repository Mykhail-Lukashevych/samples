using System;
using System.Collections.Generic;
using System.Text;

namespace MnbSoapApi
{
    public class MnbDay
    {
        public MnbDay()
        {
            Rates = new List<MnbRate>();
        }
        public DateTime Date { get; set; }
        public List<MnbRate> Rates { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach(var rate in Rates)
            {
                sb.Append("\n\t" + rate.ToString());
            }
            return string.Format("Date: {0}; Rates: {1}", Date.ToShortDateString(), sb.ToString());
        }
    }
}
