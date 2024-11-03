using TechChallenge.Fase1.Api.Models;

namespace TechChallenge.Fase1.Api.Interfaces;

public interface IUsuarioRepository
{
    IEnumerable<Usuario> ListarUsuarios();
    Usuario Usuario(int id);
    Usuario Usuario(string username, string password);
    Usuario CriarUsuario(Usuario dadosUsuario);
    void AtualizarUsuario(Usuario dadosUsuario);
    void DeletarUsuario(int id);
}
