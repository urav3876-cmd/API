namespace API.DTOs
{
    public class EventCreateDto
    {
        public int SportId { get; set; }
        public string Team1 { get; set; } = string.Empty;
        public string Team2 { get; set; } = string.Empty;
        public string Score { get; set; } = "0:0";
        public string Status { get; set; } = "upcoming";
        public DateTime StartTime { get; set; }
    }

    public class EventResponseDto
    {
        public int EventId { get; set; }
        public int SportId { get; set; }
        public string Team1 { get; set; } = string.Empty;
        public string Team2 { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
    }
}