namespace IRCM.DTOs.LeaseRequest;

public class CreateLeaseRequestDto
{
    public Guid PropertyId { get; set; }

    public required string Message { get; set; }
}