using DFC.Personalisation.Common.Extensions;
using NUnit.Framework;

namespace DFC.Personalisation.Common.UnitTests.Extenisons
{
    internal enum TestEnums
    {
        Undefined = 0,
        Test1 = 1
    }
    public class ToEnumTests
    {
        [Test]
        public void If_Invalid_Value_Return_Default()
        {
            string incorrect = "sdfdfsfs";
            var result = incorrect.ToEnum(defaultValue: TestEnums.Undefined);
            Assert.AreEqual(TestEnums.Undefined, result);
        }

        [Test]
        public void If_Blank_Value_Return_Default()
        {
            var blank = string.Empty;
            var result = blank.ToEnum(defaultValue: TestEnums.Undefined);
            Assert.AreEqual(TestEnums.Undefined, result);
        }

        [Test]
        public void If_Valid_Value_Return_Correct_Value()
        {
            var validTest1 = "Test1";
            var result = validTest1.ToEnum(defaultValue: TestEnums.Undefined);
            Assert.AreEqual(TestEnums.Test1, result);
        }
        [Test]
        public void If_Valid_CaseInsensitive_Value_Return_Correct_Value()
        {
            var validTest1 = "TeSt1";
            var result = validTest1.ToEnum(defaultValue: TestEnums.Undefined);
            Assert.AreEqual(TestEnums.Test1, result);
        }
    }
}
