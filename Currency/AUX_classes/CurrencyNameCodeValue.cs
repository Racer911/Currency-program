using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Currency
{
    public class CurrencyNameCodeValue
    {
        public string Name { get; set; }
        public string CharCode { get; set; }
        public decimal ValueToRUB { get; set; }
        public int Nominal { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is CurrencyNameCodeValue current))
                return false;

            return current.CharCode == this.CharCode;
        }

        public override int GetHashCode()
        {
            return $"{Name}{CharCode}{ValueToRUB}{Nominal}".GetHashCode();
        }

    }
}
