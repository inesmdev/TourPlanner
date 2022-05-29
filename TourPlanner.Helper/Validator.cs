using System.Text.RegularExpressions;

namespace TourPlanner.Helper
{
    public static class Validator
    {
        /*
        *  Numeric
        *  Allowed characters 0-9
        */
        public static bool isNumeric(string input)
        {
            Regex regex = new Regex(@"[0-9]+",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return regex.IsMatch(input);
        }

        /*
        *  Float
        *  floating point number with . seperator
        */
        public static bool isFloat(string input)
        {
            Regex regex = new Regex(@"[+-]?([0-9]+([.][0-9]*)?|[.][0-9]+)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return regex.IsMatch(input);
        }

        /*
         *  Text
         *  Allowed characters A-Z, a-z, 0-9, _, *space*
         */
        public static bool isText(string input)
        {
            Regex regex = new Regex(@"[A-Za-z0-9_ ]+",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return regex.IsMatch(input);
        }

        /*
        *  Location
        *  Street 123, 1234 City, Country
        */
        public static bool isLocation(string input)
        {
            string locationPattern = @"[A-Za-z]\w+ [0-9]{1,3}(\/[0-9]{1,3})*, ([0-9]{4}) [A-Za-z]\w+, [A-Za-z]\w+";

            Regex regex = new Regex(locationPattern,
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

            return regex.IsMatch(input);
        }
    }
}
