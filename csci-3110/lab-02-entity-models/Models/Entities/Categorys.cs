using System.ComponentModel.DataAnnotations;

namespace Lab02Models.Models.Entities;

public class Category

{
    public int Id {get; set;}
    
    [Required, StringLength(100)]
    public string name {get; set;} = string.Empty;

    public ICollection<Product> Product {get; set;}
    = new List<Product>();
}