using ASMED.Domain.Entities;
using ASMED.Shared.DTOs;
using ASMED.Shared.Requests;
using ASMED.Shared.Responses;
using AutoMapper;

namespace ASMED.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Entidade -> DTO
        CreateMap<Patient, PatientDto>();
        CreateMap<Address, AddressDto>();

        CreateMap<Doctor, DoctorDto>();

        // DTO -> Entidade (se necessário)
        CreateMap<PatientDto, Patient>();
        CreateMap<AddressDto, Address>();

        CreateMap<DoctorDto, Doctor>();

        CreateMap<Surgery, SurgeryDto>()
        .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName));

        CreateMap<SurgeryUpdateRequest, Surgery>();
        CreateMap<SurgeryCreateRequest, Surgery>()
                .ForMember(dest => dest.ProcedureCodes, opt => opt.MapFrom(src => src.ProcedureCodes.ToList()));
        CreateMap<Surgery, SurgeryResponse>()
            .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FullName))
            .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName));
        
        CreateMap<SurgeryDocumentRequest, SurgeryDocument>();
        CreateMap<SurgeryDocument, SurgeryDocumentResponse>();
    }
}
