using NUnit.Framework;
using TourPlanner.Helper;

namespace TourPlanner.TestBackend
{
    class TestValidator
    {
        [SetUp]
        public void Setup()
        {
        }

        #region isText
        [Test]
        public void TestValidatorisText_isValid()
        {
            string validText = "aS_dF 123aSdF";
            bool res = Validator.isText(validText);
            Assert.IsTrue(res);          
        }

        [Test]
        public void TestValidatorisText_isInValid()
        {
            string invalidText = "a_SdF123aSdF!$";
            bool res = Validator.isText(invalidText);
            Assert.IsFalse(res);
        }

        #endregion
        #region isNumeric
        [Test]
        public void TestValidatorisNumeric_isValid()
        {
            string validNumber = "01259";
            bool res = Validator.isNumeric(validNumber);
            Assert.IsTrue(res);
        }

        [Test]
        public void TestValidatorisNumeric_isInvalid()
        {
            string invalidNumber = "0abc1259";
            bool res = Validator.isNumeric(invalidNumber);
            Assert.IsFalse(res);
        }
        #endregion
        #region isFloat
        [Test]
        public void TestValidatorisFloat_isValid()
        {
            string validFloat = "0154.4654";
            bool res = Validator.isFloat(validFloat);
            Assert.IsTrue(res);
        }

        [Test]
        public void TestValidatorisFloat_isValid_noDecimalPlaces()
        {
            string validFloat = "0456";
            bool res = Validator.isFloat(validFloat);
            Assert.IsTrue(res);
        }

        [Test]
        public void TestValidatorisFloat_isInvalid()
        {
            string invalidFloat = "0,456";
            bool res = Validator.isFloat(invalidFloat);
            Assert.IsFalse(res);
        }
        #endregion
        #region isLocation
        [Test]
        public void TestValidatorisLocation_isValid()
        {
            string validLocation = "Lepperdorf 82, 4612 Scharten, Austria";
            bool res = Validator.isLocation(validLocation);
            Assert.IsTrue(res);
        }

        [Test]
        public void TestValidatorisLocation_isInvalid()
        {
            string invalidLocation = "Invalid Location";
            bool res = Validator.isLocation(invalidLocation);
            Assert.IsFalse(res);
        }
        #endregion
    }
}
