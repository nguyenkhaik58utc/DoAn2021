using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class Ultility
    {
        public static decimal? ConvertToDecimal(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            value = value.Replace(".", "");
            if (Regex.IsMatch(value, @"[\d\,]"))
                return Decimal.Parse(value);
            else
                return null;
        }

        public static string ConvertToString(decimal? value){
            if (value == null)
            {
                return string.Empty;
            }
            var result = value.Value.ToString("#,#.00");
            return result;
        }
    }
}
