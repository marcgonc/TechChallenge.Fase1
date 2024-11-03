using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
using TechChallenge.Fase1.Api.Interfaces;
using TechChallenge.Fase1.Api.Models;

namespace TechChallenge.Fase1.Api.Repository
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly IDbConnection _connection;

        public ContatoRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Contato> ListarContatos()
        {
            return _connection.GetAll<Contato>();
        }

        public Contato Contato(int id)
        {
            string query = "SELECT * FROM Contatos WHERE Id = @Id";
            return _connection.Query<Contato>(query, new { Id = id }).FirstOrDefault();
        }
        public List<Contato> ObterContatoDdd(int ddd)
        {
            string query = "SELECT * FROM Contatos WHERE Ddd = @Ddd";
            return _connection.Query<Contato>(query, new { Ddd = ddd }).ToList();
        }

        public Contato CriarContato(Contato dadosContato)
        {
            var id = _connection.Insert(dadosContato);
            dadosContato.Id = (int)id;
            return dadosContato;
        }

        public void AtualizarContato(Contato dadosContato)
        {
            _connection.Update(dadosContato);
        }

        public void DeletarContato(int id)
        {
            var contato = Contato(id);
            if (contato != null)
            {
                _connection.Delete(contato);
            }
        }        
    }
}
