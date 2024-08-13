using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookManagementSystem.Application.Dtos.InvoiceDetail;

namespace BookManagementSystem.Application.Dtos.Invoice
{
    public class CreateInvoiceDto
    {
        [Required]
        public List<CreateInvoiceDetailDto>? InvoiceDetails { get; set; }        
        [Required]
        public string? InvoiceDate { get; set; }
        public int? CustomerID { get; set; }
    }
}