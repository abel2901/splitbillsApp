using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SplitBillsApi.Data;
using SplitBillsApi.DTO;
using SplitBillsApi.Hubs;
using SplitBillsApi.Models;

namespace SplitBillsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GastoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<GastosHub> _hubContext;



        public GastoController(AppDbContext context, IHubContext<GastosHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> CriarGasto([FromBody] GastoCreateDto dto)
        {
            var gasto = new Gasto
            {
                Descricao = dto.Descricao,
                Valor = dto.Valor,
                GrupoId = dto.GrupoId,
                Data = DateTime.UtcNow,
                PagoPorId = dto.PagoPorId
            };
            gasto.Pago = false;

            _context.Gastos.Add(gasto);
            await _context.SaveChangesAsync();

            // Recarregar com includes para montar o DTO de saída
            var gastoComIncludes = await _context.Gastos
                .Include(g => g.PagoPor)
                .Include(g => g.Grupo)
                .FirstAsync(g => g.Id == gasto.Id);

            var gastoDto = new GastoDto
            {
                Id = gastoComIncludes.Id,
                Descricao = gastoComIncludes.Descricao,
                Valor = gastoComIncludes.Valor,
                PagoPorNome = gastoComIncludes.PagoPor.Nome,
                
            };

            // Notificar via SignalR
            await _hubContext.Clients.All.SendAsync("NovoGasto", gastoDto);

            return Ok(gastoDto);
        }

        //[HttpGet("{grupoId}")]
        //public async Task<IActionResult> ListarGastos([FromBody]int grupoId)
        //{
        //    var gastos = await _context.Gastos
        //        .Where(g => g.GrupoId == grupoId)
        //        .Select(g => new GastoDto
        //        {
        //            Id = g.Id,
        //            Descricao = g.Descricao,
        //            Valor = g.Valor,
        //            PagoPorUsuarioId = g.PagoPorId,
        //            PagoPorNome = g.PagoPor.Nome,
        //            Pago = g.Pago
        //        })
        //        .ToListAsync();

        //    return Ok(gastos);
        //}

        [HttpPost("{id}/pagar")]
        public async Task<IActionResult> PagarGasto(int id)
        {
            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto == null) return NotFound();

            gasto.Pago = true;
            await _context.SaveChangesAsync();

            // Notificar via SignalR que o gasto foi pago
            await _hubContext.Clients.All.SendAsync("GastoPago", gasto.Id);

            return Ok(gasto);
        }

        [HttpGet("{grupoId}")]
        public async Task<IActionResult> ListarGastosPendentes(int grupoId)
        {
            var gastos = await _context.Gastos
                .Where(g => g.GrupoId == grupoId && !g.Pago)
                .Include(g => g.PagoPor)
                .ToListAsync();

            var gastosDto = gastos.Select(g => new GastoDto
            {
                Id = g.Id,
                Descricao = g.Descricao,
                Valor = g.Valor,
                PagoPorUsuarioId = g.PagoPorId
            }).ToList();

            return Ok(gastosDto);
        }
    }
}
