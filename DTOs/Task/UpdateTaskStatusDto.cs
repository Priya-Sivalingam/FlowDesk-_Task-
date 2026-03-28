using FlowDesk.Api.Enums;

namespace FlowDesk.Api.DTOs.Task;

public class UpdateTaskStatusDto
{
    public BoardTaskStatus Status { get; set; }
}