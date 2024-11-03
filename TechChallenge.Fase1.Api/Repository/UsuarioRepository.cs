using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;
using TechChallenge.Fase1.Api.Interfaces;
using TechChallenge.Fase1.Api.Models;

namespace TechChallenge.Fase1.Api.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IDbConnection _connection;

    public UsuarioRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public IEnumerable<Usuario> ListarUsuarios()
    {
        return _connection.GetAll<Usuario>();
    }

    public Usuario Usuario(int id)
    {
        string query = "SELECT * FROM Usuarios WHERE Id = @Id";
        return _connection.Query<Usuario>(query, new { Id = id }).FirstOrDefault();
    }

    public Usuario Usuario(string username, string password)
    {
        string query = "SELECT * FROM Usuarios WHERE Username = @Username AND Password = @Password";
        return _connection.Query<Usuario>(query, new { Username = username, Password = password}).FirstOrDefault();
    }

    public Usuario CriarUsuario(Usuario dadosUsuario)
    {
        var id = _connection.Insert(dadosUsuario);
        dadosUsuario.Id = (int)id;
        return dadosUsuario;
    }

    public void AtualizarUsuario(Usuario dadosUsuario)
    {
        _connection.Update(dadosUsuario);
    }

    public void DeletarUsuario(int id)
    {
        var usuario = Usuario(id);
        if (usuario != null)
        {
            _connection.Delete(usuario);
        }
    }
}
