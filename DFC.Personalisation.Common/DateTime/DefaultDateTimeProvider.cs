namespace DFC.Personalisation.Common.DateTime
{
    public class DefaultDateTimeProvider : DateTimeProvider, IDateTimeProvider
    {
        public override System.DateTime UtcNow => System.DateTime.UtcNow;
    }
}
