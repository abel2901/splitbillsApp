namespace SplitBillsApi.DTO
{
    public class SaldoUsuario
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal Saldo { get; set; } // positivo = deve receber, negativo = deve pagar
    }
}
