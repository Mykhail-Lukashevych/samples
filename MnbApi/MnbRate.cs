
namespace MnbSoapApi
{
    public class MnbRate
    {
        public string Currency { get; set; }
        public int Unit { get; set; }
        public double Value { get; set; }

        public override string ToString()
        {
            return string.Format("Currency: {0}; Unit: {1}; Value: {2}", Currency, Unit, Value);
        }
    }
}
