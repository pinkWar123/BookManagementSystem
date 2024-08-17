using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.Invoice
{
    public class IncomeViewDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int Income { get; set; }
    }
}