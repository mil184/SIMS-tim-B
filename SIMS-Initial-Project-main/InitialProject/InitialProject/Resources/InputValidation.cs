using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InitialProject.Resources
{
    static class InputValidation
    {
        public static bool Regex(string input, string pattern)
        {
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(input))
                return true;

            return false;
        }

        public static bool CheckDate(string input)
        {
            try
            {
                DateTime.ParseExact(input, "dd.MM.yyyy.", CultureInfo.CurrentCulture);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckNumber(string input)
        {
            if (input.All(char.IsDigit))
            {
                return true;
            }
            return false;
        }
    }
}
