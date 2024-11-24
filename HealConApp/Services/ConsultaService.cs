using HealConApp.Models;
using System.Net.Http.Json;

namespace HealConApp.Services
{
    public class ConsultaService
    {
        private readonly HttpClient _httpClient;

        public ConsultaService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(AppSettings.ApiBaseUrl)
            };
        }

        // Chamada na API para salvar uma nova consulta
        public async Task<bool> SalvarConsultaAsync(Consulta consulta)
        {
            var response = await _httpClient.PostAsJsonAsync("api/consulta/cadastrar", consulta);

            return response.IsSuccessStatusCode;
        }

        // Chamada na API para buscar todas as consultas
        public async Task<List<Consulta>> ObterConsultasAsync()
        {
            var response = await _httpClient.GetAsync("api/consulta/listar");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Consulta>>();
            }

            return new List<Consulta>();
        }

        // Chamada na API para atualizar os dados de uma consulta
        public async Task<bool> AtualizarConsultaAsync(Consulta consulta)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/consulta/alterar", consulta);

            return response.IsSuccessStatusCode;
        }

        // Chamada na API para excluir os dados de uma consulta
        public async Task<bool> ExcluirConsultaAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/consulta/excluir/{id}");

            return response.IsSuccessStatusCode;
        }

    }
}
