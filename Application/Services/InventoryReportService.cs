using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.InventoryReport;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.InventoryReport;
using BookManagementSystem.Infrastructure.Repositories.InventoryReportDetail;
using FluentValidation;
using FluentValidation.Results;
namespace BookManagementSystem.Application.Services
{
    public class InventoryReportService : IInventoryReportService
    {
        private readonly IInventoryReportRepository _inventoryReportRepository;
        private readonly IInventoryReportDetailService _inventoryReportDetailService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateInventoryReportDto> _createValidator;
        private readonly IValidator<UpdateInventoryReportDto> _updateValidator;


        public InventoryReportService(
            IInventoryReportRepository _inventoryReportRepository,
            IInventoryReportDetailRepository _inventoryReportDetailRepository,
            IMapper mapper,
            IValidator<CreateInventoryReportDto> _createValidator,
            IValidator<UpdateInventoryReportDto> _updateValidator)
        {
            this._inventoryReportRepository = _inventoryReportRepository;
            this._inventoryReportDetailService = _inventoryReportDetailService;
            _mapper = mapper;
            this._createValidator = _createValidator;
            this._updateValidator = _updateValidator;
        }

        public async Task<InventoryReportDto> CreateInventoryReport(CreateInventoryReportDto _createInventoryReportDto)
        {
            var validationResult = await _createValidator.ValidateAsync(_createInventoryReportDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var InventoryReport = _mapper.Map<InventoryReport>(_createInventoryReportDto);
            await _inventoryReportRepository.AddAsync(InventoryReport);
            await _inventoryReportRepository.SaveChangesAsync();
            return _mapper.Map<InventoryReportDto>(InventoryReport);
        }

        public async Task<bool> DeleteInventoryReport(int reportId)
        {
            var inventoryReport = await _inventoryReportRepository.GetByIdAsync(reportId);
            if (inventoryReport == null)
            {
                return false;
            }
            bool checkdelete = await _inventoryReportDetailService.DeleteAllInventoryReportDetailWithReportId(reportId);
            if(checkdelete == false)
                return false;
            _inventoryReportRepository.Remove(inventoryReport);
            await _inventoryReportRepository.SaveChangesAsync();
            
            return true;
        }

        public async Task<InventoryReportDto> GetInventoryReportById(int reportId)
        {
            var inventoryReport = await _inventoryReportRepository.GetByIdAsync(reportId);
            if (inventoryReport == null)
            {
                throw new KeyNotFoundException("Inventory report : " + reportId + "is not found in inventory");
            }

            return _mapper.Map<InventoryReportDto>(inventoryReport);
        }

        public async Task<InventoryReportDto> UpdateInventoryReport(int reportId, UpdateInventoryReportDto _updateInventoryReportDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(_updateInventoryReportDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingReport = await _inventoryReportRepository.GetByIdAsync(reportId);
            if (existingReport == null)
            {
                throw new KeyNotFoundException($"DebtReport with ID {reportId} not found.");
            }

            _mapper.Map(_updateInventoryReportDto, existingReport);
            var updatedReport = await _inventoryReportRepository.UpdateAsync(reportId, existingReport);
            await _inventoryReportRepository.SaveChangesAsync();
            return _mapper.Map<InventoryReportDto>(updatedReport);
        }
    }
}