namespace API.DTOs
{
    public class EventCreateDto
    {
        public string SportName { get; set; } = string.Empty;
        public string Team1 { get; set; } = string.Empty;
        public string Team2 { get; set; } = string.Empty;
        public string Score { get; set; } = "0:0";
        public string Status { get; set; } = "scheduled";
        public DateTime StartTime { get; set; }
    }

    public class EventResponseDto
    {
        public Guid EventId { get; set; }
        public string SportName { get; set; } = string.Empty;
        public string Team1 { get; set; } = string.Empty;
        public string Team2 { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
    }
}