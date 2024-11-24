namespace HealConApp.Models
{
    public class Consulta
    {
        public int ConsultaId { get; set; }
        public string PacienteNome { get; set; }
        public string MedicoNome { get; set; }
        public string Especialidade { get; set; }
        public DateTime DataConsulta { get; set; } = DateTime.Now;
        public TimeSpan HorarioConsulta { get; set; }
        public string Observacoes { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.Now;
    }
}
