using ASMED.Domain.Enums;

namespace ASMED.Domain.Entities;

public class Surgery
{
    public Guid Id { get; set; }

    public Guid PatientId { get; set; }
    public Patient Patient { get; set; }

    public Guid DoctorId { get; set; } // Cirurgião principal
    public Doctor Doctor { get; set; }

    public string HospitalName { get; set; }
    public string Room { get; set; }

    public DateTime ScheduledDateTime { get; set; }
    public TimeSpan Duration { get; set; }

    public ESurgeryStatus Status { get; set; } // Agendada, Cancelada, Realizada, etc

    public string Lateralidade { get; set; } // Direito, Esquerdo, Bilateral

    public string Cid { get; set; }
    public List<string> ProcedureCodes { get; set; } // Lista de códigos TUSS

    public bool HasOpme { get; set; }
    public string OpmeDescription { get; set; }
    public string OpmeProvider { get; set; }

    public decimal? MedicalFee { get; set; } // Honorários
    public decimal? EstimatedCost { get; set; }

    public string Notes { get; set; } // Observações livres

    public bool IsActive { get; set; } = true;

    public ICollection<SurgeryDocument> Documents { get; set; }
}