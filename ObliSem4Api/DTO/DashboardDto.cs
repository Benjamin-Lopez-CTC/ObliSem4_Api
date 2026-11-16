using ObliSem4Api.DTO.Tasks;
using ObliSem4Api.DTO.Projects;
using System;
using System.ComponentModel.DataAnnotations;

namespace ObliSem4Api.DTO;

public class DashboardDto
{
    // Tareas que vencen hoy
    public List<TaskReadDto> Today { get; set; } = new();
    public int TodayCount { get; set; }

    // Tareas que vencen en fechas proximas a hoy, no completadas
    public List<TaskReadDto> Upcoming { get; set; } = new();
    public int UpcomingCount { get; set; }

    // Tareas vencidas que no fueron completadas
    public List<TaskReadDto> Overdue { get; set; } = new();
    public int OverdueCount { get; set; }

    // Estadisticas - Categoria amarilla (deseable)
    public DashboardStatsDto? Stats { get; set; } = null!;
}

public class DashboardStatsDto
{
    public int Completed { get; set; }
    public int Pending { get; set; }
    public int Overdue { get; set; }

    // Tareas que se completaron antes de que se vencieran, porcentaje
    public int OnTimePercent { get; set; }
}