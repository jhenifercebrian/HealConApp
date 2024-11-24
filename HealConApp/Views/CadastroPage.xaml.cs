using System.Windows.Input;
using HealConApp.Models;
using HealConApp.Services;

namespace HealConApp
{
    public partial class CadastroPage : ContentPage
    {
        public CadastroViewModel ViewModel { get; private set; }

        public CadastroPage(Consulta consulta = null)
        {
            InitializeComponent();
            ViewModel = new CadastroViewModel(consulta);
            BindingContext = ViewModel;
        }
    }

    public class CadastroViewModel : BindableObject
    {
        private Consulta _consulta;
        private readonly ConsultaService _consultaService;

        public Consulta Consulta
        {
            get => _consulta;
            set
            {
                _consulta = value;
                OnPropertyChanged();
            }
        }

        public ICommand SalvarCommand { get; }

        public CadastroViewModel(Consulta consulta = null)
        {
            // Se uma consulta for passada, carregue-a; caso contrário, crie uma nova.
            Consulta = consulta ?? new Consulta();
            _consultaService = new ConsultaService();
            SalvarCommand = new Command(async () => await SalvarConsulta());
        }

        private async Task SalvarConsulta()
        {
            if (string.IsNullOrWhiteSpace(Consulta.PacienteNome) || string.IsNullOrWhiteSpace(Consulta.MedicoNome))
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Preencha todos os campos obrigatórios.", "OK");
                return;
            }

            // Determinar se é um cadastro novo ou uma edição
            bool sucesso;
            if (Consulta.ConsultaId == 0)
            {
                // Salvar a consulta via API
                sucesso = await _consultaService.SalvarConsultaAsync(Consulta);
            }
            else
            {
                // Atualizar a consulta via API
                sucesso = await _consultaService.AtualizarConsultaAsync(Consulta);
            }

            
            if (sucesso)
            {
                // Enviar mensagem para atualizar a lista na MainPage
                MessagingCenter.Send(this, "AtualizarConsultas");

                await Application.Current.MainPage.DisplayAlert("Sucesso", "Consulta salva com sucesso!", "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Ocorreu um erro ao salvar a consulta.", "OK");
            }
        }
    }
}
