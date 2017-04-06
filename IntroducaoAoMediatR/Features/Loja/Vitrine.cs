using IntroducaoAoMediatR.Models;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntroducaoAoMediatR.Features.Vitrine
{
    public class Vitrine : IRequest<IEnumerable<Produto>> { }

    public class VitrineHandler : IAsyncRequestHandler<Vitrine, IEnumerable<Produto>>
    {
        private readonly IRepositorioDeProdutos repositorioDeProdutos;
        private readonly IMemoryCache cache;

        private const string ChaveDoCache = "__Catalogo";

        public VitrineHandler(IRepositorioDeProdutos repositorioDeProdutos, IMemoryCache cache)
        {
            this.repositorioDeProdutos = repositorioDeProdutos;
            this.cache = cache;
        }

        public async Task<IEnumerable<Produto>> Handle(Vitrine message)
        {
            IEnumerable<Produto> produtos = null;

            if (!cache.TryGetValue(ChaveDoCache, out produtos))
            {
                produtos = await repositorioDeProdutos.BuscarVitrine();

                if (produtos?.Any() ?? false)
                    cache.Set(ChaveDoCache, produtos);
            }

            return produtos;
        }
    }
}