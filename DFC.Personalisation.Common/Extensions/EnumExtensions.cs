using System;

namespace DFC.Personalisation.Common.Extensions
{
    public static class EnumExtensions
    {
        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            return Enum.TryParse<T>(value, true, out T result) ? result : defaultValue;
        }
        public static string ToLower (this object obj)
        {
            return obj.ToString().ToLower();
        }
        public static string ToUpper (this object obj)
        {
            return obj.ToString().ToUpper();
        }
    }
}
