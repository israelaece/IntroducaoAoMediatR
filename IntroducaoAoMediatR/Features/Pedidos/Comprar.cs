using IntroducaoAoMediatR.Features.NotasFiscais;
using IntroducaoAoMediatR.Models;
using MediatR;
using System.Threading.Tasks;

namespace IntroducaoAoMediatR.Features
{
    public class Comprar : IRequest
    {
        public int ProdutoId { get; set; }

        public int Quantidade { get; set; }
    }

    public class ComprarHandler : IAsyncRequestHandler<Comprar>
    {
        private readonly IRepositorioDeProdutos repositorioDeProdutos;
        private readonly IRepositorioDePedido repositorioDePedidos;
        private readonly IMediator mediator;

        public ComprarHandler(IRepositorioDeProdutos repositorioDeProdutos, IRepositorioDePedido repositorioDePedidos, IMediator mediator)
        {
            this.repositorioDeProdutos = repositorioDeProdutos;
            this.repositorioDePedidos = repositorioDePedidos;
            this.mediator = mediator;
        }

        public async Task Handle(Comprar message)
        {
            var produto = await repositorioDeProdutos.BuscarPor(message.ProdutoId);

            var pedido = new Pedido();
            pedido.Adicionar(new Pedido.Item(produto, message.Quantidade));

            await repositorioDePedidos.Adicionar(pedido);
            await mediator.Publish(new Emitir() { Valor = pedido.Total });
        }
    }
}