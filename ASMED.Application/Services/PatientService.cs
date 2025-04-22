using ASMED.Application.Interfaces;
using ASMED.Domain.Entities;
using ASMED.Domain.Interfaces;
using ASMED.Shared.DTOs;
using ASMED.Shared.Models;
using ASMED.Shared.Requests;
using AutoMapper;
using System.Security.Claims;

namespace ASMED.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IMapper _mapper;

    public PatientService(IPatientRepository patientRepository, IDoctorRepository doctorRepository, IMapper mapper)
    {
        _patientRepository = patientRepository;
        _doctorRepository = doctorRepository;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<PatientResponse>> CreateAsync(PatientCreateRequest request, ClaimsPrincipal userClaims, CancellationToken cancellationToken)
    {
        var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return ResponseMessage<PatientResponse>.CreateInvalidResult("Usuário inválido.");

        var doctor = await _doctorRepository.GetByUserIdAsync(userId,cancellationToken);
        if (doctor == null)
            return ResponseMessage<PatientResponse>.CreateInvalidResult("Usuário não está vinculado a um médico.");

        if (await _patientRepository.ExistsByCpfAsync(request.CPF,cancellationToken))
            return ResponseMessage<PatientResponse>.CreateInvalidResult("CPF já cadastrado.");

        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName,
            BirthDate = request.BirthDate,
            CPF = request.CPF,
            Email = request.Email,
            Phone = request.Phone,
            Mobile = request.Mobile,
            Insurance = request.Insurance,
            CardNumber = request.CardNumber,
            IsActive = true
        };

        await _patientRepository.AddAsync(patient, cancellationToken);
        await _doctorRepository.AddDoctorPatientRelationAsync(doctor.Id, patient.Id,cancellationToken);

        return ResponseMessage<PatientResponse>.CreateValidResult(new PatientResponse
        {
            Id = patient.Id,
            FullName = patient.FullName,
            CPF = patient.CPF,
            Email = patient.Email,
            Mobile = patient.Mobile
        });
    }

    public async Task<ResponseMessage<List<PatientResponse>>> GetAllByLoggedDoctorAsync(ClaimsPrincipal userClaims, CancellationToken cancellationToken)
    {
        var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return ResponseMessage<List<PatientResponse>>.CreateInvalidResult("Usuário inválido.");

        var doctor = await _doctorRepository.GetByUserIdAsync(userId,cancellationToken);
        if (doctor == null)
            return ResponseMessage<List<PatientResponse>>.CreateInvalidResult("Usuário não está vinculado a um médico.");

        var patients = await _patientRepository.GetAllByDoctorIdAsync(doctor.Id, cancellationToken);

        var response = patients.Select(p => new PatientResponse
        {
            Id = p.Id,
            FullName = p.FullName,
            CPF = p.CPF,
            Email = p.Email,
            Mobile = p.Mobile
        }).ToList();

        return ResponseMessage<List<PatientResponse>>.CreateValidResult(response);
    }
    
    public async Task<ResponseMessage<PatientDto?>> GetPatientByInsuranceIdAsync(string cardNumber, CancellationToken cancellationToken)
    {
        var patient = await _patientRepository.GetByInsuranceIdAsync(cardNumber, cancellationToken);
        if (patient == null)
            return ResponseMessage<PatientDto?>.CreateInvalidResult("Paciente não encontrado.");

        var dto = _mapper.Map<PatientDto>(patient);
        return ResponseMessage<PatientDto?>.CreateValidResult(dto);
    }

}
