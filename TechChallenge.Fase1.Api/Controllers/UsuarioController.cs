using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.Fase1.Api.Enums;
using TechChallenge.Fase1.Api.Interfaces;
using TechChallenge.Fase1.Api.Models;

namespace TechChallenge.Fase1.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioCadastro;

        public UsuarioController(IUsuarioService usuarioCadastro)
        {
            _usuarioCadastro = usuarioCadastro;
        }

        [HttpGet("RetornaTodosUsuarios")]
        [Authorize]
        public ActionResult<IEnumerable<Usuario>> ListarUsuarios()
        {
            return Ok(_usuarioCadastro.ListarUsuarios());
        }

        [HttpGet("RetornaUsuarioPorId")]
        [Authorize(Roles = $"{PermissaoSistema.Administrador},{PermissaoSistema.Atendimento}")]
        public ActionResult<Usuario> ObterUsuario(int id)
        {
            var usuario = _usuarioCadastro.Usuario(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost("CadastraUsuario")]
        [Authorize(Roles = $"{PermissaoSistema.Administrador},{PermissaoSistema.Atendimento}")]
        public ActionResult<Usuario> CriarUsuario(Usuario dadosUsuario)
        {
            var usuario = _usuarioCadastro.CriarUsuario(dadosUsuario);
            return CreatedAtAction(nameof(ObterUsuario), new { id = usuario.Id }, usuario);
        }

        [HttpPut("AtualizaUsuario")]
        [Authorize(Roles = $"{PermissaoSistema.Administrador},{PermissaoSistema.Atendimento}")]
        public IActionResult AtualizarUsuario(int id, Usuario dadosUsuario)
        {
            if (id != dadosUsuario.Id) return BadRequest();
            _usuarioCadastro.AtualizarUsuario(dadosUsuario);
            return NoContent();
        }

        [HttpDelete("DeleteUsuarioPorId")]
        [Authorize(Roles = PermissaoSistema.Administrador)]
        public IActionResult DeletarUsuario(int id)
        {
            _usuarioCadastro.DeletarUsuario(id);
            return NoContent();
        }
    }
}