﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using DFC.Personalisation.Common.Extensions;
using NUnit.Framework;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace DFC.Personalisation.Common.UnitTests.Extensions
{
    internal enum TestEnums
    {
        Undefined = 0,
        [Display(Name = "Test One")]
        Test1 = 1
    }
    public class EnumExtensionTests
    {
        [Test]
        public void If_Invalid_Value_Return_Default()
        {
            string incorrect = "sdfdfsfs";
            var result = incorrect.ToEnum(defaultValue: TestEnums.Undefined);
            result.Should().Be(TestEnums.Undefined);
        }

        [Test]
        public void If_Blank_Value_Return_Default()
        {
            var blank = string.Empty;
            var result = blank.ToEnum(defaultValue: TestEnums.Undefined);
            result.Should().Be(TestEnums.Undefined);
        }

        [Test]
        public void If_Valid_Value_Return_Correct_Value()
        {
            var validTest1 = "Test1";
            var result = validTest1.ToEnum(defaultValue: TestEnums.Undefined);
            result.Should().Be(TestEnums.Test1);
        }
        [Test]
        public void If_Valid_CaseInsensitive_Value_Return_Correct_Value()
        {
            var validTest1 = "TeSt1";
            var result = validTest1.ToEnum(defaultValue: TestEnums.Undefined);
            result.Should().Be(TestEnums.Test1);
        }

        [Test]
        public void If_GetDisplayName_Return_DisplayName()
        {
            var enumDisplay = TestEnums.Test1.GetDisplayName();
            enumDisplay.Should().Be("Test One");
        }
    }
}
