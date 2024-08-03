using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.InventoryReportDetail;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Queries;
using BookManagementSystem.Application.Validators;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.InventoryReportDetail;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Application.Services
{
    public class InventoryReportDetailService : IInventoryReportDetailService
    {

        private readonly IInventoryReportDetailRepository _inventoryReportDetailRepository;
        private readonly IMapper _mapper;

        public InventoryReportDetailService(
            IInventoryReportDetailRepository _inventoryReportDetailRepository,
            IMapper mapper,
            IValidator<CreateInventoryReportDetailDto> _createValidator,
            IValidator<UpdateInventoryReportDetailDto> _updateValidator)
        {
            this._inventoryReportDetailRepository = _inventoryReportDetailRepository;
            _mapper = mapper;
        }

        public async Task<InventoryReportDetailDto> CreateInventoryReportDetail(CreateInventoryReportDetailDto _createInventoryReportDetailDto)
        {

            var InventoryReport = _mapper.Map<InventoryReportDetail>(_createInventoryReportDetailDto);
            await _inventoryReportDetailRepository.AddAsync(InventoryReport);
            await _inventoryReportDetailRepository.SaveChangesAsync();
            return _mapper.Map<InventoryReportDetailDto>(InventoryReport);
        }

        public async Task<bool> DeleteAllInventoryReportDetailWithReportId(int reportId)
        {
            var inventoryReportDetailList = await _inventoryReportDetailRepository.GetListInventoryReportDetailsByIdAsync(reportId);
            if (inventoryReportDetailList == null || !inventoryReportDetailList.Any())
            {
                return false;
            }

            foreach (var detail in inventoryReportDetailList)
            {
                _inventoryReportDetailRepository.Remove(detail);
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

        public async Task<IEnumerable<InventoryReportDetailDto>> GetAllInventoryReportDetails(InventoryReportDetailQuery inventoryReportDetailQuery) 
        {
            var query = _inventoryReportDetailRepository.GetValuesByQuery(inventoryReportDetailQuery);
            if (query == null)
            {
                return Enumerable.Empty<InventoryReportDetailDto>();
            }
            var debtReports = await query.ToListAsync();

            return _mapper.Map<IEnumerable<InventoryReportDetailDto>>(debtReports);
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