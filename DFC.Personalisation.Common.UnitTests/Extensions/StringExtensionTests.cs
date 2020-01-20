using DFC.Personalisation.Common.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace DFC.Personalisation.Common.UnitTests.Extensions
{
    public class StringExtensionTests
    {
        [Test]
        public void When_FirstCharToUpperCalled_Then_TheStringReturnedShouldHaveAnUpperCaseFirstChar()
        {
            const string testString = "test";

            testString.FirstCharToUpper().Should().Be("Test");

        }
    }
}
