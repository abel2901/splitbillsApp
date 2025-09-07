namespace SplitBillsApi.Models
{
    public class Gasto
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }

        public int PagoPorId { get; set; }       // FK única para Usuario
        public Usuario PagoPor { get; set; } = null!;

        public int GrupoId { get; set; }
        public Grupo Grupo { get; set; } = null!;

        public DateTime Data { get; set; } = DateTime.UtcNow;
        public bool Pago { get; set; } = false;   

    }

}
