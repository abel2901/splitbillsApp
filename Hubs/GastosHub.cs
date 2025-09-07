using Microsoft.AspNetCore.SignalR;
using SplitBillsApi.Models;

namespace SplitBillsApi.Hubs
{
    public class GastosHub : Hub
    {
        public async Task NotificarNovoGasto(Gasto gasto)
        {
            await Clients.All.SendAsync("NovoGasto", gasto.Descricao, gasto.Valor, gasto.PagoPorId, gasto.GrupoId);
        }
    }
}
