using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using BookManagementSystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Data;
using BookManagementSystem.Application.Dtos.InventoryReport;
using BookManagementSystem.Application.Dtos.InventoryReportDetail;

namespace BookManagementSystem.Services
{
    public class InventoryReportMonthlyCreateBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public InventoryReportMonthlyCreateBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime now = DateTime.Now;
                DateTime nextRunTime = new DateTime(now.Year, now.Month, 1, 0, 0, 0).AddMonths(1);


                TimeSpan delay = nextRunTime - now;


                await Task.Delay(delay, stoppingToken);
                

                using (var scope = _serviceProvider.CreateScope()) // Tạo scope mới
                {
                    var inventoryReportService = scope.ServiceProvider.GetRequiredService<IInventoryReportService>();
                    var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();
                    var inventoryReportDetailService = scope.ServiceProvider.GetRequiredService<IInventoryReportDetailService>();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

                    using (var transaction = await context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var createInventoryReportDto = new CreateInventoryReportDto
                            {
                                ReportMonth = DateTime.Now.Month,
                                ReportYear = DateTime.Now.Year
                            };
                            var inventoryReport = await inventoryReportService.CreateInventoryReport(createInventoryReportDto);
                            await context.SaveChangesAsync();
                            
                            var bookIds = await bookService.GetAllBookId();

                            foreach (var BookId in bookIds)
                            {
                                var book = await bookService.GetBookById(BookId);
                                var createinventoryReportDetailDto = new CreateInventoryReportDetailDto
                                {
                                    ReportID = inventoryReport.ReportID,
                                    BookID = BookId,
                                    InitialStock = book.StockQuantity,
                                    FinalStock = book.StockQuantity,
                                    AdditionalStock = 0
                                };
                                await inventoryReportDetailService.CreateInventoryReportDetail(createinventoryReportDetailDto);
                            }


                            await context.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
            }
        }
    }
}