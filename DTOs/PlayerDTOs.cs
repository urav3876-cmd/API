namespace API.DTOs
{

    public class PlayerCreateDto
    {
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PassportNumber { get; set; } = string.Empty;
        public string PassportSeries { get; set; } = string.Empty;
    }

    public class PlayerUpdateDto
    {
        public string? Login { get; set; }
        public string? PasswordHash { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PassportNumber { get; set; }
        public string? PassportSeries { get; set; }
        public bool? IsActive { get; set; }
    }

    public class PlayerResponseDto
    {
        public int PlayerId { get; set; }
        public string Login { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PassportNumber { get; set; } = string.Empty;
        public string PassportSeries { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
    }
}



