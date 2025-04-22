namespace ASMED.Shared.Requests
{
    public class SurgeryCreateRequest
    {
        public Guid PatientId { get; set; }
        public string HospitalName { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
        public DateTime ScheduledDateTime { get; set; }
        public TimeSpan Duration { get; set; }

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