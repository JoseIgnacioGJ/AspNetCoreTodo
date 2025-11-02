using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
namespace AspNetCoreTodo.IntegrationTests
{
    // Esta prueba realiza una solicitud anónima (sin iniciar sesión) a la ruta /todo
    // y verifica que el navegador se redirige a la página de inicio de sesión.
    public class TodoRouteShould : IClassFixture<TestFixture>
    {
        private readonly HttpClient _client;

        public TodoRouteShould(TestFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task ChallengeAnonymousUser()
        {
            // Solicita la ruta /todo sin autenticación
            var request = new HttpRequestMessage(HttpMethod.Get, "/todo");

            // Solicita la ruta /todo
            var response = await _client.SendAsync(request);

            // Se asegura que el usuario es enviado a la página de inicio de sesión
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("http://localhost:8888/Account" + "/Login?ReturnUrl=%2Ftodo",
                        response.Headers.Location.ToString());
        }
    }
}