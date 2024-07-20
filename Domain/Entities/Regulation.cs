using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Regulation : Base
    {
        [StringLength(5)]
        [Column(TypeName = "char(5)")]
        public required string Code { get; set; }
        [StringLength(200)]
        public required string Content { get; set; }
        public required int Value { get; set; }
        public required bool Status { get; set; } // 1 = true, 0 = false
    }
}
