using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using BookManagementSystem.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Data;
using BookManagementSystem.Application.Dtos.DebtReport;
using BookManagementSystem.Application.Dtos.DebtReportDetail;

namespace BookManagementSystem.Services
{
    public class DebtReportBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public DebtReportBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime now = DateTime.Now;
                DateTime nextRunTime = new DateTime(now.Year, now.Month, 1, 0, 0, 0).AddMonths(1);

                // DateTime nextRunTime = now.AddMinutes(1); 

                TimeSpan delay = nextRunTime - now;

                // Console.WriteLine($"Current time: {now}");
                // Console.WriteLine($"Scheduled run time: {nextRunTime}");
                // Console.WriteLine($"Delay until next run: {delay.TotalSeconds} seconds ({delay})");

                await Task.Delay(delay, stoppingToken);
                

                using (var scope = _serviceProvider.CreateScope()) // Tạo scope mới
                {
                    var debtReportService = scope.ServiceProvider.GetRequiredService<IDebtReportService>();
                    var customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();
                    var debtReportDetailService = scope.ServiceProvider.GetRequiredService<IDebtReportDetailService>();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

                    using (var transaction = await context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var ReportMonth = DateTime.UtcNow.Month;
                            var ReportYear = DateTime.UtcNow.Year;
                            var debtReportId = await debtReportService.GetReportIdByMonthYear(ReportMonth, ReportYear);

                            if(debtReportId == 0)
                            {
                                var createDebtReportDto = new CreateDebtReportDto
                                {
                                    ReportMonth = DateTime.UtcNow.Month,
                                    ReportYear = DateTime.UtcNow.Year
                                };
                                var debtReport = await debtReportService.CreateNewDebtReport(createDebtReportDto);
                                await context.SaveChangesAsync();
                                debtReportId = debtReport.Id;
                            }
                            // Bước 1: Tạo báo cáo nợ (Debt Report)
                            
                            // Bước 2: Lấy danh sách ID các khách hàng
                            var customerIds = await customerService.GetAllCustomerId();

                            // Bước 3: Tạo chi tiết báo cáo nợ cho mỗi khách hàng
                            foreach (var customerId in customerIds)
                            {
                                var customer = await customerService.GetCustomerById(customerId);
                                var createDebtReportDetailDto = new CreateDebtReportDetailDto
                                {
                                    ReportID = debtReportId,
                                    CustomerID = customerId,
                                    InitialDebt = customer.TotalDebt,
                                    FinalDebt = customer.TotalDebt
                                };
                                await debtReportDetailService.CreateNewDebtReportDetail(createDebtReportDetailDto);
                            }

                            // Console.WriteLine("Doneeeeeeeeeeeeee");

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
