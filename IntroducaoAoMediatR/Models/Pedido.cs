using System;
using System.Collections.Generic;
using System.Linq;

namespace IntroducaoAoMediatR.Models
{
    public class Pedido
    {
        private readonly IList<Item> itens = new List<Item>();

        public Guid Id { get; set; } = Guid.NewGuid();

        public decimal Total { get; set; }

        public IEnumerable<Item> Itens => itens.ToList();

        public DateTime Data { get; set; } = DateTime.Now;

        public void Adicionar(Item item)
        {
            itens.Add(item);
            Total += item.Total;
        }

        public class Item
        {
            public Item(Produto produto, int quantidade)
            {
                Produto = produto;
                Quantidade = quantidade;
            }

            public Produto Produto { get; }

            public int Quantidade { get; }

            public decimal Total => Produto.Valor * Quantidade;
        }
    }
}