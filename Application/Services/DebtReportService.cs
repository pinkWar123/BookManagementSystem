using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.DebtReport;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.DebtReport;
using BookManagementSystem.Infrastructure.Repositories.DebtReportDetail;
using FluentValidation;
using FluentValidation.Results;
using BookManagementSystem.Application.Exceptions;
using System.Net;
using BookManagementSystem.Application.Queries;
using BookManagementSystem.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Application.Dtos.DebtReportDetail;


namespace BookManagementSystem.Application.Services
{
    public class DebtReportService : IDebtReportService
    {
        private readonly IDebtReportRepository _debtReportRepository;
        private readonly IDebtReportDetailRepository _debtReportDetailRepository;
        private readonly IMapper _mapper;
        public DebtReportService(
            IDebtReportRepository debtReportRepository,
            IDebtReportDetailRepository debtReportDetailRepository,
            IMapper mapper)
        {
            _debtReportRepository = debtReportRepository ?? throw new ArgumentNullException(nameof(debtReportRepository));
            _debtReportDetailRepository = debtReportDetailRepository ?? throw new ArgumentNullException(nameof(debtReportDetailRepository));
            _mapper = mapper;
        }

        public async Task<DebtReportDto> CreateNewDebtReport(CreateDebtReportDto createDebtReportDto)
        {
            var debtReport = _mapper.Map<Domain.Entities.DebtReport>(createDebtReportDto);
            await _debtReportRepository.AddAsync(debtReport);
            await _debtReportRepository.SaveChangesAsync();
            return _mapper.Map<DebtReportDto>(debtReport);
        }

        public async Task<DebtReportDto> UpdateDebtReport(int reportId, UpdateDebtReportDto updateDebtReportDto)
        {
            var existingReport = await _debtReportRepository.GetByIdAsync(reportId);
            if (existingReport == null)
            {
                throw new DebtReportNotFound(reportId);
            }

            _mapper.Map(updateDebtReportDto, existingReport);
            var updatedReport = await _debtReportRepository.UpdateAsync(reportId, existingReport);
            await _debtReportRepository.SaveChangesAsync();
            return _mapper.Map<DebtReportDto>(updatedReport);
        }

        public async Task<DebtReportDto> GetDebtReportById(int reportId)
        {
            var debtReport = await _debtReportRepository.GetByIdAsync(reportId);
            if (debtReport == null)
            {
                throw new DebtReportNotFound(reportId);
            }
            return _mapper.Map<DebtReportDto>(debtReport);
        }

        public async Task<IEnumerable<DebtReportDto>> GetAllDebtReports(DebtReportQuery debtReportQuery)
        {
            var query = _debtReportRepository.GetValuesByQuery(debtReportQuery);
            if (query == null)
            {
                return Enumerable.Empty<DebtReportDto>();
            }
            var debtReports = await query.ToListAsync();

            return _mapper.Map<IEnumerable<DebtReportDto>>(debtReports);
        }

        public async Task<bool> DeleteDebtReport(int reportId)
        {
            var debtReport = await _debtReportRepository.GetByIdAsync(reportId);
            if (debtReport == null)
            {
                return false;
            }
            _debtReportRepository.Remove(debtReport);
            await _debtReportRepository.SaveChangesAsync();
            return true;
        }
        public async Task<int> GetReportIdByMonthYear(int month, int year)
        {
            return await _debtReportRepository.GetReportIdByMonthYearAsync(month, year);
        }

        public async Task<bool> DebtReportExists(int month, int year)
        {
            return await _debtReportRepository.DebtReportExists(month, year);
        }

        // public async Task<IEnumerable<AllDebtReportDetailDto>> GetAllDebtReportDetailsByMonth(int month, int year, DebtReportQuery debtReportQuery)
        // {
        //     int reportId = await GetReportIdByMonthYear(month, year);
            
        //     var debtReportDetails = await _debtReportRepository.GetDebtReportDetailsByReportIdAsync(reportId, debtReportQuery);

        //     if (debtReportDetails == null || !debtReportDetails.Any())
        //     {
        //         // throw new DebtReportDetailListNotFound(reportId);
        //         return new List<AllDebtReportDetailDto>();
        //     }

        //     return debtReportDetails;
        // }

        public async Task<IEnumerable<AllDebtReportDetailDto>> GetAllDebtReportDetailsByMonth(DebtReportQuery debtReportQuery)
        {
            if (debtReportQuery.ReportMonth != null && debtReportQuery.ReportYear != null)
            {
                var reportId = await _debtReportRepository.GetReportIdByMonthYearAsync(
                    (int)debtReportQuery.ReportMonth,
                    (int)debtReportQuery.ReportYear
                );
                Console.WriteLine("++++++++++++++++++++++++++++: " + reportId);

                if(reportId == -1)
                    return Enumerable.Empty<AllDebtReportDetailDto>();

                var debtReportDetailQuery = new DebtReportDetailQuery
                {
                    PageNumber = debtReportQuery.PageNumber,
                    PageSize = debtReportQuery.PageSize,
                    ReportId = reportId == -1 ? null : reportId,
                    SortBy = debtReportQuery.SortBy,
                    IsDescending = debtReportQuery.IsDescending,
                };

                var query = _debtReportDetailRepository.GetValuesByQuery(debtReportDetailQuery);

                if (query == null || !query.Any())
                {
                    return Enumerable.Empty<AllDebtReportDetailDto>();
                }

                var debtReportDetails = await query
                    .Include(ird => ird.Customer)
                    .ToListAsync();

                var result = debtReportDetails.Select(ird => new AllDebtReportDetailDto
                {
                    ReportID = ird.ReportID,
                    CustomerID = ird.CustomerID,
                    customerName = ird.Customer.CustomerName,
                    InitialDebt = ird.InitialDebt,
                    FinalDebt = ird.FinalDebt,
                    AdditionalDebt = ird.AdditionalDebt
                }).ToList();

                return result; 
            }

            return Enumerable.Empty<AllDebtReportDetailDto>(); 
        }


        // public async Task<IEnumerable<GetAllDebtReportDto>> GetAllDebtReportDetails(DebtReportQuery debtReportQuery)
        // {
        //     var query = _debtReportRepository.GetValuesByQuery(debtReportQuery);

        //     if (query == null)
        //     {
        //         return Enumerable.Empty<GetAllDebtReportDto>();
        //     }

        //     var debtReports = await query
        //         .Include(dr => dr.DebtReportDetails)
        //             .ThenInclude(drd => drd.Customer)
        //         .ToListAsync();

        //     var result = debtReports.Select(dr => new GetAllDebtReportDto
        //     {
        //         Id = dr.Id,
        //         ReportMonth = dr.ReportMonth,
        //         ReportYear = dr.ReportYear,
        //         DebtReportDetails = dr.DebtReportDetails.Select(drd => new AllDebtReportDetailDto
        //         {
        //             ReportID = dr.Id,
        //             CustomerID = drd.CustomerID,
        //             customerName = drd.Customer != null ? drd.Customer.CustomerName : "Unknown",
        //             InitialDebt = drd.InitialDebt,
        //             FinalDebt = drd.FinalDebt,
        //             AdditionalDebt = drd.AdditionalDebt
        //         }).ToList()
        //     });

        //     return result;
        // }
    }
}
