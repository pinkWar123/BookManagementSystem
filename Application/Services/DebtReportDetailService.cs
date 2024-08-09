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
using BookManagementSystem.Application.Queries;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Application.Services
{
    public class DebtReportDetailService : IDebtReportDetailService
    {
        private readonly IDebtReportDetailRepository _debtReportDetailRepository;
        private readonly IMapper _mapper;

        public DebtReportDetailService(
            IDebtReportDetailRepository debtReportDetailRepository,
            IMapper mapper)
        {
            _debtReportDetailRepository = debtReportDetailRepository ?? throw new ArgumentNullException(nameof(debtReportDetailRepository));
            _mapper = mapper;
        }

        public async Task<DebtReportDetailDto> CreateNewDebtReportDetail(CreateDebtReportDetailDto createDebtReportDetailDto)
        {
            var debtReportDetail = _mapper.Map<DebtReportDetail>(createDebtReportDetailDto);
            
            if (createDebtReportDetailDto.InitialDebt.HasValue && createDebtReportDetailDto.FinalDebt.HasValue)
            {
                debtReportDetail.AdditionalDebt = createDebtReportDetailDto.FinalDebt.Value - createDebtReportDetailDto.InitialDebt.Value;
            }
            else
            {
                debtReportDetail.AdditionalDebt = 0;
            }
            
            await _debtReportDetailRepository.AddAsync(debtReportDetail);
            await _debtReportDetailRepository.SaveChangesAsync();
            return _mapper.Map<DebtReportDetailDto>(debtReportDetail);
        }

        public async Task<DebtReportDetailDto> UpdateDebtReportDetail(int reportId, int customerId, UpdateDebtReportDetailDto updateDebtReportDetailDto)
        {
            var existingDetail = await _debtReportDetailRepository.GetByIdAsync(reportId, customerId);
            if (existingDetail == null)
            {
                throw new DebtReportDetailNotFound(reportId, customerId);
            }

            _mapper.Map(updateDebtReportDetailDto, existingDetail);
            
            existingDetail.AdditionalDebt = updateDebtReportDetailDto.FinalDebt.Value - existingDetail.InitialDebt;
            
            var updatedDetail = await _debtReportDetailRepository.UpdateAsync(reportId, customerId, existingDetail);
            await _debtReportDetailRepository.SaveChangesAsync();
            return _mapper.Map<DebtReportDetailDto>(updatedDetail);
        }

        public async Task<DebtReportDetailDto> GetDebtReportDetailById(int reportId, int customerId)
        {
            var debtReportDetail = await _debtReportDetailRepository.GetByIdAsync(reportId, customerId);
            if (debtReportDetail == null)
            {
                throw new DebtReportDetailNotFound(reportId, customerId);
            }
            return _mapper.Map<DebtReportDetailDto>(debtReportDetail);
        }

        public async Task<IEnumerable<DebtReportDetailDto>> GetAllDebtReportDetails(DebtReportDetailQuery debtReportDetailQuery)
        {
            var query = _debtReportDetailRepository.GetValuesByQuery(debtReportDetailQuery);
            if (query == null)
            {
                return Enumerable.Empty<DebtReportDetailDto>();
            }
            var debtReports = await query.ToListAsync();

            return _mapper.Map<IEnumerable<DebtReportDetailDto>>(debtReports);
        }

        public async Task<bool> DeleteDebtReportDetail(int reportId, int customerId)
        {
            var debtReportDetail = await _debtReportDetailRepository.GetByIdAsync(reportId, customerId);
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
