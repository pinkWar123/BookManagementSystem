using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.InventoryReport;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Queries;
using BookManagementSystem.Application.Validators;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.InventoryReport;
using BookManagementSystem.Infrastructure.Repositories.InventoryReportDetail;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
namespace BookManagementSystem.Application.Services
{
    public class InventoryReportService : IInventoryReportService
    {
        private readonly IInventoryReportRepository _inventoryReportRepository;
        private readonly IInventoryReportDetailService _inventoryReportDetailService;
        private readonly IMapper _mapper;


        public InventoryReportService(
            IInventoryReportRepository _inventoryReportRepository,
            IInventoryReportDetailService inventoryReportDetailService,
            IMapper mapper)
        {
            this._inventoryReportRepository = _inventoryReportRepository;
            this._inventoryReportDetailService = inventoryReportDetailService;
            _mapper = mapper;

        }
        public async Task<int> GetReportIdByMonthYear(int month, int year)
        {
            return await _inventoryReportRepository.GetReportIdByMonthYearAsync(month, year);
        }

        public async Task<InventoryReportDto> CreateInventoryReport(CreateInventoryReportDto _createInventoryReportDto)
        {
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
            
            _inventoryReportRepository.Remove(inventoryReport);
            await _inventoryReportRepository.SaveChangesAsync();
            
            return true;
        }

        public async Task<IEnumerable<InventoryReportDto>> GetAllDebtReports(InventoryReportQuery InventoryReportQuery) 
        {
            var query = _inventoryReportRepository.GetValuesByQuery(InventoryReportQuery);
            if (query == null)
            {
                return Enumerable.Empty<InventoryReportDto>();
            }
            var inventoryreprts = await query.ToListAsync();

            return _mapper.Map<IEnumerable<InventoryReportDto>>(inventoryreprts);
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


            var existingReport = await _inventoryReportRepository.GetByIdAsync(reportId);
            if (existingReport == null)
            {
                throw new KeyNotFoundException($"Inventory Report with ID {reportId} not found.");
            }

            _mapper.Map(_updateInventoryReportDto, existingReport);
            var updatedReport = await _inventoryReportRepository.UpdateAsync(reportId, existingReport);
            await _inventoryReportRepository.SaveChangesAsync();
            return _mapper.Map<InventoryReportDto>(updatedReport);
        }
    }
}