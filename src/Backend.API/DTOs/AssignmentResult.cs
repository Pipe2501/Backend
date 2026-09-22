namespace Backend.API.DTOs;

public enum AssignmentActionStatus
{
    Success,
    NotFound,
    EquipmentNotFound,
    UserNotFound,
    EquipmentAlreadyAssigned,
    NotActive
}

public record AssignmentResult(AssignmentActionStatus Status, AssignmentResponse? Assignment);