namespace DFC.Personalisation.Common.Extensions
{
    public static class StringExtensions
    {
        public static string UppercaseFirst(this string src)
        {
            if (string.IsNullOrWhiteSpace(src))
            {
                return string.Empty;
            }
            char[] c = src.ToCharArray();
            c[0] = char.ToUpper(c[0]);
            return new string(c);
        }
    }
}
