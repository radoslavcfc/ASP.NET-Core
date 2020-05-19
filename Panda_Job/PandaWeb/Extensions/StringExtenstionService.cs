using System.Linq;

namespace Panda.App.Extensions
{
    public static class StringExtenstionService
    {
        public static string Capitalize(this string input)
        {
            var firstLetterCapital = input.ElementAt(0).ToString().ToUpper();
            var userNameWithCapital = firstLetterCapital + input.Substring(1);
            return userNameWithCapital;
        }
    }
}
