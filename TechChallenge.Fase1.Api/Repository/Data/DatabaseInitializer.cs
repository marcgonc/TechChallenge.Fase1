using Dapper;
using System.Data;

namespace TechChallenge.Fase1.Api.Repository.Data;

public static class DatabaseInitializer
{
    public static void CriarBancoDeDados(IDbConnection connection)
    {
        var criarTabelaUsuarios = @"
            CREATE TABLE IF NOT EXISTS Usuarios (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL,
                Password TEXT NOT NULL,
                PermissaoSistema INTEGER NOT NULL
            );";

        var criarTabelaContatos = @"
            CREATE TABLE IF NOT EXISTS Contatos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Nome TEXT NOT NULL,
                DDD INTEGER NOT NULL,
                Telefone TEXT,
                Email TEXT
            );";

        connection.Execute(criarTabelaUsuarios);
        connection.Execute(criarTabelaContatos);
    }

    public static bool VerificarTabelas(IDbConnection connection)
    {
        var verificarUsuarios = connection.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Usuarios';");

        var verificarContatos = connection.ExecuteScalar<int>(
            "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Contatos';");

        return verificarUsuarios > 0 && verificarContatos > 0;
    }
}

