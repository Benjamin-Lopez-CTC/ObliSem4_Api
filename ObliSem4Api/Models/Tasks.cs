namespace ObliSem4Api.Models
{
    public class Tasks
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool DueEmailSent { get; set; }
        public string? AccentColor { get; set; }
        public DateTime CreatedAt { get; set; }
        public double ZoneLatitude { get; set; }
        public double ZoneLongitude { get; set; }
        public int? ZoneRadiusMeters { get; set; }
        public string? ZoneAddress { get; set; }
        public string? AttachmentKey { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentType { get; set; }
        public string? AttachmentUrl { get; set; }
    }
}
