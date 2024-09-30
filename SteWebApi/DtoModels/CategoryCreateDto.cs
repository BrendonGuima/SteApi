using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SteWebApi.DtoModels;

public class CategoryCreateDto
{
    [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
    [MinLength(5, ErrorMessage = "O nome da categoria deve ter pelo menos 5 caracteres.")]
    [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "O nome da categoria não pode conter caracteres especiais.")]
    [DefaultValue("WebCams")]
    public string Name { get; set; }
}