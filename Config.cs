
namespace ConsoleApp1
{
    internal class Config
    {
        public static int GridSizeXMin
        {
            get { return 0; }
        }
        public static int GridSizeYMin
        {
            get { return 0; }
        }
        public static int GridSizeXMax
        {
            get { return 5; }
        }
        public static int GridSizeYMax
        {
            get { return 5; }
        }

        public static int GridSizeYCount
        {
            get { return Math.Abs(GridSizeYMin - GridSizeYMax); }
        }

        public static int GridSizeXCount
        {
            get { return Math.Abs(GridSizeXMin - GridSizeXMax); }
        }
        public static List<Rover> RoverList { get; set; }

        public static void ApplicationHeading()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("/ / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /");
            Console.WriteLine("/ / / / / / / / / / / / / /  Mars Rover Expedition  / / / / / / / / / / / / / /");
            Console.WriteLine("/ / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / / /");
            Console.WriteLine("/ / / The only way to control Mars Rovers without being on NASAs Payroll! / / /");
            Console.WriteLine(" ");
            Console.ResetColor();
        }
        // this is to make sure the Max is greater than the min boundries
        public static bool VerifyBoundries()
        {
            if (GridSizeXMin < GridSizeXMax && GridSizeXMin < GridSizeXMax)
            {
                return true;
            }
            return false;
        }
        public static bool VerifySpawnedRovers()
        {
            var RoveratDifferentPositions = Config.RoverList.DistinctBy(x => new { x.yAxis, x.xAxis }).Count();
            if (RoveratDifferentPositions != Config.RoverList.Count)
            {
                return false;
            }

            return true;
        }

    }
}
