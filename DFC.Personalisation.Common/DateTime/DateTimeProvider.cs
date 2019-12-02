using System;

namespace DFC.Personalisation.Common.DateTime
{
    public interface IDateTimeProvider
    {
        System.DateTime UtcNow { get; }
    }

    public abstract class DateTimeProvider : IDateTimeProvider
    {
        private static DateTimeProvider _current;

        static DateTimeProvider()
        {
            ResetToDefault();
        }

        public static DateTimeProvider Current
        {
            get { return DateTimeProvider._current; }
            set
            {
                if (null == value)
                {
                    throw new ArgumentException("value");
                }

                DateTimeProvider._current = value;
            }
        }

        public abstract System.DateTime UtcNow { get; }

        public static void ResetToDefault()
        {
            DateTimeProvider._current = new DefaultDateTimeProvider();
        }
    }
}