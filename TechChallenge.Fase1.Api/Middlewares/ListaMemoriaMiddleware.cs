using TechChallenge.Fase1.Api.Enums;
using TechChallenge.Fase1.Api.Models;

namespace TechChallenge.Fase1.Api.Middlewares;

public class ListaMemoriaMiddleware
{
    private readonly RequestDelegate _next;

    public ListaMemoriaMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext httpContext)
    {
        ListaUsuario.Usuarios = new List<Usuario>();
        ListaUsuario.Usuarios.Add(new Usuario { Id = 1, Username = "teste", Password = "123", PermissaoSistema = TipoPermissaoSistema.Administrador });
        ListaUsuario.Usuarios.Add(new Usuario { Id = 2, Username = "marcioteste", Password = "123", PermissaoSistema = TipoPermissaoSistema.Administrador });
        ListaUsuario.Usuarios.Add(new Usuario { Id = 2, Username = "thiagopecador", Password = "123", PermissaoSistema = TipoPermissaoSistema.Administrador });

        return _next(httpContext);
    }
}

public static class ListaMemoriaMiddlewareExtensions
{
    public static IApplicationBuilder UseListaMemoriaMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ListaMemoriaMiddleware>();
    }
}
