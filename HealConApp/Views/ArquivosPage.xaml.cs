using HealConApp.Models;
using HealConApp.Services;
using System.Collections.ObjectModel;

namespace HealConApp
{
    public partial class ArquivosPage : ContentPage
    {
        private readonly ArquivoService _arquivoService;
        private readonly int _consultaId;

        public ObservableCollection<ConsultaArquivo> Arquivos { get; } = new();

        public ArquivosPage(int consultaId)
        {
            InitializeComponent();
            _consultaId = consultaId;
            _arquivoService = new ArquivoService();
            BindingContext = this;

            CarregarArquivos();
        }

        private async void CarregarArquivos()
        {
            try
            {
                var arquivos = await _arquivoService.ObterArquivosAsync(_consultaId);
                Arquivos.Clear();
                foreach (var arquivo in arquivos)
                {
                    Arquivos.Add(arquivo);
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Erro", "Não foi possível carregar os arquivos.", "OK");
            }
        }

        private async void ImportarArquivo(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Pdf,
                PickerTitle = "Selecione um arquivo"
            });

            if (result != null)
            {
                try
                {
                    var fileData = await File.ReadAllBytesAsync(result.FullPath);

                    var novoArquivo = new ConsultaArquivo
                    {
                        ConsultaId = _consultaId,
                        FileName = result.FileName,
                        FileType = result.ContentType,
                        FileData = fileData
                    };

                    if (await _arquivoService.AdicionarArquivoAsync(novoArquivo))
                    {
                        Arquivos.Add(novoArquivo);
                        await DisplayAlert("Sucesso", "Arquivo importado com sucesso!", "OK");
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert("Erro", "Erro ao importar o arquivo.", "OK");
                }
            }
        }

        private async void ExibirArquivo(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is ConsultaArquivo arquivoSelecionado)
            {
                var caminhoTemp = Path.Combine(FileSystem.CacheDirectory, arquivoSelecionado.FileName);
                await File.WriteAllBytesAsync(caminhoTemp, arquivoSelecionado.FileData);

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(caminhoTemp)
                });
            }
        }
    }
}
