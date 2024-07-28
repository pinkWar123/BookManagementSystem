using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Regulation;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IRegulationService
    {
        Task<RegulationDto> CreateRegulation(CreateRegulationDto createRegulationDto);
        Task<RegulationDto> UpdateRegulation(int RegulationId, UpdateRegulationDto updateRegulationDto);
        Task<RegulationDto> GetRegulationById(int RegulationId);
        Task<bool> DeleteRegulation(int RegulationId);

    }
}
