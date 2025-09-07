namespace SplitBillsApi.DTO
{
    public class GastoDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public int PagoPorUsuarioId { get; set; }
        public string PagoPorNome { get; set; }
        public bool Pago { get; set; }
    }

}
