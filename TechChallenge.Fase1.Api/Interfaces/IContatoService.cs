using Microsoft.AspNetCore.Mvc;
using TechChallenge.Fase1.Api.Models;

namespace TechChallenge.Fase1.Api.Interfaces;

public interface IContatoService
{
    public IEnumerable<Contato> ListarContatos();
    public Contato Contato(int id);
    public List<Contato> ObterContatoDdd(int ddd);
    public Contato CriarContato(Contato dadosContato);
    public void AtualizarContato(Contato dadosContato);
    public void DeletarContato(int Id);
}

