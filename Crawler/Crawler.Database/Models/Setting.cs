using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CurrencyExchange.Crawler.Database.Entities.Models;

[Table("Settings")]
public class Setting
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Column("Type")]
    [Required]
    public SettingValueType Type  {get;set;}

    [Column("Name")]
    [Required]
    public string? Name { get; set; }

    [Column("Value")]
    [Required]
    public string? Value { get; set; }
}
