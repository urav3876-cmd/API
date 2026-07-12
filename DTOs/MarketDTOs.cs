using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{

    // Для создания маркета (POST /api/events/{eventId}/markets)
    public class MarketCreateDto
    {
        [Required(ErrorMessage = "Market type is required")]
        [StringLength(100, ErrorMessage = "Market type cannot exceed 100 characters")]
        public string MarketType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Market value is required")]
        [StringLength(100, ErrorMessage = "Market value cannot exceed 100 characters")]
        public string MarketValue { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }

        public List<SelectionCreateDto> Selections { get; set; } = new List<SelectionCreateDto>();
    }

    // Для создания selection (в составе маркета)
    public class SelectionCreateDto
    {
        [Required(ErrorMessage = "Selection name is required")]
        [StringLength(100, ErrorMessage = "Selection name cannot exceed 100 characters")]
        public string SelectionName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Coefficient is required")]
        public decimal Coefficient { get; set; }

        public bool IsWon { get; set; }
    }

    // Для ответа (GET /api/events/{eventId}/markets/{marketId})
    public class MarketResponseDto
    {
        public Guid MarketId { get; set; }
        public Guid EventId { get; set; }
        public string MarketType { get; set; } = string.Empty;
        public string MarketValue { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public List<SelectionResponseDto> Selections { get; set; } = new List<SelectionResponseDto>();
    }

    // Для ответа (selection в составе маркета)
    public class SelectionResponseDto
    {
        public Guid SelectionId { get; set; }
        public Guid MarketId { get; set; }
        public string SelectionName { get; set; } = string.Empty;
        public decimal Coefficient { get; set; }
        public bool IsWon { get; set; }
    }
}