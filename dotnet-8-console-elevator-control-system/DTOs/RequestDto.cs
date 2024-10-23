using dotnet_8_console_elevator_control_system.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
