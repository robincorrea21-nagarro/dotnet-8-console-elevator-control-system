using dotnet_8_console_elevator_control_system.Core.Enums;
using dotnet_8_console_elevator_control_system.DTOs;

namespace dotnet_8_console_elevator_control_system.Entities
{
    public class Building
    {
        private List<Elevator> elevators = new List<Elevator>();
        private Random random = new Random();
        private bool requestInProgress = false;

        public Building(int numberOfElevators)
        {
            for (int i = 0; i < numberOfElevators; i++)
            {
                elevators.Add(new Elevator(i + 1, this));
            }
        }

        public async Task StartElevatorSystemAsync()
        {
            while (true)
            {
                if (!requestInProgress)
                {
                    GenerateNewRequest();
                }

                await Task.Delay(1000); // Simulate delay between checks asynchronously
            }
        }

        public void ElevatorStopped(Elevator elevator)
        {
            requestInProgress = false;
        }

        private void GenerateNewRequest()
        {
            requestInProgress = true;

            int floor = random.Next(0, 11);
            Direction direction;

            if (floor == 0)
            {
                direction = Direction.Up;
            }
            else if (floor == 10)
            {
                direction = Direction.Down;
            }
            else
            {
                direction = random.Next(0, 2) == 0 ? Direction.Up : Direction.Down;
            }

            RequestDto request = new RequestDto(floor, direction);
            AddRequest(request);
        }

        public void AddRequest(RequestDto request)
        {
            Console.WriteLine($"Someone pressed the \"{request.Direction.ToString().ToLower()}\" button on floor {request.Floor}.");

            Elevator selectedElevator = GetNearestAvailableElevator(request);
            if (selectedElevator != null)
            {
                selectedElevator.AddDestination(request.Floor, request.Direction);
            }
            else
            {
                Console.WriteLine("No available elevator at the moment.");
                requestInProgress = false;
            }
        }

        private Elevator GetNearestAvailableElevator(RequestDto request)
        {
            Elevator bestElevator = null;
            double bestScore = double.MinValue; // The higher the score, the better the elevator

            foreach (var elevator in elevators)
            {
                // Calculate idle time in seconds
                TimeSpan idleTime = elevator.State == ElevatorState.Idle ? DateTime.Now - elevator.IdleSince : TimeSpan.Zero;

                // Calculate distance to the requested floor
                int distance = Math.Abs(elevator.CurrentFloor - request.Floor);

                // Normalize idle time
                double idleTimeScore = idleTime.TotalSeconds / 10.0;

                // Normalize distance
                double maxDistance = 10.0;
                double distanceScore = maxDistance - distance;

                // Combine idle time and distance into a final score, giving each a weight
                double idleWeight = 0.7;
                double distanceWeight = 0.3;

                double finalScore = (idleTimeScore * idleWeight) + (distanceScore * distanceWeight);

                // Choose the elevator with the best score
                if (finalScore > bestScore)
                {
                    bestScore = finalScore;
                    bestElevator = elevator;
                }
            }

            return bestElevator;
        }
    }
}
