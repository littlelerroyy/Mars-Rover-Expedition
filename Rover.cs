using System.Text;

namespace ConsoleApp1
{
    class Rover
    {
        public Rover(string _roverName, int _xAxis, int _yAxis, Directions _direction, RoverCommand? _initRoverCmd = null)
        {
            // Lets make sure we cant spawn the rovers outside the grid area.
            if (_xAxis > Config.GridSizeXMax || _xAxis < Config.GridSizeXMin || _yAxis > Config.GridSizeYMax || _yAxis < Config.GridSizeYMin)
            {
                throw new ArgumentException("You cannot spawn a rover outside the boundries");
            }
            RoverName = _roverName;
            xAxis = _xAxis;
            yAxis = _yAxis;
            Direction = _direction;
            RoverCommand = _initRoverCmd;
        }
        public string RoverName { get; set; }
        public int xAxis { get; set; }
        public int yAxis { get; set; }
        public enum Directions : sbyte
        {
            N = 0,
            E = 1,
            S = 2,
            W = 3
        }
        public Directions Direction { get; set; }

        public RoverCommand? RoverCommand { get; set; }

        public bool Move()
        {
            // the conditionals inside the switch statement are to tell the execution process its about to leave the boundry.
            switch (this.Direction)
            {
                case Directions.N:

                    if (!CheckForCollision(this.xAxis, this.yAxis + 1))
                    {
                        this.yAxis += 1;
                        return true;
                    }
                    break;
                case Directions.S:

                    if (!CheckForCollision(this.xAxis, this.yAxis - 1))
                    {
                        this.yAxis -= 1;
                        return true;
                    }
                    break;

                case Directions.E:

                    if (!CheckForCollision(this.xAxis + 1, this.yAxis))
                    {
                        this.xAxis += 1;
                        return true;
                    }
                    break;

                case Directions.W:

                    if (!CheckForCollision(this.xAxis - 1, this.yAxis))
                    {
                        this.xAxis -= 1;
                        return true;
                    }
                    break;
            }
            return false;
        }
        public void RotateRight()
        {
            if (this.Direction == Directions.W)
            {
                this.Direction = Directions.N;
            }
            else
            {
                this.Direction = this.Direction + 1;
            }
        }
        public void RotateLeft()
        {
            if (this.Direction == Directions.N)
            {
                this.Direction = Directions.W;
            }
            else
            {
                this.Direction = this.Direction - 1;
            }
        }
        public bool BatchExecute(string? args = null)
        {
            if (RoverCommand == null || RoverCommand.BatchCommands == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"No Batch Commands Found for {this.RoverName} No movements will be executed");
                Console.ResetColor();
                return false;
            }

            if (!RoverCommand.CheckCommandsAreValid())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Commands Detected! Please only a L R or M. No movements will be executed");
                Console.ResetColor();
                return false;
            }

            Console.WriteLine($"Commencing Batch Commands for {this.RoverName}..");

            foreach (var Command in this.RoverCommand.BatchCommands)
            {
                if (Char.ToUpper(Command) == 'M')
                {
                    if (!this.Move())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You're trying to navigate the rover into another rover or run outside the boundry, please dont do that!");
                        Console.WriteLine("Discarding any remainder commands");
                        Console.ResetColor();
                        break;
                    }
                }
                else if (Char.ToUpper(Command) == 'L')
                {
                    this.RotateLeft();
                }
                else if (Char.ToUpper(Command) == 'R')
                {
                    this.RotateRight();
                }

                if (args == "animate")
                {
                    this.Render();
                    Console.WriteLine(" ");// Blank Line
                    Thread.Sleep(500);
                }
            }
            Console.WriteLine($"{this.RoverName}: Resting Place: {this.xAxis}, {this.yAxis}, {this.Direction}");
            return true;
        }

        public Boolean CheckForCollision(int DesiredxAxis, int DesiredyAxis)
        {
            //these two are to check if the rover will run out of bounds
            if (DesiredxAxis < Config.GridSizeXMin || DesiredxAxis > Config.GridSizeXMax)
                return true;

            if (DesiredyAxis < Config.GridSizeYMin || DesiredyAxis > Config.GridSizeYMax)
                return true;
            // this is to see if it will collide with another rover
            if (Config.RoverList.Any(x => x.xAxis == DesiredxAxis && x.yAxis == DesiredyAxis))
            {
                return true;
            }

            return false;
        }

        public void AnnounceCurrentPosition()
        {
            Console.WriteLine($"{RoverName}: Current Location: {xAxis}, {yAxis}, {Direction}");
        }

        public void Render()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            for (var yAxis = Config.GridSizeYMax; yAxis >= Config.GridSizeYMin; yAxis--)
            {
                // this is for X axis
                var RowString = new StringBuilder("", Config.GridSizeXCount);
                for (var xAxis = Config.GridSizeXMin; xAxis <= Config.GridSizeXMax; xAxis++)
                {
                    //current rover position
                    if (this.xAxis == xAxis && this.yAxis == yAxis)
                    {
                        RowString.Append(this.RoverIcon() + "  ");
                    }
                    //list other rovers
                    else if (Config.RoverList.Any(x => x.xAxis == xAxis && x.yAxis == yAxis))
                    {
                        RowString.Append("\u263B  ");
                    }
                    //The mars wilderness
                    else
                    {
                        RowString.Append("\u2592  ");
                    }
                }
                Console.WriteLine(RowString);
            }
            Console.ResetColor();
        }

        public string RoverIcon()
        {
            string DirectionCharacter = "";
            switch (this.Direction)
            {
                case Directions.N:
                    DirectionCharacter = "\u25b2";
                    break;
                case Directions.S:
                    DirectionCharacter = "\u25bc";
                    break;
                case Directions.E:
                    DirectionCharacter = "\u25ba";
                    break;
                case Directions.W:
                    DirectionCharacter = "\u25c4";
                    break;
            }
            return DirectionCharacter;
        }
    }
}
