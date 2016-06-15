using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace MnbSoapApi
{
    internal class MnbResponseParser
    {
        private string CURR_ATTRIBUTE_NAME = "curr";
        private string DATE_ATTRIBUTE_NAME = "date";
        private string UNIT_ATTRIBUTE_NAME = "unit";

        private double ParseValue(string input)
        {
            return Double.Parse(input.Replace(',', '.'));
        }

        public List<MnbDay> Parse(string source)
        {
            var days = new List<MnbDay>();

            using (var reader = XmlReader.Create(new StringReader(source)))
            {
                MnbDay day = null;

                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (day == null && String.Equals(reader.Name, "Day", StringComparison.OrdinalIgnoreCase))
                            {
                                day = new MnbDay
                                {
                                    Date = DateTime.Parse(reader[DATE_ATTRIBUTE_NAME])
                                };
                            }
                            if (day != null && String.Equals(reader.Name, "Rate", StringComparison.OrdinalIgnoreCase))
                            {
                                var rate = new MnbRate
                                {
                                    Currency = reader[CURR_ATTRIBUTE_NAME],
                                    Unit = Int32.Parse(reader[UNIT_ATTRIBUTE_NAME]),
                                };
                                if (reader.Read())
                                {
                                    rate.Value = ParseValue(reader.Value.Trim());
                                }
                                day.Rates.Add(rate);
                            }
                            break;
                        case XmlNodeType.EndElement:
                            if (day != null && String.Equals(reader.Name, "Day", StringComparison.OrdinalIgnoreCase))
                            {
                                days.Add(day);
                                day = null;
                            }
                            break;
                    }
                }
            }
            return days;
        }
    }
}
