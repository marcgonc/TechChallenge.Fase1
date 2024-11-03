using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SQLitePCL;
using System.Data;
using System.Text;
using TechChallenge.Fase1.Api.Interfaces;
using TechChallenge.Fase1.Api.Logging;
using TechChallenge.Fase1.Api.Middlewares;
using TechChallenge.Fase1.Api.Repository;
using TechChallenge.Fase1.Api.Repository.Data;
using TechChallenge.Fase1.Api.Service;

Batteries.Init();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IContatoService, ContatoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IContatoRepository, ContatoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<ITokenService, TokenService>();

ConfigureJWTApplication(builder);
ConfigureDatabase(builder);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

ConfigureSwagerAuth(builder);

builder.Logging.ClearProviders();
builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information,
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseListaMemoriaMiddleware();

app.UseAuthorization();

app.MapControllers();

CreateLocalDatabase(app);

app.Run();

static void ConfigureDatabase(WebApplicationBuilder builder)
{
    var dbPath = Path.Combine(AppContext.BaseDirectory, "techchallengefase1.db");
    var stringConexao = $"Data Source={dbPath}";
    builder.Services.AddScoped<IDbConnection>((conexao) => new SqliteConnection(stringConexao));
}

static void ConfigureJWTApplication(WebApplicationBuilder builder)
{
    var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

    var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretJWT"));

    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
}

static void CreateLocalDatabase(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        DatabaseInitializer.CriarBancoDeDados(connection);

        bool tabelasCriadas = DatabaseInitializer.VerificarTabelas(connection);
        if (tabelasCriadas)
        {
            Console.WriteLine("Banco de dados e tabelas foram criados com sucesso.");
        }
        else
        {
            Console.WriteLine("Erro: As tabelas não foram criadas corretamente.");
        }
    }
}

static void ConfigureSwagerAuth(WebApplicationBuilder builder)
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tech Chanllenge Fase 1", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n" +
                "Digite 'Bearer' [espaço] e então seu token no campo abaixo.\r\n\r\n" +
                "Exemplo (informar sem as aspas): 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });
}