using dotnet_8_console_elevator_control_system.Core.Enums;

namespace dotnet_8_console_elevator_control_system.Entities
{
    public class Elevator
    {
        public int Id { get; }
        public int CurrentFloor { get; private set; }
        public Direction CurrentDirection { get; private set; }
        public ElevatorState State { get; private set; }
        public DateTime IdleSince { get; private set; } // Track when the elevator became idle
        private Building building;

        private List<int> destinationFloors = new List<int>();

        public Elevator(int id, Building building)
        {
            Id = id;
            CurrentFloor = 0;
            CurrentDirection = Direction.Idle;
            State = ElevatorState.Idle;
            IdleSince = DateTime.Now; // Initialize with the current time
            this.building = building;
        }

        public void AddDestination(int floor, Direction requestedDirection)
        {
            SetConsoleColor();
            Console.WriteLine($"Assigned floor {floor} to Elevator {Id}.");
            ResetConsoleColor();

            if (CurrentFloor == floor && CurrentDirection != requestedDirection && requestedDirection != Direction.Idle)
            {
                SetConsoleColor();
                Console.WriteLine($"Car {Id} is already at floor {floor}, waiting for the next request.");
                ResetConsoleColor();

                State = ElevatorState.Stopped;
                Task.Delay(1000).Wait(); // Simulate passenger interaction
                State = ElevatorState.Idle;
                CurrentDirection = Direction.Idle;
                IdleSince = DateTime.Now; // Reset idle time
                building.ElevatorStopped(this);
                return;
            }

            if (!destinationFloors.Contains(floor))
            {
                destinationFloors.Add(floor);

                if (State == ElevatorState.Idle)
                {
                    IdleSince = DateTime.MinValue; // Clear idle time when it starts moving
                    Task.Run(() => MoveAsync()); // Start the movement asynchronously
                }
            }
        }

        public async Task MoveAsync()
        {
            while (destinationFloors.Count > 0)
            {
                State = ElevatorState.Moving;
                int nextFloor = destinationFloors.First();
                destinationFloors.RemoveAt(0);

                while (CurrentFloor != nextFloor)
                {
                    if (CurrentFloor < nextFloor)
                    {
                        CurrentFloor++;
                        CurrentDirection = Direction.Up;
                    }
                    else if (CurrentFloor > nextFloor)
                    {
                        CurrentFloor--;
                        CurrentDirection = Direction.Down;
                    }

                    SetConsoleColor();
                    Console.WriteLine($"Car {Id} is at floor {CurrentFloor}");
                    ResetConsoleColor();
                    await Task.Delay(2000); // Simulate moving between floors asynchronously
                }

                SetConsoleColor();
                Console.WriteLine($"Car {Id} has arrived at floor {CurrentFloor} and is stopping.");
                ResetConsoleColor();
                State = ElevatorState.Stopped;
                await Task.Delay(2000); // Simulate passenger interaction

                State = ElevatorState.Idle;
                CurrentDirection = Direction.Idle;
                IdleSince = DateTime.Now; // Reset idle time
            }

            SetConsoleColor();
            Console.WriteLine($"Car {Id} is idle at floor {CurrentFloor}, waiting for the next request.");
            ResetConsoleColor();

            building.ElevatorStopped(this);
        }

        private void SetConsoleColor()
        {
            switch (Id)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                default:
                    ResetConsoleColor();
                    break;
            }
        }

        private void ResetConsoleColor()
        {
            Console.ResetColor();
        }
    }
}
