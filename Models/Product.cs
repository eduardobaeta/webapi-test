using System.ComponentModel.DataAnnotations;

public class Product{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [MaxLength(60, ErrorMessage = "This field must contain between 3 and 60 characters.")]
    [MinLength(3, ErrorMessage = "This field must contain between 3 and 60 characters.")]
    public string Title { get; set; }

    [MaxLength(1024, ErrorMessage = "This field must contain a maximum of 1024 characters.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "The price must be more than zero.")]
    public double Price { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
}