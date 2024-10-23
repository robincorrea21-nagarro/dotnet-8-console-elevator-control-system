using dotnet_8_console_elevator_control_system.Core.Enums;
using dotnet_8_console_elevator_control_system.Entities;

namespace dotnet_8_console_elevator_control_system.Services
{
    public class ElevatorService
    {
        public async Task MoveAsync(Elevator elevator, Building building)
        {
            while (elevator.DestinationFloors.Count > 0)
            {
                elevator.State = ElevatorState.Moving;
                int nextFloor = elevator.DestinationFloors.First();
                elevator.DestinationFloors.RemoveAt(0);

                // Simulate elevator movement between floors
                while (elevator.CurrentFloor != nextFloor)
                {
                    if (elevator.CurrentFloor < nextFloor)
                    {
                        elevator.CurrentFloor++;
                        elevator.CurrentDirection = Direction.Up;
                    }
                    else if (elevator.CurrentFloor > nextFloor)
                    {
                        elevator.CurrentFloor--;
                        elevator.CurrentDirection = Direction.Down;
                    }

                    SetConsoleColor(elevator.Id);
                    Console.WriteLine($"Car {elevator.Id} is at floor {elevator.CurrentFloor}");
                    ResetConsoleColor();
                    await Task.Delay(2000); // Simulate delay between floors
                }

                SetConsoleColor(elevator.Id);
                Console.WriteLine($"Car {elevator.Id} has arrived at floor {elevator.CurrentFloor} and is stopping.");
                ResetConsoleColor();
                elevator.State = ElevatorState.Stopped;
                await Task.Delay(2000); // Simulate passenger interaction

                // Elevator goes idle after reaching the destination
                elevator.State = ElevatorState.Idle;
                elevator.CurrentDirection = Direction.Idle;
                elevator.IdleSince = DateTime.Now; // Reset idle time
            }

            SetConsoleColor(elevator.Id);
            Console.WriteLine($"Car {elevator.Id} is idle at floor {elevator.CurrentFloor}, waiting for the next request.");
            ResetConsoleColor();

            // Notify the building that this elevator is now idle and ready for new requests
            building.RequestInProgress = false; // Reset requestInProgress here
        }

        public void AddDestination(Elevator elevator, int floor, Direction requestedDirection)
        {
            if (!elevator.DestinationFloors.Contains(floor))
            {
                elevator.DestinationFloors.Add(floor);
                SetConsoleColor(elevator.Id);
                Console.WriteLine($"Assigned floor {floor} to Elevator {elevator.Id}.");
                ResetConsoleColor();
            }
        }

        private void SetConsoleColor(int elevatorId)
        {
            switch (elevatorId)
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
                    Console.ResetColor();
                    break;
            }
        }

        private void ResetConsoleColor()
        {
            Console.ResetColor();
        }
    }
}
