using DFC.Personalisation.Common.DateTime;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DFC.Personalisation.Common.UnitTests.DateTime
{
    [TestFixture]
    public class DateTimeProviderTests
    {
        [TestFixture]
        public class UtcNow
        {
            [TestCase(100)]
            public void When_DefaultDateTimeProvider_Then_ReturnCurrentTime(int toleranceMs)
            {
                // Arrange

                System.DateTime systemNow = System.DateTime.UtcNow;
                IDateTimeProvider dtp = new DefaultDateTimeProvider();

                // Act
                System.DateTime dtpNow = dtp.UtcNow;

                // Assert
                dtpNow.Should().BeCloseTo(systemNow, toleranceMs);
            }

            [TestCase(100)]
            public void When_MockedDateTimeProvider_Then_ReturnMockTime(int toleranceMs)
            {
                // Arrange

                System.DateTime fakeSystemNow = new System.DateTime(2019, 12, 2, 09, 34, 12, 345);
                var dateTimeMock = new Mock<IDateTimeProvider>();
                dateTimeMock.SetupGet(tp => tp.UtcNow).Returns(fakeSystemNow);
                IDateTimeProvider dtp = dateTimeMock.Object;

                // Act
                System.DateTime dtpNow = dtp.UtcNow;

                // Assert
                dtpNow.Should().BeCloseTo(fakeSystemNow, toleranceMs);
            }
        }
    }
}
