namespace dotnet_8_console_elevator_control_system.Entities
{
    public class Building
    {
        public List<Elevator> Elevators { get; private set; }
        public bool RequestInProgress { get; set; }

        public Building(int numberOfElevators)
        {
            Elevators = new List<Elevator>();
            for (int i = 0; i < numberOfElevators; i++)
            {
                Elevators.Add(new Elevator(i + 1));
            }
        }
    }
}
