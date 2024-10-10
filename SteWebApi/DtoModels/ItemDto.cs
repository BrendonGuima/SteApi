using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SteWebApi.DtoModels;

public class ItemDto
{
    [Required(ErrorMessage = "O nome do item é obrigatório.")]
    [MinLength(5, ErrorMessage = "O nome do item deve ter pelo menos 5 caracteres.")]
    [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "O nome do item não pode conter caracteres especiais.")]
    [DefaultValue("Joystick xbox 360")]
    public string? Name { get; set; }
    [Required]
    [RegularExpression(@"^\d+$", ErrorMessage = "O code do item deve conter apenas números.")]
    [DefaultValue("01")]
    public string? Code { get; set; }
    
    [Required]
    [DefaultValue("Id da categoria")]
    public string? CategoryId { get; set; }
}