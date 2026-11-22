namespace ObliSem4Api.DTO.Filters;

public class ProjectFilterDto
{
    public int? UserId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? AccentColor { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? CreatedAt { get; set; }
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; } = false;
}