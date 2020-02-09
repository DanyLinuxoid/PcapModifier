using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FluentAssertions;
using PcapDotNet.Packets.IpV4;
using PcapPacketModifier.Logic.Helpers.Interfaces;
using PcapPacketModifier.Logic.Modules.StructHandlers.Interfaces;
using PcapPacketModifier.Logic.Modules.StructHandlers;

namespace UnitTests.ModuleTests.StructHandlersTests
{
    [TestClass]
    public class ModuleStructArgumentsHandlerTests
    {
        private readonly Mock<IStringHelper> _stringModifierMock;
        private readonly IModuleStructArgumentsHandler _target;

        public ModuleStructArgumentsHandlerTests()
        {
            _stringModifierMock = new Mock<IStringHelper>();
            _target = new ModuleStructArgumentsHandler(_stringModifierMock.Object);
        }

        [TestMethod]
        public void ConfigureParametersForIpV4Fragmentation_ParametersAreOk_ReturnsObject()
        {
            // Arrange            
            var parameters = new string[2] { "MoreFragments", "80" };

            _stringModifierMock.Setup(x => x.StringWithSignSeparatorsToArrayOfValues("MoreFragments, 80"))
                .Returns(parameters);

            // Act
            var result = _target.InputToParametersForIpV4Fragmentation("MoreFragments, 80");

            // Assert
            result.Should().BeOfType<object[]>();
            result[0].Should().Be(IpV4FragmentationOptions.MoreFragments);
            result[1].Should().Be(80);
        }

        [TestMethod]
        public void ConfigureParametersForIpV4Fragmentation_ParametersAreSwapped_ReturnsNull()
        {
            var parameters = new string[2] { "80", "MoreFragments" };

            _stringModifierMock.Setup(x => x.StringWithSignSeparatorsToArrayOfValues("80, MoreFragments"))
                .Returns(parameters);

            // Act
            var result = _target.InputToParametersForIpV4Fragmentation("80, MoreFragments");

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void ConfigureParametersForIpV4Fragmentation_NotEnoughParameters_ReturnsNull()
        {
            // Arrange
            var parameters = new string[1] { "one" };

            _stringModifierMock.Setup(x => x.StringWithSignSeparatorsToArrayOfValues("one"))
                .Returns(parameters);

            // Act
            var result = _target.InputToParametersForIpV4Fragmentation("one");

            // Assert
            result.Should().BeNull();
        }
    }
}
