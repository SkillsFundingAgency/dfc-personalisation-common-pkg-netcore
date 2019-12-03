using System;
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
            [TestCase(1)]
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

            [TestCase(1)]
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

        [TestFixture]
        public class Current
        {
            [TestCase(1)]
            public void When_DefaultDateTimeProvider_Then_GetCurrentTime(int toleranceMs)
            {
                // Arrange
                System.DateTime systemNow = System.DateTime.UtcNow;
                DateTimeProvider.ResetToDefault();

                // Act
                IDateTimeProvider dtpNow = DateTimeProvider.Current;

                // Assert
                dtpNow.UtcNow.Should().BeCloseTo(systemNow, toleranceMs);
            }

            [Test]
            public void When_CustomDateTimeProvider_Then_SetCustomTime()
            {
                // Arrange
                System.DateTime customDateTime = new System.DateTime(1970, 01, 01, 00, 00, 00, DateTimeKind.Local);
                DateTimeProvider.Current = new CustomDateTimeProvider(customDateTime);

                // Act
                var dtpNow = DateTimeProvider.Current.UtcNow;

                // Assert
                dtpNow.Should().Be(customDateTime);
            }
        }
    }
}
