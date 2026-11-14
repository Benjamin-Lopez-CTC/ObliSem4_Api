using System;
using ObliSem4Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ObliSem4Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Users> Users { get; set; }
    public DbSet<Projects> Projects { get; set; }
    public DbSet<Tasks> Tasks { get; set; }
    public DbSet<Subtasks> Subtasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relacion de Usuarios y Proyectos (1 a muchos)
        modelBuilder.Entity<Users>()
            .HasMany(u => u.Projects)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacion de Usuarios y Tareas (1 a muchos)
        modelBuilder.Entity<Users>()
            .HasMany(u => u.Tasks)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacion de Proyectos y Tareas (1 a muchos)
        modelBuilder.Entity<Projects>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.SetNull);

        // Relacion de Tareas y Subtareas (1 a muchos)
        modelBuilder.Entity<Tasks>()
            .HasMany(t => t.Subtasks)
            .WithOne(s => s.Task)
            .HasForeignKey(s => s.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indices unicos
        modelBuilder.Entity<Users>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Filtros globales para soft delete
        modelBuilder.Entity<Tasks>().HasQueryFilter(t => !t.IsDeleted);
        modelBuilder.Entity<Projects>().HasQueryFilter(p => !p.IsDeleted);

        // Filtro global para subtareas basado en la propiedad IsDeleted de la tarea asociada
        modelBuilder.Entity<Subtasks>().HasQueryFilter(s => !s.Task.IsDeleted);
    }
}
