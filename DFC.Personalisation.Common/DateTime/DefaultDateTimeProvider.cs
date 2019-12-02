namespace DFC.Personalisation.Common.DateTime
{
    public class DefaultDateTimeProvider : DateTimeProvider, IDateTimeProvider
    {
        public override System.DateTime UtcNow
        {
            get { return System.DateTime.UtcNow; }
        }
    }
}
