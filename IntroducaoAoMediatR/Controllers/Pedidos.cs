using IntroducaoAoMediatR.Features;
using IntroducaoAoMediatR.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IntroducaoAoMediatR.Controllers
{
    public class Pedidos : Controller
    {
        private readonly IMediator mediator;

        public Pedidos(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Comprar([FromForm]ItemDeCompra item)
        {
            await mediator.Send(new Comprar() { ProdutoId = item.ProdutoId, Quantidade = item.Quantidade });

            return RedirectToAction("Relacao");
        }

        public async Task<IActionResult> Relacao()
        {
            return View(await mediator.Send(new Relacao()));
        }
    }
}