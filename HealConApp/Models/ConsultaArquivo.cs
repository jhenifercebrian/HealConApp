
namespace HealConApp.Models
{
    public class ConsultaArquivo
    {
        public int Id { get; set; }
        public int ConsultaId { get; set; } // Chave estrangeira para Consulta
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] FileData { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.Now;
    }

}
