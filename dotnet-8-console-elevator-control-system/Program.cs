using dotnet_8_console_elevator_control_system.Entities;
using dotnet_8_console_elevator_control_system.Services;

// Initialize services
var elevatorService = new ElevatorService();
var buildingService = new BuildingService(elevatorService);

// Create a building with 4 elevators
var building = new Building(4);

// Start the elevator request system
await buildingService.StartElevatorSystemAsync(building);
