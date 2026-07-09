using API.DTOs;

namespace API.Interfaces
{
    public interface IPlayerService
    {
        Task<PlayerResponseDto> CreatePlayerAsync(PlayerCreateDto dto);
        Task<PlayerResponseDto?> GetPlayerAsync(int id);
        Task<PlayerResponseDto> UpdatePlayerAsync(int id, PlayerUpdateDto dto);
        Task<bool> PlayerExistsAsync(int id);
    }

    public interface IBetService
    {
        Task<BetResponseDto> CreateBetAsync(BetCreateDto dto);
        Task<BetResponseDto?> GetBetAsync(int id);
    }
}
