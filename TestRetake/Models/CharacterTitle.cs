
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestRetake.Models;

[Table("CharacterTitle")]
[PrimaryKey(nameof(CharacterId), nameof(TitleId))]
public class CharacterTitle
{
    public int CharacterId { get; set; }
    public int TitleId { get; set; }

    public DateTime AquiredAt { get; set; }

    [ForeignKey(nameof(CharacterId))]
    public Character Character { get; set; } = null!;

    [ForeignKey(nameof(TitleId))]
    public Title Title { get; set; } = null!;
}
