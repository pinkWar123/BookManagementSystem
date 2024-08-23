using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Invoice;
using BookManagementSystem.Application.Dtos.PaymentReceipt;
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
        private readonly IPaymentReceiptService _paymentReceiptService;
        private readonly ICustomerService _customerService;
        private readonly IInvoiceService _invoiceService;
        public StatisticController(IPaymentReceiptService paymentReceiptService, ICustomerService customerService, IInvoiceService invoiceService)
        {
            _paymentReceiptService = paymentReceiptService;
            _customerService = customerService;
            _invoiceService = invoiceService;
        }

        [HttpGet("income")]
        public async Task<IActionResult> GetIncome([FromQuery] int month, [FromQuery] int year)
        {
            var income = await _paymentReceiptService.GetTotalAmountByMonthYear(month, year);
            var incomeViewDto = new IncomeViewDto
            {
                Month = month,
                Year = year,
                Income = income
            };
            return Ok(new Response<IncomeViewDto>(incomeViewDto));
        }

        [HttpGet("customer-count")]
        public async Task<IActionResult> GetTotalCustomer()
        {
            var customerCount = await _customerService.GetCustomerCount();
            return Ok(new Response<int>(customerCount));
        }

        [HttpGet("invoice-count")]
        public async Task<IActionResult> GetInvoiceCount([FromQuery] int month, [FromQuery] int year)
        {
            var invoiceCount = await _invoiceService.GetInvoiceCountByMonthYear(month, year);
            return Ok(new Response<int>(invoiceCount));
        }

        [HttpGet("income-history")]
        public async Task<IActionResult> GetIncomeFromJanuary()
        {
            var incomeList = await _paymentReceiptService.GetIncomeFromJanuary();
            return Ok(new Response<List<IncomeByMonthDto>>(incomeList));
        }
    }
}