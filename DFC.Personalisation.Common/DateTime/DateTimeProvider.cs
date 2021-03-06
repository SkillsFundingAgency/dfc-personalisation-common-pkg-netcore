﻿using System;

namespace DFC.Personalisation.Common.DateTime
{
    public interface IDateTimeProvider
    {
        System.DateTime UtcNow { get; }
    }

    public abstract class DateTimeProvider : IDateTimeProvider
    {
        private static IDateTimeProvider _current;

        static DateTimeProvider()
        {
            ResetToDefault();
        }

        public static IDateTimeProvider Current
        {
            get => _current;
            set => _current = value ?? throw new ArgumentNullException("value");
        }

        public abstract System.DateTime UtcNow { get; }

        public static void ResetToDefault()
        {
            _current = new DefaultDateTimeProvider();
        }
    }
}