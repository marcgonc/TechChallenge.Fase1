using TechChallenge.Fase1.Api.Interfaces;
using TechChallenge.Fase1.Api.Models;

namespace TechChallenge.Fase1.Api.Service
{
    public class ContatoService : IContatoService
    {
        private readonly IContatoRepository _contatoRepository;

        public ContatoService(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }

        public IEnumerable<Contato> ListarContatos()
        {
            return _contatoRepository.ListarContatos();
        }

        public Contato Contato(int id)
        {
            return _contatoRepository.Contato(id);
        }

        public List<Contato> ObterContatoDdd(int ddd)
        {
            return _contatoRepository.ObterContatoDdd(ddd);
        }

        public Contato CriarContato(Contato dadosContato)
        {
            return _contatoRepository.CriarContato(dadosContato);
        }

        public void AtualizarContato(Contato dadosContato)
        {
            _contatoRepository.AtualizarContato(dadosContato);
        }

        public void DeletarContato(int id)
        {
            _contatoRepository.DeletarContato(id);
        }
    }
}
