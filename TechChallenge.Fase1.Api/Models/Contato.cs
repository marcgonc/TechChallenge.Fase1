using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TechChallenge.Fase1.Api.Models.DataAnnotations;

namespace TechChallenge.Fase1.Api.Models;

public class Contato
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; }
    
    [Range(10, 99, ErrorMessage = "O DDD deve estar entre 10 e 99.")]
    public int Ddd { get; set; }

    [Required(ErrorMessage = "O telefone é obrigatório.")]
    [Telefone(ErrorMessage = "O telefone deve conter 8 ou 9 dígitos e não incluir o DDD.")]
    public string Telefone { get; set; }

    [Required(ErrorMessage = "O email é obrigatório.")]
    [EmailAddress(ErrorMessage = "Email inválido. Verifique o formato.")]
    public string email { get; set; }
}
