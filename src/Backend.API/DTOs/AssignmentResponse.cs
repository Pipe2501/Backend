using Backend.API.Models;

namespace Backend.API.DTOs;

public class AssignmentResponse
{
    public Guid Id { get; set; }
    public Guid EquipmentId { get; set; }
    public Guid UserId { get; set; }
    public string? EquipmentInternalCode { get; set; }
    public string? EquipmentSerialNumber { get; set; }
    public string? UserFullName { get; set; }
    public DateTime AssignedAt { get; set; }
    public DateTime? ReleasedAt { get; set; }
    public AssignmentStatus Status { get; set; }
    public string Observations { get; set; } = string.Empty;
}