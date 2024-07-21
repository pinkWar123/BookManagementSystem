using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    public class Base
    {
        [StringLength(5)]
        [Column(TypeName = "char(5)")]
        public required string Id { get; set; }
    }
}
