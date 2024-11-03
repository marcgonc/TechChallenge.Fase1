using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechChallenge.Fase1.Api.Enums;
using TechChallenge.Fase1.Api.Interfaces;
using TechChallenge.Fase1.Api.Models;

namespace TechChallenge.Fase1.Api.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IUsuarioService _usuarioCadastro;

    public TokenService(IConfiguration configuration, IUsuarioService usuarioCadastro)
    {
        _configuration = configuration;
        _usuarioCadastro = usuarioCadastro;
    }

    public string GetToken(Usuario usuario)
    {
        var usuarioExiste =
            ListaUsuario.Usuarios.Any(usu => usu.Username.Equals(usuario.Username) && usu.Password.Equals(usuario.Password));

        if (!usuarioExiste)
        {
            var userDb = _usuarioCadastro.Usuario(usuario.Username, usuario.Password);

            usuarioExiste = userDb?.Username is not null;
            usuario.PermissaoSistema = usuarioExiste? userDb.PermissaoSistema : TipoPermissaoSistema.NinguemImportante;
        }

        if (!usuarioExiste)
            return string.Empty;

        var tokenHandler = new JwtSecurityTokenHandler();

        var chaveCriptografia = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretJWT"));

        var tokenPropriedades = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.Role, (usuario.PermissaoSistema).ToString()),
            }),

            Expires = DateTime.UtcNow.AddHours(8),

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(chaveCriptografia),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenPropriedades);
        return tokenHandler.WriteToken(token);

    }

    public string GetToken(Contato contato)
    {
        throw new NotImplementedException();
    }
}
