namespace ASMED.Domain.Entities;

public class SurgeryDocument
{
    public Guid Id { get; set; }
    public Guid SurgeryId { get; set; }
    public Surgery Surgery { get; set; }

    public string Type { get; set; } // Termo, Guia, Imagem, etc
    public string FileUrl { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}