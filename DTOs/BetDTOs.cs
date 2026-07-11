namespace API.DTOs
{ 
    public class BetSelectionDto
    {
        public long SelectionId { get; set; }
        public int MarketId { get; set; }
        public string SelectionName { get; set; } = string.Empty;
        public decimal Coefficient { get; set; }
        public bool? IsWon { get; set; }
    }

    // Для создания ставки (POST /api/bets)
    public class BetCreateDto
    {
        public int PlayerId { get; set; }
        public decimal Amount { get; set; }
        public decimal CalculatedPayout { get; set; }
        public decimal ActualPayout { get; set; }
        public List<BetSelectionDto> Selections { get; set; } = new();
    }

    // Для получения данных ставки (GET /api/bets/{id})
    public class BetResponseDto
    {
        public int BetId { get; set; }
        public int PlayerId { get; set; }
        public decimal Amount { get; set; }
        public decimal CalculatedPayout { get; set; }
        public decimal ActualPayout { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? SettledAt { get; set; }
        public List<BetSelectionDto> Selections { get; set; } = new();
    }
}