namespace ASMED.Shared.Responses;

public class SurgeryDocumentResponse
{
    public Guid Id { get; set; }
    public string Type { get; set; } = default!;
    public string FileUrl { get; set; } = default!;
    public DateTime UploadedAt { get; set; }
}
