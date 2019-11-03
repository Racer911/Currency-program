using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Currency
{
    public class Convertation
    {
        public static decimal ConvertCurrency(decimal xToRuble, decimal yToRuble, decimal current = 1)
        {
            return current * xToRuble / yToRuble;
        }
    }
}
