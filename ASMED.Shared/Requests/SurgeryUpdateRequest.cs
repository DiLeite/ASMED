using ASMED.Domain.Enums;

namespace ASMED.Shared.Requests;

public class SurgeryUpdateRequest
{
    public DateTime ScheduledDateTime { get; set; }
    public TimeSpan Duration { get; set; }
    public string HospitalName { get; set; }
    public string Room { get; set; }
    public ESurgeryStatus Status { get; set; }
    public string Lateralidade { get; set; }
    public string Cid { get; set; }
    public List<string> ProcedureCodes { get; set; }
    public bool HasOpme { get; set; }
    public string OpmeDescription { get; set; }
    public string OpmeProvider { get; set; }
    public decimal? MedicalFee { get; set; }
    public decimal? EstimatedCost { get; set; }
    public string Notes { get; set; }
}
