using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Application.Queries
{
    public class RegulationQuery : QueryObject
    {
        // public  int? RegulationId { get; set; }
        // public  string? Code { get; set; }
        // public  string? Content { get; set; }
        // public  int? Value { get; set; }
        public  bool? Status { get; set; } // 1 = true, 0 = false

    }
}