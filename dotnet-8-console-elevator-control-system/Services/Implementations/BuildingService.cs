using dotnet_8_console_elevator_control_system.Core.Enums;
using dotnet_8_console_elevator_control_system.DTOs;
using dotnet_8_console_elevator_control_system.Entities;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_8_console_elevator_control_system.Services
{
    public class BuildingService
    {
        private readonly ElevatorService _elevatorService;

        public BuildingService(ElevatorService elevatorService)
        {
            _elevatorService = elevatorService;
        }

        public async Task StartElevatorSystemAsync(Building building)
        {
            while (true)
            {
                if (!building.RequestInProgress)
                {
                    GenerateNewRequest(building);
                }

                await Task.Delay(1000); // Simulate system checking delay
            }
        }

        private void GenerateNewRequest(Building building)
        {
            building.RequestInProgress = true;

            var random = new Random();
            int floor = random.Next(0, 11);
            Direction direction = floor == 0 ? Direction.Up : (floor == 10 ? Direction.Down : random.Next(0, 2) == 0 ? Direction.Up : Direction.Down);
            RequestDto request = new RequestDto(floor, direction);

            AddRequest(building, request);
        }

        public void AddRequest(Building building, RequestDto request)
        {
            Console.WriteLine($"Someone pressed the \"{request.Direction.ToString().ToLower()}\" button on floor {request.Floor}.");

            Elevator selectedElevator = GetNearestAvailableElevator(building.Elevators, request);
            if (selectedElevator != null)
            {
                _elevatorService.AddDestination(selectedElevator, request.Floor, request.Direction);
                _elevatorService.MoveAsync(selectedElevator, building); // Ensure building is passed to notify when idle
            }
            else
            {
                Console.WriteLine("No available elevator at the moment.");
                building.RequestInProgress = false; // Reset the request progress if no elevator is available
            }
        }

        private Elevator GetNearestAvailableElevator(List<Elevator> elevators, RequestDto request)
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
