namespace ObliSem4Api.Models
{
    public class Subtasks
    {
        public int Id { get; set; }

        // Tarea
        public int TaskId { get; set; }
        public Tasks Task { get; set; }

        // Detalles
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
