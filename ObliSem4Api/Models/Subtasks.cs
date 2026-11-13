namespace ObliSem4Api.Models
{
    public class Subtasks
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
