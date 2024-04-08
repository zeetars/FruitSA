using System.Text.RegularExpressions;

namespace FruitSA.Web.Providers
{
    public class UniqueCodeValidator
    {

        public static bool Validate(string code)
        {
            bool isValid = CheckFormat(code);
            return isValid;
        }

        public static bool CheckFormat(string code)
        {
            // Regular expression pattern to match 3 alphabetic letters followed by 3 numeric characters
            string pattern = @"^[A-Za-z]{3}\d{3}$";

            // Check if input matches the pattern
            return Regex.IsMatch(code, pattern);
        }
    }


}
