using NUnit.Framework;

namespace ConsoleApp1
{
    [TestFixture]
    internal class UnitTest
    {
        [Test]
        public static void Rover(Rover Rover, Rover ExpectedRoverOutput) {
            Assert.AreEqual(Rover.xAxis, ExpectedRoverOutput.xAxis);
            Assert.AreEqual(Rover.yAxis, ExpectedRoverOutput.yAxis);
            Assert.AreEqual(Rover.Direction, ExpectedRoverOutput.Direction);
            Assert.AreEqual(Rover.RoverName, ExpectedRoverOutput.RoverName);
        }

        public static void RoverCommand(char[] CommandArray, bool boolean)
        {
            var RoverCommand = new RoverCommand { BatchCommands = CommandArray };
            Assert.AreEqual(RoverCommand.CheckCommandsAreValid(), boolean);
        }
    }
}
