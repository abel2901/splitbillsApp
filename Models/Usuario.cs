using System.Text.RegularExpressions;

namespace SplitBillsApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public ICollection<Gasto> GastosPagos { get; set; } = new List<Gasto>();
        public ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
    }
}
