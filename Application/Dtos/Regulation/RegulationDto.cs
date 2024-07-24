using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.Regulation
{
    public class RegulationDto
    {
        public required string Code { get; set; }
        public required string Content { get; set; }
        public required int Value { get; set; }
        public required bool Status { get; set; } // 1 = true, 0 = false
    }
}
