using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.DebtReportDetail;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.DebtReportDetail;
using BookManagementSystem.Application.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace BookManagementSystem.Application.Services
{
    public class DebtReportDetailService : IDebtReportDetailService
    {
        private readonly IDebtReportDetailRepository _debtReportDetailRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDebtReportDetailDto> _createValidator;
        private readonly IValidator<UpdateDebtReportDetailDto> _updateValidator;

        public DebtReportDetailService(
            IDebtReportDetailRepository debtReportDetailRepository, 
            IMapper mapper, 
            IValidator<CreateDebtReportDetailDto> createValidator,
            IValidator<UpdateDebtReportDetailDto> updateValidator)
        {
            _debtReportDetailRepository = debtReportDetailRepository ?? throw new ArgumentNullException(nameof(debtReportDetailRepository));
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<DebtReportDetailDto> CreateNewDebtReportDetail(CreateDebtReportDetailDto createDebtReportDetailDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDebtReportDetailDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var debtReportDetail = _mapper.Map<DebtReportDetail>(createDebtReportDetailDto);
            await _debtReportDetailRepository.AddAsync(debtReportDetail);
            return _mapper.Map<DebtReportDetailDto>(debtReportDetail);
        }

        public async Task<DebtReportDetailDto> UpdateDebtReportDetail(string reportId, string customerId, UpdateDebtReportDetailDto updateDebtReportDetailDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateDebtReportDetailDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // write again GetByIdAsync
            // var existingDetail = await _debtReportDetailRepository.GetByIdAsync(reportId, customerId);
            var existingDetail = await _debtReportDetailRepository.GetByIdAsync(reportId);

            if (existingDetail == null)
            {
                throw new KeyNotFoundException($"DebtReportDetail with ReportID {reportId} and CustomerID {customerId} not found.");
            }

            _mapper.Map(updateDebtReportDetailDto, existingDetail);

            // write again UpdateAsync
            // var updatedDetail = await _debtReportDetailRepository.UpdateAsync(reportId, customerId, existingDetail);
            var updatedDetail = await _debtReportDetailRepository.UpdateAsync(reportId, existingDetail);

            return _mapper.Map<DebtReportDetailDto>(updatedDetail);
        }

        public async Task<DebtReportDetailDto> GetDebtReportDetailById(string reportId, string customerId)
        {
            // var debtReportDetail = await _debtReportDetailRepository.GetByIdAsync(reportId, customerId);
            var debtReportDetail = await _debtReportDetailRepository.GetByIdAsync(reportId);
            if (debtReportDetail == null)
            {
                throw new KeyNotFoundException($"DebtReportDetail with ReportID {reportId} and CustomerID {customerId} not found.");
            }
            return _mapper.Map<DebtReportDetailDto>(debtReportDetail);
        }

        // public async Task<IEnumerable<DebtReportDetailDto>> GetAllDebtReportDetails()
        // {
        //     var debtReportDetails = await _debtReportDetailRepository.GetAllAsync();
        //     return _mapper.Map<IEnumerable<DebtReportDetailDto>>(debtReportDetails);
        // }

        public async Task<bool> DeleteDebtReportDetail(string reportId, string customerId)
        {
            // var debtReportDetail = await _debtReportDetailRepository.GetByIdAsync(reportId, customerId);
            var debtReportDetail = await _debtReportDetailRepository.GetByIdAsync(reportId);
            if (debtReportDetail == null)
            {
                return false;
            }
            _debtReportDetailRepository.Remove(debtReportDetail);
            return true;
        }
    }
}