using System;

namespace ObliSem4Api.DTO.Projects;

public class ProjectReadDTO
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? AccentColor { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TasksCount { get; set; } = 0;
    public int CompleteTasks {get; set;} = 0;
    public int ProgressPercent => TasksCount == 0 ? 0 : CompleteTasks * 100 / TasksCount;
}
