using IntroducaoAoMediatR.Features.Vitrine;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IntroducaoAoMediatR.Controllers
{
    public class Loja : Controller
    {
        private readonly IMediator mediator;

        public Loja(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IActionResult> Vitrine()
        {
            return View(await mediator.Send(new Vitrine()));
        }

        public async Task<IActionResult> Detalhes(int id)
        {
            return View(await mediator.Send(new Detalhes(id)));
        }
    }
}