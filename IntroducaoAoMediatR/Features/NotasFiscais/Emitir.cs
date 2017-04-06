using IntroducaoAoMediatR.Models;
using MediatR;
using System.Threading.Tasks;

namespace IntroducaoAoMediatR.Features.NotasFiscais
{
    public class Emitir : INotification
    {
        public decimal Valor { get; set; }
    }

    public class EmitirHandler : IAsyncNotificationHandler<Emitir>
    {
        private readonly IRepositorioDeNotasFiscais repositorioDeNotasFiscais;

        public EmitirHandler(IRepositorioDeNotasFiscais repositorioDeNotasFiscais)
        {
            this.repositorioDeNotasFiscais = repositorioDeNotasFiscais;
        }

        public async Task Handle(Emitir notification)
        {
            await repositorioDeNotasFiscais.Adicionar(new NotaFiscal() { Valor = notification.Valor });
        }
    }
}