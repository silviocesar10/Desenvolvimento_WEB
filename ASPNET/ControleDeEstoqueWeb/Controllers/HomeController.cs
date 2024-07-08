using Microsoft.AspNetCore.Mvc;
using ControleDeEstoqueWeb.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoqueWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ControleDeEstoqueWebContext _context;

        public HomeController(ControleDeEstoqueWebContext context)
        {
            this._context = context;
        }
         public async Task<IActionResult> Index()
        {
            var pedidos = await _context.Pedidos
                .Where(p => !p.DataPedido.HasValue)
                .Include(p => p.Cliente)
                .OrderByDescending(p => p.IdPedido)
                .AsNoTracking().ToListAsync();

            return View(pedidos);
        }
    }
}