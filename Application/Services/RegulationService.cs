using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.Regulation;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.Regulation;
using FluentValidation;

namespace BookManagementSystem.Application.Services
{
    public class RegulationService : IRegulationService
    {

        private readonly IRegulationRepository _RegulationRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateRegulationDto> _createValidator;
        private readonly IValidator<UpdateRegulationDto> _updateValidator;


        public RegulationService(
            IRegulationRepository RegulationRepository,
            IMapper mapper,
            IValidator<CreateRegulationDto> _createValidator,
            IValidator<UpdateRegulationDto> _updateValidator)
        {
            this._RegulationRepository = RegulationRepository;
            _mapper = mapper;
            this._createValidator = _createValidator;
            this._updateValidator = _updateValidator;
        }
        public async Task<RegulationDto> CreateRegulation(CreateRegulationDto createRegulationDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createRegulationDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var book = _mapper.Map<Regulation>(createRegulationDto);
            await _RegulationRepository.AddAsync(book);
            await _RegulationRepository.SaveChangesAsync();
            return _mapper.Map<RegulationDto>(book);
        }

        public async Task<bool> DeleteRegulation(int RegulationId)
        {
            var book = await _RegulationRepository.GetByIdAsync(RegulationId);

            if(book == null)
            {
                return false;
            }

            _RegulationRepository.Remove(book);
            await _RegulationRepository.SaveChangesAsync();
            return true;
        }

        public async Task<RegulationDto> GetRegulationById(int RegulationId)
        {
            var regulation = await _RegulationRepository.GetByIdAsync(RegulationId);

            if(regulation == null)
            {
                 throw new KeyNotFoundException($"Không tìm thấy RegulationId");
            }

            return _mapper.Map<RegulationDto>(regulation);
        }

        public async Task<RegulationDto> UpdateRegulation(int RegulationId, UpdateRegulationDto updateRegulationDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateRegulationDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var regulation = await _RegulationRepository.GetByIdAsync(RegulationId);

            if(regulation == null)
            {
                 throw new KeyNotFoundException($"Không tìm thấy RegulationId, không thể cập nhật");
            }
            _mapper.Map(updateRegulationDto, regulation);
            await _RegulationRepository.UpdateAsync(RegulationId, regulation);
            await _RegulationRepository.SaveChangesAsync();
            return _mapper.Map<RegulationDto>(regulation);

        }
    }
}