namespace API.DTOs
{
    // Для ответа (GET /api/sports)
    public class SportResponseDto
    {
        public Guid SportId { get; set; }
        public string SportName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}