namespace API.DTOs
{
    // Для ответа (GET /api/sports)
    public class SportResponseDto
    {
        public int SportId { get; set; }
        public string SportName { get; set; } = string.Empty;
    }
}