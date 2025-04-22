namespace ASMED.Domain.Entities;

public class Address
{
    public Guid Id { get; set; }
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string? Complement { get; set; }

    public Doctor? Doctor { get; set; }
    public Patient? Patient { get; set; }
}
