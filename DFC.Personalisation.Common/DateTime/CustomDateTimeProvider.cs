using System;
using System.Collections.Generic;
using System.Text;

namespace DFC.Personalisation.Common.DateTime
{
    public class CustomDateTimeProvider : DateTimeProvider, IDateTimeProvider
    {
        private readonly System.DateTime _customDateTime;
        public CustomDateTimeProvider(System.DateTime customDateTime)
        {
            _customDateTime = customDateTime;
        }

        public override System.DateTime UtcNow
        {
            get { return _customDateTime.ToUniversalTime(); }
        }
    }
}
