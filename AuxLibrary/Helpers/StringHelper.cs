using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Currency
{
    public static class StringHelper
    {
        public static bool ContainsIgnoreCase(this string source, string toCheck)
        {
            return source?.IndexOf(toCheck, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        public static bool IsDecimal(this string source)
        {
            return decimal.TryParse(source, out decimal res) || string.IsNullOrEmpty(source);
        }

        public static string AddRemoveFirstZero(this string source)
        {
            var chars = source.ToCharArray();
            if (chars.Length > 0 && chars[0] == ',')
                return "0" + source;

            else if (chars.Length > 1 && chars[0] == '0' && chars[1] != ',')
                return source.Substring(1);

            return source;
        }
    }
}
