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

        [Test]
        public void When_ToLower_Used_Return_Lowercase_Enum_As_String()
        {
            var result = TestEnums.Test1.ToLower();
            result.Should().Be("test1");
        }
        [Test]
        public void When_ToUpper_Used_Return_Lowercase_Enum_As_String()
        {
            var result = TestEnums.Test1.ToUpper();
            result.Should().Be("TEST1");
        }
    }
}
