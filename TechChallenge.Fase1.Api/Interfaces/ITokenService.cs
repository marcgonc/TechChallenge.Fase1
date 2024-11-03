
using TechChallenge.Fase1.Api.Models;

namespace TechChallenge.Fase1.Api.Interfaces;

public interface ITokenService
{
    public string GetToken(Usuario usuario);
}
