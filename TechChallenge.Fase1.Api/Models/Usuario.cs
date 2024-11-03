
using System.Text.Json.Serialization;
using TechChallenge.Fase1.Api.Enums;

namespace TechChallenge.Fase1.Api.Models;

public static class ListaUsuario
{
    public static IList<Usuario> Usuarios { get; set; }
}

public class Usuario
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    [JsonIgnore]
    public TipoPermissaoSistema PermissaoSistema { get; set; }
}
