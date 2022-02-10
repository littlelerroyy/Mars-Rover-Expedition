using ConsoleApp1;

//Title of the app
Config.ApplicationHeading();

//Validate the boundries from the config
if (!Config.VerifyBoundries())
{
    Console.WriteLine("Boundries are invalid. Make sure Grid GridSizeXMin is less than GridSizeXMax and GridSizeYMin is less than GridSizeYMax in config class");
    Environment.Exit(0);
}

//Announce the grid boundries X-Y postions set in Config
Console.WriteLine($"Lower Boundry: {Config.GridSizeXMin}, {Config.GridSizeYMin}, Upper Boundry: {Config.GridSizeXMax}, {Config.GridSizeXMax}");

// Setup the initial Commands for the 2 rovers
var Rover1Commands = new RoverCommand
{
    BatchCommands = new char[] { 'L', 'M', 'L', 'M', 'L', 'M', 'L', 'M', 'M' }
};

var Rover2Commands = new RoverCommand
{
    BatchCommands = new char[] { 'M', 'M', 'R', 'M', 'M', 'R', 'M', 'R', 'R', 'M' }
};

// Lets do a quick unit test on the rover command validation script
try
{
    UnitTest.RoverCommand(new char[] { 'm', 'L', 'R' }, true);
    UnitTest.RoverCommand(new char[] { 'D', 'L', 'R', 'M' }, false);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("Rover command validation unit test PASS!");
    Console.ResetColor();

}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Rover command validation unit test Failed!");
    Console.WriteLine(ex.Message);
    Console.WriteLine("Terminating Application!");
    Environment.Exit(0);
}

for (int i = 5; i >= 0; i--)
{
    Console.WriteLine($"Commencing Rover Test in {i}s");
    Thread.Sleep(1000);
}

//Spawn the Rovers
var RoverList = new List<Rover>()
{
    new Rover("Rover 1", 1, 2, Rover.Directions.N, Rover1Commands),
    new Rover("Rover 2", 3, 3, Rover.Directions.E, Rover2Commands),
    new Rover("Rover 3", 5, 5, Rover.Directions.S),
};

//Assign the rover list to a public property;
Config.RoverList = RoverList;

//Make sure Rovers are not spawned on top of eachother
if(!Config.VerifySpawnedRovers())
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Rovers must not be spawned in the same location!");
    Console.WriteLine("Terminating Application!");
    Environment.Exit(0);
}

//Issue Batch Commands for initial spawned rovers if they have commands issued to them, and apply Unit Tests for 2 of the first rovers.
foreach (var Rover in Config.RoverList)
{
    if (Rover.RoverCommand != null)
    {
        Rover.AnnounceCurrentPosition();
        Rover.BatchExecute("animate");
    }

    if (Rover.RoverName == "Rover 1" || Rover.RoverName == "Rover 2")
    {
        try
        {
            if (Rover.RoverName == "Rover 1")
                UnitTest.Rover(Rover, new Rover("Rover 1", 1, 3, Rover.Directions.N));

            if (Rover.RoverName == "Rover 2")
                UnitTest.Rover(Rover, new Rover("Rover 2", 5, 1, Rover.Directions.E));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Unit Test for {Rover.RoverName} PASS");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"UNIT TEST FAIL! on {Rover.RoverName} Terminating Application!");
            Console.WriteLine(ex.Message);
            Environment.Exit(0);
        }
    }
}

//Time to control the rovers //
Console.WriteLine("To continue to control the Rovers please type in the rover name to enter the selected rover");
Console.WriteLine("Then Issue commands M - (Move Rover) L - (Rotate Left) R - (Rotate Right)");
Console.WriteLine("Or type help for more commands");

while (true)
{
    var ConsoleString = Console.ReadLine().ToLower();

    // this is to issue commands to the rovers from the list.
    if (Config.RoverList.Exists(x => x.RoverName.ToLower() == ConsoleString))
    {
        var Rover = Config.RoverList.Where(x => x.RoverName.ToLower() == ConsoleString).FirstOrDefault();
        Console.WriteLine($"Entering {Rover.RoverName}");
        Rover.Render();
        while (true)
        {
            ConsoleString = Console.ReadLine();
            if (ConsoleString == "exit")
            {
                Console.WriteLine($"Exiting {Rover.RoverName}");
                break;
            }
            else
            {
                // convert string into an array
                var CommandList = ConsoleString.ToCharArray();

                //remove white space
                CommandList = CommandList.Where(x => x != ' ').ToArray();

                Rover.RoverCommand = new RoverCommand()
                {
                    BatchCommands = CommandList
                };

                Rover.BatchExecute("animate");

                Console.WriteLine("Enter more Rover commands or type 'exit' to leave the rover");
            }

        }

    }
    else
    {
        switch (ConsoleString)
        {
            case "list rovers":
                foreach (var Rover in Config.RoverList)
                {
                    Rover.AnnounceCurrentPosition();
                    Rover.Render();
                }
                break;
            case "help":
                ConsoleCommands.AvailableCommands();
                break;
            case "exit":
                Environment.Exit(0);
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Rover name doesnt exist or you've entered an in-valid command. please type in \"help\" for more options");
                Console.ResetColor();
                break;
        };
    }
}