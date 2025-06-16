using Microsoft.EntityFrameworkCore;
using TestRetake.Data;
using TestRetake.DTOs;
using TestRetake.Exceptions;
using TestRetake.Models;

namespace TestRetake.Services;

public class DbService : IDbService
{
    
    private readonly DatabaseContext _context;

    public DbService(DatabaseContext context)
    {
        _context = context;
    }
    public async Task<CharacterDto> GetCharacterById(int characterId)
    {
        var charDto = await _context.Characters.Where(c => c.CharacterId == characterId).Select(c => new CharacterDto
        {
            FirstName = c.FirstName,
            LastName = c.LastName,
            CurrentWeight = c.CurrentWeight,
            MaxWeight = c.MaxWeight,
            BackpackItems = c.Backpacks.Select(b => new ItemDto
            {
                ItemName = b.Item.Name,
                ItemWeight = b.Item.Weight,
                Amount = b.Amount,
            }).ToList(),
            Titles = c.Titles.Select(t => new TitleDto
            {
                Title = t.Title.Name,
                AquiredAt = t.AquiredAt
            }).ToList()
        }).FirstOrDefaultAsync();
        if (charDto is null)
        {
            throw new NotFoundException();
        }
        return charDto;
    }

    public async Task AddItemsToBackpack(int characterId, List<int> itemIds)
    {
        
        
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            var character = await _context.Characters
                .Include(c => c.Backpacks)
                .ThenInclude(b => b.Item)
                .FirstOrDefaultAsync(c => c.CharacterId == characterId);
            if (character is null)
                throw new NotFoundException("Character not found.");
            var items = await _context.Items
                .Where(i => itemIds.Contains(i.ItemId))
                .ToListAsync();
            if (itemIds.Count != items.Count)
            {
                throw new NotFoundException("Some items not found.");
            }
            
            int AddedWeight = items.Sum(i => i.Weight);
            if (AddedWeight + character.CurrentWeight > character.MaxWeight)
            {
                throw new ConflictException("Weight cannot exceed.");
            }

            foreach (var item in items)
            {
                var backpack = await _context.Backpacks.FirstOrDefaultAsync(b => b.ItemId == item.ItemId);
                if (backpack is not null)
                {
                    backpack.Amount += 1;
                }
                else
                {
                    _context.Backpacks.Add(new Backpack()
                    {
                        CharacterId = characterId,
                        ItemId = item.ItemId,
                        Amount = 1
                    });
                }
                character.CurrentWeight += item.Weight;
                
            }
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}