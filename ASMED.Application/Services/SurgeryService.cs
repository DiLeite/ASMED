using ASMED.Application.Interfaces;
using ASMED.Domain.Entities;
using ASMED.Domain.Enums;
using ASMED.Domain.Interfaces;
using ASMED.Shared.Models;
using ASMED.Shared.Requests;
using ASMED.Shared.Responses;
using AutoMapper;
using System.Security.Claims;

namespace ASMED.Application.Services
{
    public class SurgeryService : ISurgeryService
    {
        private readonly ISurgeryRepository _surgeryRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public SurgeryService(
            ISurgeryRepository surgeryRepository,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository,
            IMapper mapper)
        {
            _surgeryRepository = surgeryRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<ResponseMessage<SurgeryResponse>> CreateAsync(SurgeryCreateRequest request, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return ResponseMessage<SurgeryResponse>.CreateInvalidResult("Usuário inválido.");

            var doctor = await _doctorRepository.GetByUserIdAsync(userId, cancellationToken);
            if (doctor == null)
                return ResponseMessage<SurgeryResponse>.CreateInvalidResult("Usuário não está vinculado a um médico.");

            var patient = await _patientRepository.GetByIdAsync(request.PatientId, cancellationToken);
            if (patient == null)
                return ResponseMessage<SurgeryResponse>.CreateInvalidResult("Paciente não encontrado.");

            var surgery = _mapper.Map<Surgery>(request);
            surgery.DoctorId = doctor.Id;
            surgery.IsActive = true;

            await _surgeryRepository.AddAsync(surgery, cancellationToken);

            var response = _mapper.Map<SurgeryResponse>(surgery);
            return ResponseMessage<SurgeryResponse>.CreateValidResult(response);
        }

        public async Task<ResponseMessage<SurgeryResponse>> GetByIdAsync(Guid id, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return ResponseMessage<SurgeryResponse>.CreateInvalidResult("Usuário inválido.");

            var doctor = await _doctorRepository.GetByUserIdAsync(userId, cancellationToken);
            if (doctor == null)
                return ResponseMessage<SurgeryResponse>.CreateInvalidResult("Usuário não está vinculado a um médico.");

            var surgery = await _surgeryRepository.GetByIdAsync(id, cancellationToken);
            if (surgery == null || surgery.DoctorId != doctor.Id)
                return ResponseMessage<SurgeryResponse>.CreateInvalidResult("Cirurgia não encontrada ou sem permissão.");

            return ResponseMessage<SurgeryResponse>.CreateValidResult(_mapper.Map<SurgeryResponse>(surgery));
        }

        public async Task<ResponseMessage<List<SurgeryResponse>>> GetAllByDoctorAsync(ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return ResponseMessage<List<SurgeryResponse>>.CreateInvalidResult("Usuário inválido.");

            var doctor = await _doctorRepository.GetByUserIdAsync(userId, cancellationToken);
            if (doctor == null)
                return ResponseMessage<List<SurgeryResponse>>.CreateInvalidResult("Usuário não está vinculado a um médico.");

            var surgeries = await _surgeryRepository.GetAllByDoctorIdAsync(doctor.Id, cancellationToken);
            var mapped = _mapper.Map<List<SurgeryResponse>>(surgeries);

            return ResponseMessage<List<SurgeryResponse>>.CreateValidResult(mapped);
        }

        public async Task<ResponseMessage<string>> UpdateStatusAsync(Guid id, ESurgeryStatus status, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return ResponseMessage<string>.CreateInvalidResult("Usuário inválido.");

            var doctor = await _doctorRepository.GetByUserIdAsync(userId, cancellationToken);
            if (doctor == null)
                return ResponseMessage<string>.CreateInvalidResult("Usuário não está vinculado a um médico.");

            var surgery = await _surgeryRepository.GetByIdAsync(id, cancellationToken);
            if (surgery == null || surgery.DoctorId != doctor.Id)
                return ResponseMessage<string>.CreateInvalidResult("Cirurgia não encontrada ou sem permissão.");

            surgery.Status = status;
            await _surgeryRepository.UpdateAsync(surgery, cancellationToken);

            return ResponseMessage<string>.CreateValidResult("Status atualizado com sucesso.");
        }
        
        public async Task<ResponseMessage<string>> UpdateAsync(Guid id, SurgeryUpdateRequest request, CancellationToken cancellationToken)
        {
            var surgery = await _surgeryRepository.GetByIdAsync(id, cancellationToken);
            if (surgery == null)
                return ResponseMessage<string>.CreateInvalidResult("Cirurgia não encontrada.");

            // Atualiza os campos
            surgery.ScheduledDateTime = request.ScheduledDateTime;
            surgery.Duration = request.Duration;
            surgery.HospitalName = request.HospitalName;
            surgery.Room = request.Room;
            surgery.Status = request.Status;
            surgery.Lateralidade = request.Lateralidade;
            surgery.Cid = request.Cid;
            surgery.ProcedureCodes = request.ProcedureCodes;
            surgery.HasOpme = request.HasOpme;
            surgery.OpmeDescription = request.OpmeDescription;
            surgery.OpmeProvider = request.OpmeProvider;
            surgery.MedicalFee = request.MedicalFee;
            surgery.EstimatedCost = request.EstimatedCost;
            surgery.Notes = request.Notes;

            await _surgeryRepository.UpdateAsync(surgery, cancellationToken);

            return ResponseMessage<string>.CreateValidResult("Cirurgia atualizada com sucesso.");
        }

        public async Task<ResponseMessage<string>> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var surgery = await _surgeryRepository.GetByIdAsync(id, cancellationToken);
            if (surgery == null)
                return ResponseMessage<string>.CreateInvalidResult("Cirurgia não encontrada.");

            surgery.IsActive = false;
            await _surgeryRepository.UpdateAsync(surgery, cancellationToken);

            return ResponseMessage<string>.CreateValidResult("Cirurgia excluída com sucesso.");
        }

        public async Task<ResponseMessage<string>> AddDocumentAsync(Guid surgeryId, SurgeryDocumentRequest request, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return ResponseMessage<string>.CreateInvalidResult("Usuário inválido.");

            var doctor = await _doctorRepository.GetByUserIdAsync(userId, cancellationToken);
            if (doctor == null) return ResponseMessage<string>.CreateInvalidResult("Usuário não vinculado a um médico.");

            var surgery = await _surgeryRepository.GetByIdAsync(surgeryId, cancellationToken);
            if (surgery == null || surgery.DoctorId != doctor.Id)
                return ResponseMessage<string>.CreateInvalidResult("Cirurgia não encontrada ou sem permissão.");

            var document = _mapper.Map<SurgeryDocument>(request);
            document.SurgeryId = surgeryId;

            await _surgeryRepository.AddDocumentAsync(document, cancellationToken);
            return ResponseMessage<string>.CreateValidResult("Documento adicionado com sucesso.");
        }

        public async Task<ResponseMessage<List<SurgeryDocumentResponse>>> GetDocumentsAsync(Guid surgeryId, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return ResponseMessage<List<SurgeryDocumentResponse>>.CreateInvalidResult("Usuário inválido.");

            var doctor = await _doctorRepository.GetByUserIdAsync(userId, cancellationToken);
            if (doctor == null) return ResponseMessage<List<SurgeryDocumentResponse>>.CreateInvalidResult("Usuário não vinculado a um médico.");

            var surgery = await _surgeryRepository.GetByIdAsync(surgeryId, cancellationToken);
            if (surgery == null || surgery.DoctorId != doctor.Id)
                return ResponseMessage<List<SurgeryDocumentResponse>>.CreateInvalidResult("Cirurgia não encontrada ou sem permissão.");

            var documents = await _surgeryRepository.GetDocumentsAsync(surgeryId, cancellationToken);
            var response = _mapper.Map<List<SurgeryDocumentResponse>>(documents);

            return ResponseMessage<List<SurgeryDocumentResponse>>.CreateValidResult(response);
        }
    }
}