using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechChallenge.Fase1.Api.Enums;
using TechChallenge.Fase1.Api.Interfaces;
using TechChallenge.Fase1.Api.Models;

namespace TechChallenge.Fase1.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatoController : ControllerBase
    {
        private readonly IContatoService _contatoCadastro;

        public ContatoController(IContatoService contatoCadastro)
        {
            _contatoCadastro = contatoCadastro;
        }

        [HttpGet("RetornaTodosContatos")]
        [Authorize]
        public ActionResult<IEnumerable<Contato>> ListarContatos()
        {
            return Ok(_contatoCadastro.ListarContatos());
        }

        [HttpGet("RetornaContatoPorId")]
        [Authorize(Roles = $"{PermissaoSistema.Administrador},{PermissaoSistema.Atendimento}")]
        public ActionResult<Contato> ObterContato(int id)
        {
            var contato = _contatoCadastro.Contato(id);
            if (contato == null) return NotFound();
            return Ok(contato);
        }

        [HttpGet("RetornaContatoPorDdd")]
        [Authorize(Roles = $"{PermissaoSistema.Administrador},{PermissaoSistema.Atendimento}")]
        public ActionResult<Contato> ObterContatoDdd(int ddd)
        {
            var contato = _contatoCadastro.ObterContatoDdd(ddd);
            if (contato == null) return NotFound();
            return Ok(contato);
        }

        [HttpPost("CadastraContato")]
        [Authorize(Roles = $"{PermissaoSistema.Administrador},{PermissaoSistema.Atendimento}")]
        public ActionResult<Contato> CriarContato(Contato dadosContato)
        {
            var contato = _contatoCadastro.CriarContato(dadosContato);
            return CreatedAtAction(nameof(ObterContato), new { id = contato.Id }, contato);
        }

        [HttpPut("AtualizaContato")]
        [Authorize(Roles = $"{PermissaoSistema.Administrador},{PermissaoSistema.Atendimento}")]
        public IActionResult AtualizarContato(int id, Contato dadosContato)
        {
            if (id <= 0 ) return BadRequest();
            dadosContato.Id = id;
            _contatoCadastro.AtualizarContato(dadosContato);
            return NoContent();
        }

        [HttpDelete("DeleteContatoPorId")]
        [Authorize(Roles = $"{PermissaoSistema.Administrador},{PermissaoSistema.Atendimento}")]
        public IActionResult DeletarContato(int id)
        {
            _contatoCadastro.DeletarContato(id);
            return NoContent();
        }
    }
}
