using Backend.API.DTOs;

namespace Backend.API.Services;

public interface IAssignmentService
{
    Task<List<AssignmentResponse>> GetAllAsync(Guid currentUserId, string? role);
    Task<AssignmentResult> AssignAsync(CreateAssignmentRequest request);
    Task<AssignmentResult> ReleaseAsync(Guid id, string? observations);
}