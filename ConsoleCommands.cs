
namespace ConsoleApp1
{
    internal class ConsoleCommands
    {
        public static void AvailableCommands()
        {
            Console.WriteLine("Enter a rover name to enter a rover then issue commands");
            Console.WriteLine("  e.g. Rover 1");
            Console.WriteLine("  e.g. M - (Move Rover) L - (Rotate Left) R - (Rotate Right) ");
            Console.WriteLine("list rovers - List the rovers that have been spawned");
            Console.WriteLine("exit - Exit the rover or the application");
        }
    }
}
