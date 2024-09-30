using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SteWebApi.Model;

public class LendingManagerDto
{
    [Required(ErrorMessage = "O nome do estudante é obrigatório.")]
    [MinLength(4, ErrorMessage = "O nome do estudante deve ter pelo menos 4 caracteres.")]
    [RegularExpression(@"^[a-zA-Z0-9 ]+$", ErrorMessage = "O nome do estudante não pode conter caracteres especiais.")]
    [DefaultValue("JoaoSilva")]
    public string? StudentName { get; set; }
    [Required(ErrorMessage = "O codigo de pessoa do estudante é obrigatório.")]
    [MinLength(5, ErrorMessage = "O codigo de pessoa do estudante deve ter pelo menos 5 caracteres.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "O codigo de pessoa do estudante deve conter apenas números.")]
    [DefaultValue("12345")]
    public string? StudentId { get; set; }

}