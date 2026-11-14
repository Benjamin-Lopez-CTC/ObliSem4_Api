using System;

namespace ObliSem4Api.DTO.Filters;

public class ProjectFilterDto
{
    public string? Name { get; set; }
    public string? Status { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? AccentColor { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
}