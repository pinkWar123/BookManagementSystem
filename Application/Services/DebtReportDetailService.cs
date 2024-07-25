using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.DebtReportDetail;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.DebtReportDetail;
using FluentValidation;
using FluentValidation.Results;
using BookManagementSystem.Application.Exceptions;
using System.Net;
 
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
            await _debtReportDetailRepository.SaveChangesAsync();
            return _mapper.Map<DebtReportDetailDto>(debtReportDetail);
        }

        public async Task<DebtReportDetailDto> UpdateDebtReportDetail(int reportId, int customerId, UpdateDebtReportDetailDto updateDebtReportDetailDto)
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
                throw new DebtReportDetailException($"Không tìm thấy báo cáo với ID báo cáo là {reportId} và ID khách hàng là {customerId}.", HttpStatusCode.NotFound);
            }

            _mapper.Map(updateDebtReportDetailDto, existingDetail);

            // write again UpdateAsync
            // var updatedDetail = await _debtReportDetailRepository.UpdateAsync(reportId, customerId, existingDetail);
            var updatedDetail = await _debtReportDetailRepository.UpdateAsync(reportId, existingDetail);
            await _debtReportDetailRepository.SaveChangesAsync();
            return _mapper.Map<DebtReportDetailDto>(updatedDetail);
        }

        public async Task<DebtReportDetailDto> GetDebtReportDetailById(int reportId, int customerId)
        {
            // var debtReportDetail = await _debtReportDetailRepository.GetByIdAsync(reportId, customerId);
            var debtReportDetail = await _debtReportDetailRepository.GetByIdAsync(reportId);
            if (debtReportDetail == null)
            {
                throw new DebtReportDetailException($"Không tìm thấy báo cáo với ID báo cáo là {reportId} và ID khách hàng là {customerId}.", HttpStatusCode.NotFound);
            }
            return _mapper.Map<DebtReportDetailDto>(debtReportDetail);
        }

        // public async Task<IEnumerable<DebtReportDetailDto>> GetAllDebtReportDetails()
        // {
        //     var debtReportDetails = await _debtReportDetailRepository.GetAllAsync();
        //     return _mapper.Map<IEnumerable<DebtReportDetailDto>>(debtReportDetails);
        // }

        public async Task<bool> DeleteDebtReportDetail(int reportId, int customerId)
        {
            // var debtReportDetail = await _debtReportDetailRepository.GetByIdAsync(reportId, customerId);
            var debtReportDetail = await _debtReportDetailRepository.GetByIdAsync(reportId);
            if (debtReportDetail == null)
            {
                return false;
            }
            _debtReportDetailRepository.Remove(debtReportDetail);
            await _debtReportDetailRepository.SaveChangesAsync();
            return true;
        }
    }
}
