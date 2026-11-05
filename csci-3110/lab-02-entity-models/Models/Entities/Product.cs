using System.ComponentModel.DataAnnotations;

namespace Lab02Models.Models.Entities;

public class Product

{

    public int Id { get; set;}

    [Required, StringLength(100)]
    public string name {get; set;} = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set;}

    public int CategoryId { get; set;}

    public Category? Category { get; set;}
}