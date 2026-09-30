using System.ComponentModel.DataAnnotations;

namespace Domain.Dto;

public class TeacherDto
{
    [Required]
    [StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Title { get; set; }

    [StringLength(1000)]
    public string? Bio { get; set; }
}