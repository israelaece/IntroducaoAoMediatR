using IntroducaoAoMediatR.Models;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace IntroducaoAoMediatR.Features.Vitrine
{
    public class Detalhes : IRequest<Produto>
    {
        public Detalhes(int produtoId) => this.ProdutoId = produtoId;

        public int ProdutoId { get; }
    }

    public class DetalhesHandler : IAsyncRequestHandler<Detalhes, Produto>
    {
        private readonly IRepositorioDeProdutos repositorioDeProdutos;
        private readonly IMemoryCache cache;

        private const string ChaveDoCache = "__ProdutoDetalhes";

        public DetalhesHandler(IRepositorioDeProdutos repositorioDeProdutos, IMemoryCache cache)
        {
            this.repositorioDeProdutos = repositorioDeProdutos;
            this.cache = cache;
        }

        public async Task<Produto> Handle(Detalhes message)
        {
            Produto produto = null;

            if (!cache.TryGetValue(ChaveDoCache, out produto))
            {
                produto = await repositorioDeProdutos.BuscarPor(message.ProdutoId);

                if (produto != null)
                    cache.Set(ChaveDoCache, produto);
            }

            return produto;
        }
    }
}