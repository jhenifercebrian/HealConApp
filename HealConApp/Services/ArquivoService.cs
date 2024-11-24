using HealConApp.Models;
using System.Net.Http.Json;

namespace HealConApp.Services
{
    public class ArquivoService
    {
        private readonly HttpClient _httpClient;

        public ArquivoService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(AppSettings.ApiBaseUrl)
            };
        }

        // Obter arquivos por ConsultaId
        public async Task<List<ConsultaArquivo>> ObterArquivosAsync(int consultaId)
        {
            var response = await _httpClient.GetAsync($"api/arquivo/{consultaId}/listar");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ConsultaArquivo>>();
            }

            return new List<ConsultaArquivo>();
        }

        // Adicionar um arquivo
        public async Task<bool> AdicionarArquivoAsync(ConsultaArquivo arquivo)
        {
            var response = await _httpClient.PostAsJsonAsync("api/arquivo/cadastrar", arquivo);
            return response.IsSuccessStatusCode;
        }
    }
}
