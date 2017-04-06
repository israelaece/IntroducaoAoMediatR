using IntroducaoAoMediatR.Models;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntroducaoAoMediatR.Features
{
    public class Relacao : IRequest<IEnumerable<Pedido>> { }

    public class RelacaoHandler : IAsyncRequestHandler<Relacao, IEnumerable<Pedido>>
    {
        private readonly IRepositorioDePedido repositorioDeIntroducaoAoMediatR;

        public RelacaoHandler(IRepositorioDePedido repositorioDeIntroducaoAoMediatR)
        {
            this.repositorioDeIntroducaoAoMediatR = repositorioDeIntroducaoAoMediatR;
        }

        public async Task<IEnumerable<Pedido>> Handle(Relacao message)
        {
            return await repositorioDeIntroducaoAoMediatR.BuscarUltimosIntroducaoAoMediatR();
        }
    }
}