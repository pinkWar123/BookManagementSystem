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

namespace BookManagementSystem.Application.Services
{
    public class DebtReportService : IDebtReportService
    {
        private readonly IDebtReportRepository _debtReportRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDebtReportDto> _createValidator;
        private readonly IValidator<UpdateDebtReportDto> _updateValidator;

        public DebtReportService(
            IDebtReportRepository debtReportRepository,
            IMapper mapper,
            IValidator<CreateDebtReportDto> createValidator,
            IValidator<UpdateDebtReportDto> updateValidator)
        {
            _debtReportRepository = debtReportRepository ?? throw new ArgumentNullException(nameof(debtReportRepository));
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<DebtReportDto> CreateNewDebtReport(CreateDebtReportDto createDebtReportDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDebtReportDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var debtReport = _mapper.Map<Domain.Entities.DebtReport>(createDebtReportDto);
            await _debtReportRepository.AddAsync(debtReport);
            await _debtReportRepository.SaveChangesAsync();
            return _mapper.Map<DebtReportDto>(debtReport);
        }

        public async Task<DebtReportDto> UpdateDebtReport(int reportId, UpdateDebtReportDto updateDebtReportDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDebtReportDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingReport = await _debtReportRepository.GetByIdAsync(reportId);
            if (existingReport == null)
            {
                throw new KeyNotFoundException($"DebtReport with ID {reportId} not found.");
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
                throw new KeyNotFoundException($"DebtReport with ID {reportId} not found.");
            }
            return _mapper.Map<DebtReportDto>(debtReport);
        }

        // public async Task<IEnumerable<DebtReportDto>> GetAllDebtReports()
        // {
        //     var debtReports = await _debtReportRepository.GetValuesAsync();
        //     return _mapper.Map<IEnumerable<DebtReportDto>>(debtReports);
        // }
        // public async Task<IEnumerable<DebtReportDto>> GetAllDebtReports()
        // {
        //     var debtReports = await _debtReportRepository.GetContext().ToListAsync();
        //     return _mapper.Map<IEnumerable<DebtReportDto>>(debtReports);
        // }

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
