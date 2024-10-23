using dotnet_8_console_elevator_control_system.Entities;

// Top-Level Program Logic
var building = new Building(4); // Create a building with 4 elevators

// Start the elevator request logic, which triggers only after an elevator stops
await building.StartElevatorSystemAsync();