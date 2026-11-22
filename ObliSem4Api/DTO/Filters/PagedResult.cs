using System;

namespace ObliSem4Api.DTO.Filters;

public class PagedResult<T>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public IReadOnlyList<T> Items { get; init; } = [];

    public int TotalPagina => PageSize == 0 ? 0 : (int)Math.Ceiling((double)Total / PageSize);
}
