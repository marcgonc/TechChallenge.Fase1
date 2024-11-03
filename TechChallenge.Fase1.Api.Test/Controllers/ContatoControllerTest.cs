using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TechChallenge.Fase1.Api.Controllers;
using TechChallenge.Fase1.Api.Interfaces;
using TechChallenge.Fase1.Api.Models;

namespace TechChallenge.Fase1.Api.Test.Controllers
{
    public class ContatoControllerTest
    {

        #region [ Privates ]

        private readonly Mock<IContatoService> _mockContatoService;
        private readonly ContatoController _controller;

        private List<ValidationResult> ValidateModel(Contato contato)
        {
            var context = new ValidationContext(contato, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(contato, context, results, true);
            return results;
        }

        #endregion

        #region [ Construtores ]

        public ContatoControllerTest()
        {
            // Configura o mock do IContatoService
            _mockContatoService = new Mock<IContatoService>();

            // Instancia o controlador com o serviço "mockado"
            _controller = new ContatoController(_mockContatoService.Object);

            // Configura o HttpContext com autenticação de token fictício
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "User Test"),
                        new Claim(ClaimTypes.Role, "Administrador")
                    }, "Bearer"))
                }
            };
        }

        #endregion

        #region [ Testes de Validação ]

        [Fact]
        public void Contato_NomeObrigatorio_RetornaErro()
        {
            // Arrange
            var contato = new Contato
            {
                Nome = null, // Nome é obrigatório
                Ddd = 11,
                Telefone = "12345678",
                email = "teste@exemplo.com"
            };

            // Act
            var results = ValidateModel(contato);

            // Assert
            Assert.Contains(results, v => v.ErrorMessage == "O nome é obrigatório.");
        }

        [Fact]
        public void Contato_DddForaDoIntervalo_RetornaErro()
        {
            // Arrange
            var contato = new Contato
            {
                Nome = "Teste",
                Ddd = 100, // DDD fora do intervalo permitido
                Telefone = "12345678",
                email = "teste@exemplo.com"
            };

            // Act
            var results = ValidateModel(contato);

            // Assert
            Assert.Contains(results, v => v.ErrorMessage == "O DDD deve estar entre 10 e 99.");
        }

        [Fact]
        public void Contato_TelefoneFormatoInvalido_RetornaErro()
        {
            // Arrange
            var contato = new Contato
            {
                Nome = "Teste",
                Ddd = 11,
                Telefone = "1234", // Telefone fora do formato esperado
                email = "teste@exemplo.com"
            };

            // Act
            var results = ValidateModel(contato);

            // Assert
            Assert.Contains(results, v => v.ErrorMessage == "O telefone deve conter 8 ou 9 dígitos e não incluir o DDD.");
        }

        [Fact]
        public void Contato_EmailInvalido_RetornaErro()
        {
            // Arrange
            var contato = new Contato
            {
                Nome = "Teste",
                Ddd = 11,
                Telefone = "12345678",
                email = "email-invalido" // Email em formato incorreto
            };

            // Act
            var results = ValidateModel(contato);

            // Assert
            Assert.Contains(results, v => v.ErrorMessage == "Email inválido. Verifique o formato.");
        }

        [Fact]
        public void Contato_Valido_NaoRetornaErros()
        {
            // Arrange
            var contato = new Contato
            {
                Nome = "Teste Válido",
                Ddd = 21,
                Telefone = "12345678",
                email = "teste@exemplo.com"
            };

            // Act
            var results = ValidateModel(contato);

            // Assert
            Assert.Empty(results); // Não deve haver erros de validação
        }

        #endregion

        #region [ Testes de EndPoints ]

        [Fact]
        public void ListarContatos_RetornaOkResult()
        {
            // Arrange
            var contatos = new List<Contato> { new Contato { Id = 1, Nome = "Teste", Ddd = 11, email = "teste", Telefone = "51324321" } };
            _mockContatoService.Setup(service => service.ListarContatos()).Returns(contatos);

            // Act
            var result = _controller.ListarContatos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Contato>>(okResult.Value);
            Assert.NotEmpty(returnValue);
        }

        [Fact]
        public void ObterContato_QuandoIdExiste_RetornaOkResult()
        {
            // Arrange
            var contato = new Contato {
                Id = 1,
                Nome = "Teste",
                Ddd = 11,
                email = "teste",
                Telefone = "51324321" };
            _mockContatoService.Setup(service => service.Contato(1)).Returns(contato);

            // Act
            var result = _controller.ObterContato(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Contato>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public void ObterContato_QuandoIdNaoExiste_RetornaNotFoundResult()
        {
            // Arrange
            _mockContatoService.Setup(service => service.Contato(2)).Returns((Contato)null);

            // Act
            var result = _controller.ObterContato(2);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void ObterContato_PorDDD_RetornaOkResult()
        {
            // Arrange
            var contato = new Contato
            {
                Id = 1,
                Nome = "Teste",
                Ddd = 11,
                email = "teste",
                Telefone = "51324321"
            };
            _mockContatoService.Setup(service => service.ObterContatoDdd(11)).Returns([contato]);

            // Act
            var result = _controller.ObterContatoDdd(11);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(result.Result);
            List<Contato> returnValue = Assert.IsType<List<Contato>>(okResult.Value);
            Assert.NotEmpty(returnValue);

            returnValue.ForEach(item => {
                Assert.Equal(11, item.Ddd);
            });
        }

        [Fact]
        public void CriarContato_ComDadosValidos_RetornaCreatedAtActionResult()
        {
            // Arrange
            var novoContato = new Contato
            {
                Id = 1,
                Nome = "Novo Contato",
                Ddd = 21,
                Telefone = "12345678",
                email = "novocontato@exemplo.com"
            };

            // Configura o comportamento do método CriarContato no mock
            _mockContatoService.Setup(service => service.CriarContato(It.IsAny<Contato>()))
                               .Returns(novoContato);

            // Act
            var result = _controller.CriarContato(novoContato);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Contato>(createdAtActionResult.Value);
            Assert.Equal(novoContato.Id, returnValue.Id);
            Assert.Equal(novoContato.Nome, returnValue.Nome);
            Assert.Equal(novoContato.Ddd, returnValue.Ddd);
            Assert.Equal(novoContato.Telefone, returnValue.Telefone);
            Assert.Equal(novoContato.email, returnValue.email);

            // Verifica se o método CriarContato do serviço foi chamado uma vez
            _mockContatoService.Verify(service => service.CriarContato(It.IsAny<Contato>()), Times.Once);
        }

        [Fact]
        public void AtualizarContato_ComDadosValidos_RetornaNoContent()
        {
            // Arrange
            int contatoId = 1;
            Contato contatoAtualizado = new Contato
            {
                Id = contatoId,
                Nome = "Contato Atualizado",
                Ddd = 11,
                Telefone = "87654321",
                email = "atualizado@exemplo.com"
            };

            _mockContatoService.Setup(service => service.AtualizarContato(It.IsAny<Contato>()));

            // Act
            var result = _controller.AtualizarContato(contatoId, contatoAtualizado);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockContatoService.Verify(service => service.AtualizarContato(It.Is<Contato>(c => c.Id == contatoId)), Times.Once);
        }

        [Fact]
        public void DeletarContato_ComIdValido_RetornaNoContent()
        {
            // Arrange
            int contatoId = 1;

            _mockContatoService.Setup(service => service.DeletarContato(contatoId));

            // Act
            var result = _controller.DeletarContato(contatoId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockContatoService.Verify(service => service.DeletarContato(contatoId), Times.Once);
        }

        #endregion

    }
}
