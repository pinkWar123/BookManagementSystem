using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.InventoryReportDetail;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.InventoryReportDetail;
using FluentValidation;
using FluentValidation.Results;

namespace BookManagementSystem.Application.Services
{
    public class InventoryReportDetailService : IInventoryReportDetailService
    {

        private readonly IInventoryReportDetailRepository _inventoryReportDetailRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateInventoryReportDetailDto> _createValidator;
        private readonly IValidator<UpdateInventoryReportDetailDto> _updateValidator;


        public InventoryReportDetailService(
            IInventoryReportDetailRepository _inventoryReportDetailRepository,
            IMapper mapper,
            IValidator<CreateInventoryReportDetailDto> _createValidator,
            IValidator<UpdateInventoryReportDetailDto> _updateValidator)
        {
            this._inventoryReportDetailRepository = _inventoryReportDetailRepository;
            _mapper = mapper;
            this._createValidator = _createValidator;
            this._updateValidator = _updateValidator;
        }

        public async Task<InventoryReportDetailDto> CreateInventoryReportDetail(CreateInventoryReportDetailDto _createInventoryReportDetailDto)
        {
            var validationResult = await _createValidator.ValidateAsync(_createInventoryReportDetailDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var InventoryReport = _mapper.Map<InventoryReportDetail>(_createInventoryReportDetailDto);
            await _inventoryReportDetailRepository.AddAsync(InventoryReport);
            await _inventoryReportDetailRepository.SaveChangesAsync();
            return _mapper.Map<InventoryReportDetailDto>(InventoryReport);
        }

        public async Task<bool> DeleteAllInventoryReportDetailWithReportId(int reportId) {
            var InventoryReportDetaillist = await _inventoryReportDetailRepository.GetListInventoryReportDetailsByIdAsync(reportId);
            if(InventoryReportDetaillist == null)
                return false;
            
            foreach (var temp in InventoryReportDetaillist)
            {
                _inventoryReportDetailRepository.Remove(temp);
            }
            await _inventoryReportDetailRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteInventoryReportDetail(int reportId, int BookID)
        {
            // var debtReportDetail = await _debtReportDetailRepository.GetByIdAsync(reportId, customerId);
            var InventoryReportDetail = await _inventoryReportDetailRepository.GetByIdAsync(reportId);
            if (InventoryReportDetail == null)
            {
                return false;
            }
            _inventoryReportDetailRepository.Remove(InventoryReportDetail);
            await _inventoryReportDetailRepository.SaveChangesAsync();
            return true;
        }

        public async Task<InventoryReportDetailDto> GetInventoryReportDetailById(int reportId, int BookID)
        {
            var existingReport = await _inventoryReportDetailRepository.GetByIdAsync(reportId, BookID);
            if (existingReport == null)
            {
                throw new KeyNotFoundException($"DebtReport with ID:  {reportId} not found.");
            }

            //_mapper.Map(_updateInventoryReportDetailDto, existingReport);

            return _mapper.Map<InventoryReportDetailDto>(existingReport);
        }

        public async Task<InventoryReportDetailDto> UpdateInventoryReportDetail(int reportId, int BookID, UpdateInventoryReportDetailDto _updateInventoryReportDetailDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(_updateInventoryReportDetailDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingReport = await _inventoryReportDetailRepository.GetByIdAsync(reportId, BookID);
            if (existingReport == null)
            {
                throw new KeyNotFoundException($"DebtReport with ID:  {reportId} not found.");
            }

            _mapper.Map(_updateInventoryReportDetailDto, existingReport);
            var temp = _mapper.Map<UpdateInventoryReportDetailDto>(existingReport);
            var updatedReport = await _inventoryReportDetailRepository.UpdateAsync(reportId, BookID, temp);
            await _inventoryReportDetailRepository.SaveChangesAsync();
            return _mapper.Map<InventoryReportDetailDto>(updatedReport);
        }

       
    }
}