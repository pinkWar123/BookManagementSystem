using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.Regulation
{
    public class UpdateRegulationDto
    {
        public  string? Code { get; set; }
        public  string? Content { get; set; }
        public  int? Value { get; set; }
        public  bool? Status { get; set; } // 1 = true, 0 = false
    }
}
