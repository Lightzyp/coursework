using System.ComponentModel.DataAnnotations;

namespace Lab02Models.Models.Entities;

public class Project

{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Assignment> Assignments { get; set; }
        = new List<Assignment>();
}

