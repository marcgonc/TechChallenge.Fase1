using TechChallenge.Fase1.Api.Interfaces;
using TechChallenge.Fase1.Api.Models;

namespace TechChallenge.Fase1.Api.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public IEnumerable<Usuario> ListarUsuarios()
        {
            return _usuarioRepository.ListarUsuarios();
        }

        public Usuario Usuario(int id)
        {
            return _usuarioRepository.Usuario(id);
        }
        public Usuario Usuario(string username, string password)
        {
            return _usuarioRepository.Usuario(username, password);
        }

        public Usuario CriarUsuario(Usuario dadosUsuario)
        {
            return _usuarioRepository.CriarUsuario(dadosUsuario);
        }

        public void AtualizarUsuario(Usuario dadosUsuario)
        {
            _usuarioRepository.AtualizarUsuario(dadosUsuario);
        }

        public void DeletarUsuario(int id)
        {
            _usuarioRepository.DeletarUsuario(id);
        }

        
    }
}
