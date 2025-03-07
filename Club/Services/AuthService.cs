using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Club.Services
{
    public class AuthService
    {

        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Register(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", new { Email = email, Password = password });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Login(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { Email = email, Password = password });
            return response.IsSuccessStatusCode;
        }
    }
}
