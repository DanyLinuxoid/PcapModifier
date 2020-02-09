using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PcapPacketModifier.Logic.Helpers.Interfaces;
using PcapPacketModifier.Logic.Helpers;

namespace UnitTests.HelpersTests
{
    [TestClass]
    public class StringHelperTests
    {
        private readonly IStringHelper _target;

        public StringHelperTests()
        {
            _target = new StringHelper();
        }

        [TestMethod]
        public void StringWithCommasToArrayOfStringValues_StringIsNull_ThrowsError()
        {
            // Act
            Action action = () => _target.StringWithSignSeparatorsToArrayOfValues(null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [TestMethod]
        public void StringWithCommasToArrayOfStringValues_StringIsEmpty_ThrowsError()
        {
            // Act
            Action action = () => _target.StringWithSignSeparatorsToArrayOfValues("");

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("first, second")]
        [DataRow("first second")]
        [DataRow("first second,")]
        [DataRow(",first second")]
        [DataRow(",first second,")]
        [DataRow("first,, second")]
        [DataRow(",,first,, second,,")]
        [DataRow("first  second")]
        [DataRow("first second ")]
        [DataRow(" first second")]
        [DataRow("first second")]
        [DataRow(".,.,first    second.,. ")]
        public void StringWithCommasToArrayOfStringValues_StringIsWithCommas_ReturnsArrayWithStringValues(string input)
                                                                                                                                                                  
        {
            // Act
            string[] result = _target.StringWithSignSeparatorsToArrayOfValues(input);

            // Assert
            result[0].Should().Be("first");
            result[1].Should().Be("second");
        }
    }
}
