namespace API.DTOs
{
    // ============================================================
    // Для создания события (POST /api/events)
    // ============================================================
    public class EventCreateDto
    {
        public Guid SportId { get; set; }
        public string Team1 { get; set; } = string.Empty;
        public string Team2 { get; set; } = string.Empty;
        public string Score { get; set; } = "0:0";
        public string Status { get; set; } = "scheduled";
        public DateTime StartTime { get; set; }
    }

    // ============================================================
    // Для ответа (GET /api/events/{id})
    // ============================================================
    public class EventResponseDto
    {
        public Guid EventId { get; set; }
        public Guid SportId { get; set; }
        public string Team1 { get; set; } = string.Empty;
        public string Team2 { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    // ============================================================
    // Для обновления (PUT /api/events/{id})
    // ============================================================
    public class EventUpdateDto
    {
        public Guid SportId { get; set; }
        public string Team1 { get; set; } = string.Empty;
        public string Team2 { get; set; } = string.Empty;
        public string Score { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
    }

    // ============================================================
    // Для обновления статуса (PATCH /api/events/{id}/status)
    // ============================================================
    public class EventStatusUpdateDto
    {
        public string Status { get; set; } = string.Empty;
    }

    // ============================================================
    // Для обновления счета (PATCH /api/events/{id}/score)
    // ============================================================
    public class EventScoreUpdateDto
    {
        public string Score { get; set; } = string.Empty;
    }
}