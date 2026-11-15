using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ObliSem4Api.Migrations
{
    /// <inheritdoc />
    public partial class DatosDePrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PasswordHash", "Role", "Theme" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "juan@gmail.com", "HASH_PRUEBA_123", "User", "light" },
                    { 2, new DateTime(2026, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "maria@gmail.com", "HASH_PRUEBA_456", "User", "dark" },
                    { 3, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", "HASH_PRUEBA_789", "Admin", "light" }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "AccentColor", "CreatedAt", "Description", "EndDate", "IsDeleted", "Name", "StartDate", "Status", "UserId" },
                values: new object[,]
                {
                    { 1, "blue", new DateTime(2026, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Desarrollo de una plataforma web para gestionar tareas y proyectos.", new DateOnly(2026, 12, 20), false, "Desarrollo de aplicación web", new DateOnly(2026, 1, 15), "En progreso", 1 },
                    { 2, "orange", new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Planificación y ejecución de reformas en la oficina principal.", new DateOnly(2027, 1, 15), false, "Reforma de oficina", new DateOnly(2026, 11, 20), "Pendiente", 1 },
                    { 3, "green", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Diseño y ejecución de una campaña de marketing digital.", new DateOnly(2026, 6, 30), false, "Campaña de marketing", new DateOnly(2026, 3, 1), "Completado", 2 },
                    { 4, "purple", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Organización de eventos corporativos durante el año.", new DateOnly(2026, 12, 15), false, "Organización de eventos", new DateOnly(2026, 8, 1), "En progreso", 2 }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AccentColor", "AttachmentKey", "AttachmentName", "AttachmentType", "AttachmentUrl", "CompletedAt", "CreatedAt", "Description", "DueDate", "DueEmailSent", "IsDeleted", "Priority", "ProjectId", "Title", "UserId", "ZoneAddress", "ZoneLatitude", "ZoneLongitude", "ZoneRadiusMeters" },
                values: new object[,]
                {
                    { 4, "orange", null, null, null, null, null, new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Comprar materiales necesarios para el trabajo.", new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, "high", null, "Comprar materiales", 1, null, null, null, null },
                    { 7, "blue", null, null, null, null, new DateTime(2026, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Preparar y enviar el informe mensual de actividades.", new DateTime(2026, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "low", null, "Enviar informe mensual", 2, null, null, null, null },
                    { 1, "blue", null, null, null, null, null, new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Crear los diseños de las pantallas principales de la aplicación.", new DateTime(2026, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, "high", 1, "Diseñar interfaz principal", 1, "Montevideo, Uruguay", -34.9011, -56.164499999999997, 100 },
                    { 2, "red", null, null, null, null, null, new DateTime(2026, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Implementar registro, inicio de sesión y cierre de sesión.", new DateTime(2026, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, "high", 1, "Implementar autenticación", 1, null, null, null, null },
                    { 3, "green", null, null, null, null, null, new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Documentar todos los endpoints de la API utilizando Swagger.", new DateTime(2026, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, "low", 1, "Documentar API", 1, null, null, null, null },
                    { 5, "purple", null, null, null, null, new DateTime(2026, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Crear las publicaciones para redes sociales.", new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), true, false, "low", 3, "Diseñar publicaciones", 2, null, null, null, null },
                    { 6, "yellow", null, null, null, null, null, new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Contactar proveedores y reservar el salón para el evento.", new DateTime(2026, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, false, "high", 4, "Reservar salón", 2, "Centro, Montevideo, Uruguay", -34.905500000000004, -56.191000000000003, 200 },
                    { 8, "orange", null, null, null, null, null, new DateTime(2026, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Analizar los costos estimados de la reforma.", new DateTime(2026, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, "high", 2, "Revisar presupuesto", 1, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Subtasks",
                columns: new[] { "Id", "Description", "IsCompleted", "TaskId" },
                values: new object[,]
                {
                    { 1, "Diseñar pantalla de inicio", true, 1 },
                    { 2, "Diseñar pantalla de proyectos", false, 1 },
                    { 3, "Diseñar pantalla de tareas", false, 1 },
                    { 4, "Crear endpoint de registro", true, 2 },
                    { 5, "Crear endpoint de login", false, 2 },
                    { 6, "Contactar salones disponibles", true, 6 },
                    { 7, "Comparar presupuestos", false, 6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Subtasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subtasks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subtasks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Subtasks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Subtasks",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Subtasks",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Subtasks",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
