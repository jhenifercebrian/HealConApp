using HealConApp.Models;
using HealConApp.Services;
using System.Collections.ObjectModel;

namespace HealConApp
{
    public partial class MainPage : ContentPage
    {
        private readonly ConsultaService _consultaService;

        // Lista de consultas vinculada ao XAML
        public ObservableCollection<Consulta> Consultas { get; set; } = new();

        public MainPage()
        {
            InitializeComponent();
            //Consultas = new ObservableCollection<Consulta>();
            _consultaService = new ConsultaService();
            BindingContext = this;

            // Inscrever-se para a mensagem "AtualizarConsultas"
            MessagingCenter.Subscribe<CadastroViewModel>(this, "AtualizarConsultas", async (sender) =>
            {
                await CarregarConsultas();
            });

            // Carregar consultas ao iniciar a página
            _ = CarregarConsultas();
        }

        private async Task CarregarConsultas()
        {
            try
            {
                // Obter consultas da API
                var consultas = await _consultaService.ObterConsultasAsync();
                Consultas.Clear();

                // Adicionar consultas à lista observável
                foreach (var consulta in consultas)
                {
                    Consultas.Add(consulta);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Não foi possível carregar as consultas", "OK");
            }
        }

        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            // Lógica para adicionar uma nova tarefa
             await Navigation.PushAsync(new CadastroPage());
        }

        private async void OnEditTaskClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Consulta consultaSelecionada)
            {
                // Navega para a página de cadastro passando a consulta selecionada
                await Navigation.PushAsync(new CadastroPage(consultaSelecionada));
            }
        }

        private async void OnDeleteTaskClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Consulta consultaSelecionada)
            {
                bool confirmacao = await DisplayAlert("Excluir Consulta", "Tem certeza que deseja excluir esta consulta?", "Sim", "Não");

                if (confirmacao)
                {
                    var consultaService = new ConsultaService();
                    bool sucesso = await consultaService.ExcluirConsultaAsync(consultaSelecionada.ConsultaId);

                    if (sucesso)
                    {
                        // Excluir consulta com sucesso, recarregar a lista
                        await CarregarConsultas();
                        await DisplayAlert("Sucesso", "Consulta excluída com sucesso!", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Erro ao excluir a consulta.", "OK");
                    }
                }
            }
        }

        private async void OnViewFilesClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is Consulta consultaSelecionada)
            {
                await Navigation.PushAsync(new ArquivosPage(consultaSelecionada.ConsultaId));
            }
        }

    }

}
