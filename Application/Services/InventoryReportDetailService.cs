using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.InventoryReportDetail;
using BookManagementSystem.Application.Dtos.InventoryReport;
using BookManagementSystem.Application.Dtos.BookEntry;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Queries;
using BookManagementSystem.Application.Validators;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.InventoryReportDetail;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Application.Dtos.BookEntryDetail;

namespace BookManagementSystem.Application.Services
{
    public class InventoryReportDetailService : IInventoryReportDetailService
    {

        private readonly IInventoryReportDetailRepository _inventoryReportDetailRepository;
        private readonly IInventoryReportService _inventoryReportService;
        private readonly IMapper _mapper;

        public InventoryReportDetailService(
            IInventoryReportDetailRepository _inventoryReportDetailRepository,
            IMapper mapper,
            IInventoryReportService _inventoryReportService)
        {
            this._inventoryReportDetailRepository = _inventoryReportDetailRepository;
            this._inventoryReportService = _inventoryReportService;
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
                throw new KeyNotFoundException($"Inventory Report with ID:  {reportId} not found.");
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
            var updatedReport = await _inventoryReportDetailRepository.UpdateAsync(reportId, BookID, existingReport);
            await _inventoryReportDetailRepository.SaveChangesAsync();
            return _mapper.Map<InventoryReportDetailDto>(updatedReport);
        }

        public async Task<InventoryReportDetailDto> CreateInventoryFromBookEntry(BookEntryDetailDto entry)
        {
            var now = DateTime.Now;

            // Lấy Report dưới dạng bất đồng bộ và await nó
            var reportIdTask = _inventoryReportService.GetReportIdByMonthYear(now.Month, now.Year);
            var reportId = await reportIdTask;

            // Nếu report không tồn tại, tạo mới
            if (reportId == -1)
            {
                var newReport = new CreateInventoryReportDto
                {
                    ReportMonth = now.Month,
                    ReportYear = now.Year
                };

                await _inventoryReportService.CreateInventoryReport(newReport);

                // Sau khi tạo báo cáo, lấy lại ID của báo cáo mới
                reportId = await _inventoryReportService.GetReportIdByMonthYear(now.Month, now.Year);
            }
            Console.WriteLine("++++++++++++ddmdmmdmdmmd++++++++++++++++++++++++++++++++++++++++++++++++++");
            // Lấy inventory report detail theo reportId và BookID
            var inventoryreportdetail = await _inventoryReportDetailRepository.GetByIdAsync(reportId, entry.BookID);
            Console.WriteLine("++++++++++++ddmdmmdmdmmd++++++++++++++++++++++++++++++++++++++++++++++++++");
            // Nếu không tồn tại, tạo mới
            if (inventoryreportdetail == null)
            {
                var newInventoryReportDetail = new CreateInventoryReportDetailDto
                {
                    InitialStock = 0,
                    FinalStock = entry.Quantity,
                    AdditionalStock = entry.Quantity,
                    ReportID = reportId,
                    BookID = entry.BookID
                };Console.WriteLine("++++++++++++ddmdmmdmdmmd++++++++++++++++++++++++++++++++++++++++++++++++++");
                
                return await CreateInventoryReportDetail(newInventoryReportDetail);
            }

            // Cập nhật inventory report detail hiện có
            var updateinventoryreportdetail = new UpdateInventoryReportDetailDto
            {
                InitialStock = inventoryreportdetail.InitalStock,
                AdditionalStock = inventoryreportdetail.AdditionalStock + entry.Quantity,
                FinalStock = inventoryreportdetail.FinalStock + entry.Quantity
            };

            return await UpdateInventoryReportDetail(reportId, entry.BookID, updateinventoryreportdetail);
        }


    }
}