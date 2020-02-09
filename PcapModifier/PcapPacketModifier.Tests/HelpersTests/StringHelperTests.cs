using System;
using FluentAssertions;
using PcapPacketModifier.Logic.Helpers.Interfaces;
using PcapPacketModifier.Logic.Helpers;
using NUnit.Framework;

namespace UnitTests.HelpersTests
{
    public class StringHelperTests
    {
        private IStringHelper _target;

        [SetUp]
        public void Setup()
        {
            _target = new StringHelper();
        }

        [Test]
        public void StringWithCommasToArrayOfStringValues_StringIsNull_ThrowsError()
        {
            // Act
            Action action = () => _target.StringWithSignSeparatorsToArrayOfValues(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void StringWithCommasToArrayOfStringValues_StringIsEmpty_ThrowsError()
        {
            // Act
            Action action = () => _target.StringWithSignSeparatorsToArrayOfValues("");

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        //[DataTestMethod]
        //[DataRow("first, second")]
        //[DataRow("first second")]
        //[DataRow("first second,")]
        //[DataRow(",first second")]
        //[DataRow(",first second,")]
        //[DataRow("first,, second")]
        //[DataRow(",,first,, second,,")]
        //[DataRow("first  second")]
        //[DataRow("first second ")]
        //[DataRow(" first second")]
        //[DataRow("first second")]
        //[DataRow(".,.,first    second.,. ")]
        //public void StringWithCommasToArrayOfStringValues_StringIsWithCommas_ReturnsArrayWithStringValues(string input)
                                                                                                                                                                  
        //{
        //    // Act
        //    string[] result = _target.StringWithSignSeparatorsToArrayOfValues(input);

        //    // Assert
        //    result[0].Should().Be("first");
        //    result[1].Should().Be("second");
        //}
    }
}
