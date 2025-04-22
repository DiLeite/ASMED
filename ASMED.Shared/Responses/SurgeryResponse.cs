using ASMED.Domain.Enums;

namespace ASMED.Shared.Responses
{
    public class SurgeryResponse
    {
        public Guid Id { get; set; }

        public Guid PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;

        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;

        public string HospitalName { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;

        public DateTime ScheduledDateTime { get; set; }
        public TimeSpan Duration { get; set; }

        public ESurgeryStatus Status { get; set; }

        public string Lateralidade { get; set; } = string.Empty;
        public string Cid { get; set; } = string.Empty;
        public List<string> ProcedureCodes { get; set; } = new();

        public bool HasOpme { get; set; }
        public string OpmeDescription { get; set; } = string.Empty;
        public string OpmeProvider { get; set; } = string.Empty;

        public decimal? MedicalFee { get; set; }
        public decimal? EstimatedCost { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}