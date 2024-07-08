using System;
using System.Linq;
using System.Threading.Tasks;
using ControleDeEstoqueWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControleDeEstoqueWeb.Controllers
{
    public class PedidoController : Controller
    {
        private readonly ControleDeEstoqueWebContext _context;

        public PedidoController(ControleDeEstoqueWebContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index(int? cid)
        {
            if (cid.HasValue)
            {
                var cliente = await _context.Clientes.FindAsync(cid);
                if (cliente != null)
                {
                    var pedido = await _context.Pedidos
                    .Where(p => p.IdCliente == cid)
                    .OrderByDescending(x => x.IdPedido)
                    .AsNoTracking()
                    .ToListAsync();
                    ViewBag.Cliente = cliente;
                    return View(pedido);
                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Cliente nao encontrado!!", TipoMensagem.Erro);
                    return RedirectToAction("Index", "Cliente");
                }
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Cliente nao informado!!", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Cadastrar(int? cid)
        {
            if (cid.HasValue)
            {
                var cliente = await _context.Clientes.FindAsync(cid);
                if (cliente != null)
                {
                    _context.Entry(cliente).Collection(c => c.Pedidos).Load();
                    PedidoModel pedido = null;
                    if (_context.Pedidos.Any(p => p.IdCliente == cid && !p.DataPedido.HasValue))
                    {
                        pedido = _context.Pedidos.FirstOrDefault(p => p.IdCliente == cid && !p.DataPedido.HasValue);
                    }
                    else
                    {
                        pedido = new PedidoModel { IdCliente = cid.Value, ValorTotal = 0 };
                        cliente.Pedidos.Add(pedido);
                        await _context.SaveChangesAsync();

                    }
                    return RedirectToAction("Index", "ItemPedido", new { ped = pedido.IdPedido });
                }
                TempData["mensagem"] = MensagemModel.Serializar("Cliente nao encontrado!!", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }
            TempData["mensagem"] = MensagemModel.Serializar("Cliente nao informado!!", TipoMensagem.Erro);
            return RedirectToAction("Index", "Cliente");
        }

        private bool PedidoExiste(int id)
        {
            return _context.Pedidos.Any(e => e.IdPedido == id);
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(int? id, [FromForm] PedidoModel pedido)
        {
            if (ModelState.IsValid)
            {
                if (id.HasValue)
                {
                    if (PedidoExiste(id.Value))
                    {
                        _context.Update(pedido);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Pedido alterada com sucesso!!");
                        }
                        else
                        {
                            TempData["mensagem"] = MensagemModel.Serializar("Erro ao alterar pedido!!", TipoMensagem.Erro);
                        }
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontrada!!!", TipoMensagem.Erro);
                    }
                }
                else
                {
                    _context.Add(pedido);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Pedido cadastrada com sucesso!!");
                    }
                    else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Erro ao cadastrar pedido", TipoMensagem.Erro);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(pedido);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if (!id.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não informado!!", TipoMensagem.Erro);
                return RedirectToAction("Index");
            }

            if (PedidoExiste(id.Value))
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontrado!!", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }

            var pedido = await _context.Pedidos
                    .Include(p => p.Cliente)
                    .Include(p => p.ItensPedido)
                    .ThenInclude(i => i.Produto)
                    .FirstOrDefaultAsync(p => p.IdPedido == id);
            return View(pedido);
        }
        
        [HttpPost]
        public async Task<IActionResult> Excluir(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                if (await _context.SaveChangesAsync() > 0)
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Pedido excluido com sucesso!!");
                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Não foi possivel exlcuir a pedido!!", TipoMensagem.Erro);
                }
                return RedirectToAction(nameof(Index), new {cid = pedido.IdCliente});
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontradao!!", TipoMensagem.Erro);
                return RedirectToAction(nameof(Index), "Cliente ");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Fechar(int? id)
        {
            if (!id.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não informado!!", TipoMensagem.Erro);
                return RedirectToAction("Index");
            }

            if (PedidoExiste(id.Value))
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontrado!!", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }

            var pedido = await _context.Pedidos
                    .Include(p => p.Cliente)
                    .Include(p => p.ItensPedido)
                    .ThenInclude(i => i.Produto)
                    .FirstOrDefaultAsync(p => p.IdPedido == id);
            return View(pedido);
        }
        
        [HttpPost]
        public async Task<IActionResult> Fechar(int id)
        {
            if(PedidoExiste(id))
            {
                var pedido = await _context.Pedidos
                    .Include(p => p.Cliente)
                    .Include(p => p.ItensPedido)
                    .ThenInclude(i => i.Produto)
                    .FirstOrDefaultAsync(p => p.IdPedido == id);
                if(pedido.ItensPedido.Count() > 0)
                {
                    pedido.DataPedido = DateTime.Now;
                    foreach(var item in pedido.ItensPedido)
                    {
                        item.Produto.Estoque -= item.Quantidade;
                    }
                    if(await _context.SaveChangesAsync() > 0)
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Pedido fechado com sucesso!!");
                    }else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Nao foi possivel fechar o seu pedido!", TipoMensagem.Erro);
                    }
                    return RedirectToAction(nameof(Index), new {cid = pedido.IdCliente});
                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Nao é possivel fechar um pedido sem itens!", TipoMensagem.Erro);
                   return RedirectToAction(nameof(Index), new {cid = pedido.IdCliente});

                }
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Erro pedido nao encontrado!", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Entregar(int? id)
        {
            if (!id.HasValue)
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não informado!!", TipoMensagem.Erro);
                return RedirectToAction("Index");
            }

            if (PedidoExiste(id.Value))
            {
                TempData["mensagem"] = MensagemModel.Serializar("Pedido não encontrado!!", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }

            var pedido = await _context.Pedidos
                    .Include(p => p.Cliente)
                    .ThenInclude(c => c.Enderecos)
                    .Include(p => p.ItensPedido)
                    .ThenInclude(i => i.Produto)
                    .FirstOrDefaultAsync(p => p.IdPedido == id);
            return View(pedido);
        }
        
        [HttpPost]
        public async Task<IActionResult> Entregar(int idPedido, int idEndereco)
        {
            if(PedidoExiste(idPedido))
            {
                var pedido = await _context.Pedidos
                    .Include(p => p.Cliente)
                    .ThenInclude(c => c.Enderecos)
                    .FirstOrDefaultAsync(p => p.IdPedido == idPedido);
                var endereco = pedido.Cliente.Enderecos
                    .FirstOrDefault(e => e.IdEndereco == idEndereco);
                if(endereco != null)
                {
                    pedido.EnderecoEntrega = endereco;
                    pedido.DataEntrega = DateTime.Now;
                    if(await _context.SaveChangesAsync() > 0)
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Entrega regitrada com sucesso!!");
                    }else
                    {
                        TempData["mensagem"] = MensagemModel.Serializar("Nao foi possivel registrar a entraga do pedido!", TipoMensagem.Erro);
                    }
                    return RedirectToAction(nameof(Index), new {cid = pedido.IdCliente});
                }
                else
                {
                    TempData["mensagem"] = MensagemModel.Serializar("Endereco nao encontrado!!!", TipoMensagem.Erro);
                   return RedirectToAction(nameof(Index), new {cid = pedido.IdCliente});

                }
            }
            else
            {
                TempData["mensagem"] = MensagemModel.Serializar("Erro pedido nao encontrado!", TipoMensagem.Erro);
                return RedirectToAction("Index", "Cliente");
            }
        }
    }
}