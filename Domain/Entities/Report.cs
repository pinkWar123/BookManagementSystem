using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    public class Report : Base
    {
        public DateTime CreatedOn { get; set; }
        public string? Content { get; set; }
    }
}