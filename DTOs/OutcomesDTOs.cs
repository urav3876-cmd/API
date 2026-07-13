using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    // Для создания этапа (POST /api/events/{eventId}/outcomes)
    public class OutcomeCreateDto
    {
        [Required(ErrorMessage = "Outcome name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Outcome name must be between 2 and 100 characters")]
        public string OutcomeName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Outcome score is required")]
        [StringLength(10, ErrorMessage = "Outcome score cannot exceed 10 characters")]
        public string OutcomeScore { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start time is required")]
        public DateTime StartTime { get; set; }
    }

    // Для ответа (GET /api/events/{eventId}/outcomes)
    public class OutcomeResponseDto
    {
        public int OutcomeId { get; set; }
        public int EventId { get; set; }
        public string OutcomeName { get; set; } = string.Empty;
        public string OutcomeScore { get; set; } = string.Empty;

    }
}