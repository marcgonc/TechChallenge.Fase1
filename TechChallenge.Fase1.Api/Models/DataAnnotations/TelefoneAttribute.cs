using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TechChallenge.Fase1.Api.Models.DataAnnotations;

public class TelefoneAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("O telefone é obrigatório.");
        }

        string telefoneNumerico = Regex.Replace(value.ToString(), @"\D", "");

        if (telefoneNumerico.Length < 8 || telefoneNumerico.Length > 9)
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}
