namespace ObliSem4Api.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public string? Theme { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Projects> Projects { get; set; } = new List<Projects>();
        public ICollection<Tasks> Tasks { get; set; } = new List<Tasks>();
    }
}
