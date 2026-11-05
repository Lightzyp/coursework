using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Lab02Models.Models.Entities;
public class Building
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int BuildingNumber { get; set; }
    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;
}
