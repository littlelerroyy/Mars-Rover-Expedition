
namespace ConsoleApp1
{
    internal class RoverCommand
    {
        public char[] BatchCommands { get; set; }

        public bool CheckCommandsAreValid()
        {
            return Array.TrueForAll(this.BatchCommands, x => Char.ToUpper(x) == 'M' || Char.ToUpper(x) == 'L' || Char.ToUpper(x) == 'R');
        }

    }
}