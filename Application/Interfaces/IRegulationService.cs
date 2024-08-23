using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Regulation;
using BookManagementSystem.Application.Queries;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IRegulationService
    {
        Task<RegulationDto> CreateRegulation(CreateRegulationDto createRegulationDto);
        Task<RegulationDto> UpdateRegulation(int RegulationId, UpdateRegulationDto updateRegulationDto);
        Task<RegulationDto> GetRegulationById(int RegulationId);
        Task<bool> DeleteRegulation(int RegulationId);
        Task<IEnumerable<RegulationDto>> GetAllRegulations(RegulationQuery regulationQuery);
        Task<RegulationDto?> GetMinimumBookEntry();
        Task<RegulationDto?> GetMaximumInventory();
        Task<RegulationDto?> GetMinimumInventoryAfterSelling();
        Task<RegulationDto?> GetMaximumCustomerDebt();
        Task<RegulationDto?> GetPaymentNotExceedDebt();


    }
}
