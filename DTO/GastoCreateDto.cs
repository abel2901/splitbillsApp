namespace SplitBillsApi.DTO
{
    public class GastoCreateDto
    {
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public int PagoPorId { get; set; }  // Enviar apenas o Id do usuário
        public int GrupoId { get; set; }
    }
}
