using System;

namespace IntroducaoAoMediatR.Models
{
    public class NotaFiscal
    {
        public Guid Codigo { get; set; } = Guid.NewGuid();

        public DateTime Data { get; set; } = DateTime.Now;

        public decimal Valor { get; set; }
    }
}