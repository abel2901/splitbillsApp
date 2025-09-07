using SplitBillsApi.DTO;
using SplitBillsApi.Models;

namespace SplitBillsApi.Service
{
    public class SaldoService
    {
        public List<SaldoUsuario> CalcularSaldos(Grupo grupo)
        {
            var usuarios = grupo.Usuarios.ToList();
            var saldos = usuarios.ToDictionary(u => u.Id, u => new SaldoUsuario
            {
                UsuarioId = u.Id,
                Nome = u.Nome,
                Saldo = 0
            });

            foreach (var gasto in grupo.Gastos)
            {
                var valorDividido = gasto.Valor / usuarios.Count;

                foreach (var usuario in usuarios)
                {
                    if (usuario.Id == gasto.PagoPorId)
                    {
                        saldos[usuario.Id].Saldo += gasto.Valor - valorDividido;
                    }
                    else
                    {
                        saldos[usuario.Id].Saldo -= valorDividido;
                    }
                }
            }

            return saldos.Values.ToList();
        }
    }
}
