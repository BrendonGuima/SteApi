using System.ComponentModel.DataAnnotations;

namespace SteWebApi.Model;

public class AuthenticateDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Password { get; set; }
}