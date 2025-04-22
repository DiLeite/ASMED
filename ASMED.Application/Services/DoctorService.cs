using ASMED.Application.Interfaces;
using ASMED.Domain.Entities;
using ASMED.Domain.Interfaces;
using ASMED.Shared.DTOs;
using ASMED.Shared.Models;
using ASMED.Shared.Requests;
using AutoMapper;

namespace ASMED.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IMapper _mapper;

    public DoctorService(IDoctorRepository doctorRepository, IMapper mapper)
    {
        _doctorRepository = doctorRepository;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<DoctorDto?>> GetDoctorByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var doctor = await _doctorRepository.GetByIdAsync(id, cancellationToken);
        if (doctor == null)
            return ResponseMessage<DoctorDto>.CreateInvalidResult("Médico não encontrado.");
        
        var dto = _mapper.Map<DoctorDto>(doctor);
        return ResponseMessage<DoctorDto?>.CreateValidResult(dto);
    }

    public async Task<ResponseMessage<DoctorDto?>> GetDoctorByUserIdAsync(string userId, CancellationToken cancellationToken)
    {
        var doctor = await _doctorRepository.GetByUserIdAsync(userId,cancellationToken);
        if (doctor == null)
            return ResponseMessage<DoctorDto?>.CreateInvalidResult("Usuário não está vinculado a um médico.");

        var dto = _mapper.Map<DoctorDto>(doctor);
        return ResponseMessage<DoctorDto?>.CreateValidResult(dto);
    }

    public async Task<ResponseMessage<DoctorDto?>> UpdateDoctorByIdAsync(Guid doctorId, DoctorUpdateRequest request, CancellationToken cancellationToken)
    {
        var doctor = await _doctorRepository.GetByIdAsync(doctorId, cancellationToken);
        if (doctor == null)
            return ResponseMessage<DoctorDto>.CreateInvalidResult("Médico não encontrado.");

        doctor.FullName = request.FullName;
        doctor.CRM = request.CRM;
        doctor.Specialty = request.Specialty;
        doctor.CPF = request.CPF;
        doctor.Phone = request.Phone;
        doctor.Mobile = request.Mobile;

        if (doctor.Address == null)
            doctor.Address = new Address();

        doctor.Address.Street = request.Address.Street;
        doctor.Address.Number = request.Address.Number;
        doctor.Address.Neighborhood = request.Address.Neighborhood;
        doctor.Address.City = request.Address.City;
        doctor.Address.State = request.Address.State;
        doctor.Address.PostalCode = request.Address.PostalCode;
        doctor.Address.Complement = request.Address.Complement;

        await _doctorRepository.UpdateAsync(doctor, cancellationToken);

        return ResponseMessage<DoctorDto>.CreateValidResult("Médico atualizado com sucesso.");
    }
}
