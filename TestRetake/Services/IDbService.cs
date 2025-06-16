using TestRetake.DTOs;

namespace TestRetake.Services;

public interface IDbService
{
    Task<CharacterDto> GetCharacterById(int characterId);
    Task AddItemsToBackpack(int characterId, List<int> itemIds);
}