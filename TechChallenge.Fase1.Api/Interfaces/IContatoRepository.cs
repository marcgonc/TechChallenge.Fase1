using TechChallenge.Fase1.Api.Models;

namespace TechChallenge.Fase1.Api.Interfaces;

public interface IContatoRepository
{
    IEnumerable<Contato> ListarContatos();
    Contato Contato(int id);
    List<Contato> ObterContatoDdd(int ddd);
    Contato CriarContato(Contato dadosContato);
    void AtualizarContato(Contato dadosContato);
    void DeletarContato(int id);
}
