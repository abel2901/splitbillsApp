using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SplitBillsApi.Data;
using SplitBillsApi.Models;
using SplitBillsApi.Service;

namespace SplitBillsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GruposController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly SaldoService _saldoService;

        public GruposController(AppDbContext context, SaldoService saldoService)
        {
            _context = context;
            _saldoService = saldoService;
        }

        [HttpPost]
        public async Task<IActionResult> CriarGrupo([FromBody] Grupo grupo)
        {
            _context.Grupos.Add(grupo);
            await _context.SaveChangesAsync();
            return Ok(grupo);
        }

        [HttpGet("{id}/saldos")]
        public async Task<IActionResult> ObterSaldos(int id)
        {
            var grupo = await _context.Grupos
                .Include(g => g.Usuarios)
                .Include(g => g.Gastos)
                .ThenInclude(g => g.PagoPor)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (grupo == null) return NotFound();

            var saldos = _saldoService.CalcularSaldos(grupo);
            return Ok(saldos);
        }

    }
}
