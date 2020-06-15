using System;
using System.Linq;

namespace DFC.Personalisation.Common.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input.First().ToString().ToUpper() + input.Substring(1)
            };

        public static string ToLower(this object obj)
        {
            return obj.ToString().ToLower();
        }

        public static string ToUpper(this object obj)
        {
            return obj.ToString().ToUpper();
        }
    }
}
