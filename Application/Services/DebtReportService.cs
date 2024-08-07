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
using FluentValidation;
using FluentValidation.Results;
using BookManagementSystem.Application.Exceptions;
using System.Net;
using BookManagementSystem.Application.Queries;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Application.Services
{
    public class DebtReportService : IDebtReportService
    {
        private readonly IDebtReportRepository _debtReportRepository;
        private readonly IMapper _mapper;
        public DebtReportService(
            IDebtReportRepository debtReportRepository,
            IMapper mapper)
        {
            _debtReportRepository = debtReportRepository ?? throw new ArgumentNullException(nameof(debtReportRepository));
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
                throw new DebtReportException($"Không tìm thấy báo cáo với ID {reportId}.", HttpStatusCode.NotFound);
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
                throw new DebtReportException($"Không tìm thấy báo cáo với ID {reportId}.", HttpStatusCode.NotFound);
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
    }
}
