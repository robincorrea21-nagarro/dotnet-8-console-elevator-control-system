using dotnet_8_console_elevator_control_system.Core.Enums;

namespace dotnet_8_console_elevator_control_system.DTOs
{
    public class RequestDto
    {
        public int Floor { get; set; }
        public Direction Direction { get; set; }

        public RequestDto(int floor, Direction direction)
        {
            Floor = floor;
            Direction = direction;
        }
    }
}
