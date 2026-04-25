namespace E_Commerce.Data.Entities;

public class Addresses
{
    public Guid Id { get; set; }
    public bool IsDefault { get; set; }
    public required string AddressLineOne { get; set; }
    public string? AddressLineTwo { get; set; }
    public required string Suburb { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }

    public Guid UserId { get; set; }
    public Users User { get; set; } = null!;
}
