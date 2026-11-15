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


        //Datos de prueba iniciales
        // USUARIOS
        modelBuilder.Entity<Users>().HasData(
            new Users
            {
                Id = 1,
                Email = "juan@gmail.com",
                PasswordHash = "HASH_PRUEBA_123",
                Role = "User",
                Theme = "light",
                CreatedAt = new DateTime(2026, 1, 10)
            },
            new Users
            {
                Id = 2,
                Email = "maria@gmail.com",
                PasswordHash = "HASH_PRUEBA_456",
                Role = "User",
                Theme = "dark",
                CreatedAt = new DateTime(2026, 2, 15)
            },
            new Users
            {
                Id = 3,
                Email = "admin@gmail.com",
                PasswordHash = "HASH_PRUEBA_789",
                Role = "Admin",
                Theme = "light",
                CreatedAt = new DateTime(2026, 3, 1)
            }
        );

        // PROYECTOS
        modelBuilder.Entity<Projects>().HasData(
            new Projects
            {
                Id = 1,
                UserId = 1,
                Name = "Desarrollo de aplicación web",
                Description = "Desarrollo de una plataforma web para gestionar tareas y proyectos.",
                Status = "En progreso",
                StartDate = new DateOnly(2026, 1, 15),
                EndDate = new DateOnly(2026, 12, 20),
                AccentColor = "blue",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 1, 15)
            },
            new Projects
            {
                Id = 2,
                UserId = 1,
                Name = "Reforma de oficina",
                Description = "Planificación y ejecución de reformas en la oficina principal.",
                Status = "Pendiente",
                StartDate = new DateOnly(2026, 11, 20),
                EndDate = new DateOnly(2027, 1, 15),
                AccentColor = "orange",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 10, 1)
            },
            new Projects
            {
                Id = 3,
                UserId = 2,
                Name = "Campaña de marketing",
                Description = "Diseño y ejecución de una campaña de marketing digital.",
                Status = "Completado",
                StartDate = new DateOnly(2026, 3, 1),
                EndDate = new DateOnly(2026, 6, 30),
                AccentColor = "green",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 3, 1)
            },
            new Projects
            {
                Id = 4,
                UserId = 2,
                Name = "Organización de eventos",
                Description = "Organización de eventos corporativos durante el año.",
                Status = "En progreso",
                StartDate = new DateOnly(2026, 8, 1),
                EndDate = new DateOnly(2026, 12, 15),
                AccentColor = "purple",
                IsDeleted = false,
                CreatedAt = new DateTime(2026, 8, 1)
            }
        );

        // TAREAS
        modelBuilder.Entity<Tasks>().HasData(
            new Tasks
            {
                Id = 1,
                UserId = 1,
                ProjectId = 1,
                Title = "Diseñar interfaz principal",
                Description = "Crear los diseños de las pantallas principales de la aplicación.",
                Priority = "high",
                DueDate = new DateTime(2026, 11, 20),
                CompletedAt = null,
                IsDeleted = false,
                DueEmailSent = false,
                AccentColor = "blue",
                CreatedAt = new DateTime(2026, 10, 15),
                ZoneLatitude = -34.9011,
                ZoneLongitude = -56.1645,
                ZoneRadiusMeters = 100,
                ZoneAddress = "Montevideo, Uruguay",
                AttachmentKey = null,
                AttachmentName = null,
                AttachmentType = null,
                AttachmentUrl = null
            },
            new Tasks
            {
                Id = 2,
                UserId = 1,
                ProjectId = 1,
                Title = "Implementar autenticación",
                Description = "Implementar registro, inicio de sesión y cierre de sesión.",
                Priority = "high",
                DueDate = new DateTime(2026, 11, 25),
                CompletedAt = null,
                IsDeleted = false,
                DueEmailSent = false,
                AccentColor = "red",
                CreatedAt = new DateTime(2026, 10, 18)
            },
            new Tasks
            {
                Id = 3,
                UserId = 1,
                ProjectId = 1,
                Title = "Documentar API",
                Description = "Documentar todos los endpoints de la API utilizando Swagger.",
                Priority = "low",
                DueDate = new DateTime(2026, 12, 1),
                CompletedAt = null,
                IsDeleted = false,
                DueEmailSent = false,
                AccentColor = "green",
                CreatedAt = new DateTime(2026, 10, 20)
            },
            new Tasks
            {
                Id = 4,
                UserId = 1,
                ProjectId = null,
                Title = "Comprar materiales",
                Description = "Comprar materiales necesarios para el trabajo.",
                Priority = "high",
                DueDate = new DateTime(2026, 11, 18),
                CompletedAt = null,
                IsDeleted = false,
                DueEmailSent = false,
                AccentColor = "orange",
                CreatedAt = new DateTime(2026, 11, 10)
            },
            new Tasks
            {
                Id = 5,
                UserId = 2,
                ProjectId = 3,
                Title = "Diseñar publicaciones",
                Description = "Crear las publicaciones para redes sociales.",
                Priority = "low",
                DueDate = new DateTime(2026, 5, 15),
                CompletedAt = new DateTime(2026, 5, 14),
                IsDeleted = false,
                DueEmailSent = true,
                AccentColor = "purple",
                CreatedAt = new DateTime(2026, 4, 1)
            },
            new Tasks
            {
                Id = 6,
                UserId = 2,
                ProjectId = 4,
                Title = "Reservar salón",
                Description = "Contactar proveedores y reservar el salón para el evento.",
                Priority = "high",
                DueDate = new DateTime(2026, 12, 5),
                CompletedAt = null,
                IsDeleted = false,
                DueEmailSent = false,
                AccentColor = "yellow",
                CreatedAt = new DateTime(2026, 10, 5),
                ZoneLatitude = -34.9055,
                ZoneLongitude = -56.1910,
                ZoneRadiusMeters = 200,
                ZoneAddress = "Centro, Montevideo, Uruguay"
            },
            new Tasks
            {
                Id = 7,
                UserId = 2,
                ProjectId = null,
                Title = "Enviar informe mensual",
                Description = "Preparar y enviar el informe mensual de actividades.",
                Priority = "low",
                DueDate = new DateTime(2026, 11, 10),
                CompletedAt = new DateTime(2026, 11, 9),
                IsDeleted = false,
                DueEmailSent = true,
                AccentColor = "blue",
                CreatedAt = new DateTime(2026, 11, 1)
            },
            new Tasks
            {
                Id = 8,
                UserId = 1,
                ProjectId = 2,
                Title = "Revisar presupuesto",
                Description = "Analizar los costos estimados de la reforma.",
                Priority = "high",
                DueDate = new DateTime(2026, 11, 22),
                CompletedAt = null,
                IsDeleted = true,
                DueEmailSent = false,
                AccentColor = "orange",
                CreatedAt = new DateTime(2026, 11, 5)
            }
        );

        // SUBTAREAS
        modelBuilder.Entity<Subtasks>().HasData(
            new Subtasks
            {
                Id = 1,
                TaskId = 1,
                Description = "Diseñar pantalla de inicio",
                IsCompleted = true
            },
            new Subtasks
            {
                Id = 2,
                TaskId = 1,
                Description = "Diseñar pantalla de proyectos",
                IsCompleted = false
            },
            new Subtasks
            {
                Id = 3,
                TaskId = 1,
                Description = "Diseñar pantalla de tareas",
                IsCompleted = false
            },
            new Subtasks
            {
                Id = 4,
                TaskId = 2,
                Description = "Crear endpoint de registro",
                IsCompleted = true
            },
            new Subtasks
            {
                Id = 5,
                TaskId = 2,
                Description = "Crear endpoint de login",
                IsCompleted = false
            },
            new Subtasks
            {
                Id = 6,
                TaskId = 6,
                Description = "Contactar salones disponibles",
                IsCompleted = true
            },
            new Subtasks
            {
                Id = 7,
                TaskId = 6,
                Description = "Comparar presupuestos",
                IsCompleted = false
            }
        );
    }
}
