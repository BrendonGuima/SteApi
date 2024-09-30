using System.ComponentModel.DataAnnotations;

namespace SteWebApi.DtoModels;

public class UserDto
{
    [Required(ErrorMessage = "O nome do usuario é obrigatório.")]
    [MinLength(5, ErrorMessage = "O nome do usuario deve ter pelo menos 5 caracteres.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Uma senha é obrigatória.")]
    [MinLength(8, ErrorMessage = "A senha deve conter pelo menos 8 caracteres.")]
    public string Password { get; set; }
}