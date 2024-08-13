using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Invoice;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementSystem.Api.Controllers
{
    [Authorize]
    [Route("api/statistic")]
    public class StatisticController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        public StatisticController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet("income")]
        public async Task<IActionResult> GetIncome([FromQuery] int month, [FromQuery] int year)
        {
            var income = await _invoiceService.getPriceByMonth(month, year);
            return Ok(new Response<IncomeViewDto>(income));
        }
    }
}