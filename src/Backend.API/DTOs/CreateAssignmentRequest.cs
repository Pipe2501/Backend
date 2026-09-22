namespace Backend.API.DTOs;

public class CreateAssignmentRequest
{
    public Guid EquipmentId { get; set; }
    public Guid UserId { get; set; }
    public string? Observations { get; set; }
}