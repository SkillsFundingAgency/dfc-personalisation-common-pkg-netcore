using NUnit.Framework;
using DFC.Personalisation.Common.Extensions;
using FluentAssertions;

namespace DFC.Personalisation.Common.UnitTests.Extensions
{
    public class StringExtensionTests
    {
        [Test]
        public void When_Null_Then_ReturnEmptyString()
        {
            // Arrange.
            string sourceString = null;

            // Act.
            var result = sourceString.UppercaseFirst();

            // Assert.
            result.Should().BeEmpty();

        }

        [Test]
        public void When_Empty_Then_ReturnEmptyString()
        {
            // Arrange.
            string sourceString = string.Empty;

            // Act.
            var result = sourceString.UppercaseFirst();

            // Assert.
            result.Should().BeEmpty();
        }

        [Test]
        public void When_NonEmpty_Then_ReturnFirstCharacterCapitalised()
        {
            // Arrange.
            string sourceString = "wibble";

            // Act.
            var result = sourceString.UppercaseFirst();

            // Assert.
            result.Should().Be("Wibble");
        }
    }
}
