using Microsoft.EntityFrameworkCore;
using TestRetake.Models;

namespace TestRetake.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Character> Characters { get; set; } = null!;
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<Backpack> Backpacks { get; set; } = null!;
    public DbSet<Title> Titles { get; set; } = null!;
    public DbSet<CharacterTitle> CharacterTitles { get; set; } = null!;

    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Character>().HasData(new List<Character>()
        {
            new Character() {CharacterId = 1, FirstName = "John", LastName = "Yakuza", CurrentWeight = 43, MaxWeight = 200}
        });
        modelBuilder.Entity<Item>().HasData(new List<Item>()
        {
            new Item { ItemId = 1, Name = "Item1", Weight = 10 },
            new Item { ItemId = 2, Name = "Item2", Weight = 11 },
            new Item { ItemId = 3, Name = "Item3", Weight = 12 }
        });
        modelBuilder.Entity<Title>().HasData(new List<Title>()
        {
            new Title { TitleId = 1, Name = "Title1" },
            new Title { TitleId = 2, Name = "Title2" },
            new Title { TitleId = 3, Name = "Title3" }
        });
        
        modelBuilder.Entity<Backpack>().HasData( new List<Backpack>() {
            new Backpack { CharacterId = 1, ItemId = 1, Amount = 2 },
            new Backpack { CharacterId = 1, ItemId = 2, Amount = 1 },
            new Backpack { CharacterId = 1, ItemId = 3, Amount = 1 }
        });
        
        modelBuilder.Entity<CharacterTitle>().HasData( new List<CharacterTitle>() {
            new CharacterTitle { CharacterId = 1, TitleId = 1, AquiredAt = DateTime.Parse("2025-05-01") },
            new CharacterTitle { CharacterId = 1, TitleId = 2, AquiredAt = DateTime.Parse("2025-05-02")},
            new CharacterTitle { CharacterId = 1, TitleId = 3, AquiredAt = DateTime.Parse("2025-05-03") }
        });
    }
}
