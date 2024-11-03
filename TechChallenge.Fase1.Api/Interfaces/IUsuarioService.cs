using TechChallenge.Fase1.Api.Models;

namespace TechChallenge.Fase1.Api.Interfaces;

public interface IUsuarioService
{
    public IEnumerable<Usuario> ListarUsuarios();
    public Usuario Usuario(int id);
    public Usuario Usuario(string username, string password);
    public Usuario CriarUsuario(Usuario dadosUsuario);
    public void AtualizarUsuario(Usuario dadosUsuario);
    public void DeletarUsuario(int id);
}
