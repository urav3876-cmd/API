using Microsoft.EntityFrameworkCore;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces; 

namespace API.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly BettingDbContext _context;

        public PlayerService(BettingDbContext context)
        {
            _context = context;
        }

        public async Task<PlayerResponseDto> CreatePlayerAsync(PlayerCreateDto dto)
        {
            //существует ли игрок с таким логином
            var exists = await _context.Players
                .AnyAsync(p => p.Login == dto.Login);

            if (exists)
                throw new InvalidOperationException($"Player with login '{dto.Login}' already exists");

            //сущность Player из DTO
            var player = new Player
            {
                Login = dto.Login,
                PasswordHash = dto.PasswordHash,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PassportNumber = dto.PassportNumber,
                PassportSeries = dto.PassportSeries
            };

            //Сохранение в базу данных
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            //DTO (без пароля)
            return MapToResponse(player);
        }

        public async Task<PlayerResponseDto?> GetPlayerAsync(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
                return null;

            return MapToResponse(player);
        }

        public async Task<PlayerResponseDto> UpdatePlayerAsync(int id, PlayerUpdateDto dto)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
                throw new KeyNotFoundException($"Player with ID {id} not found");

            // логин (с проверкой уникальности)
            if (!string.IsNullOrEmpty(dto.Login))
            {
                var loginExists = await _context.Players
                    .AnyAsync(p => p.Login == dto.Login && p.PlayerId != id);

                if (loginExists)
                    throw new InvalidOperationException($"Login '{dto.Login}' is already taken");

                player.Login = dto.Login;
            }

            //  остальные поля (только если они переданы)
            if (!string.IsNullOrEmpty(dto.PasswordHash))
                player.PasswordHash = dto.PasswordHash;

            if (!string.IsNullOrEmpty(dto.FirstName))
                player.FirstName = dto.FirstName;

            if (!string.IsNullOrEmpty(dto.LastName))
                player.LastName = dto.LastName;

            if (!string.IsNullOrEmpty(dto.PassportNumber))
                player.PassportNumber = dto.PassportNumber;

            if (!string.IsNullOrEmpty(dto.PassportSeries))
                player.PassportSeries = dto.PassportSeries;

            if (dto.IsActive.HasValue)
                player.IsActive = dto.IsActive.Value;

            await _context.SaveChangesAsync();

            return MapToResponse(player);
        }

        public async Task<bool> PlayerExistsAsync(int id)
        {
            return await _context.Players.AnyAsync(p => p.PlayerId == id);
        }

        private static PlayerResponseDto MapToResponse(Player player)
        {
            return new PlayerResponseDto
            {
                PlayerId = player.PlayerId,
                Login = player.Login,
                FirstName = player.FirstName,
                LastName = player.LastName,
                PassportNumber = player.PassportNumber,
                PassportSeries = player.PassportSeries,
                Balance = player.Balance,
                IsActive = player.IsActive
            };
        }
    }
}