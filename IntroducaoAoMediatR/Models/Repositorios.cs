using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace IntroducaoAoMediatR.Models
{
    public interface IRepositorio<T>
    {
        Task<T> BuscarPor(int id);

        Task Adicionar(T item);
    }

    public interface IRepositorioDeProdutos : IRepositorio<Produto>
    {
        Task<IEnumerable<Produto>> BuscarVitrine();
    }

    public interface IRepositorioDePedido : IRepositorio<Pedido>
    {
        Task<IEnumerable<Pedido>> BuscarUltimosIntroducaoAoMediatR();
    }

    public interface IRepositorioDeNotasFiscais : IRepositorio<NotaFiscal> { }

    public class RepositorioDeProdutos : IRepositorioDeProdutos
    {
        public Task Adicionar(Produto item) { return null; }

        public async Task<Produto> BuscarPor(int id)
        {
            using (var cmd = DbExtensions.BuildCommand("SELECT Id, Descricao, Valor, Imagem FROM Produto WHERE Id = @Id"))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.Connection.OpenAsync();

                using (var dr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    if (dr.Read())
                    {
                        return new Produto()
                        {
                            Id = dr.GetInt32(0),
                            Descricao = dr.GetString(1),
                            Valor = dr.GetDecimal(2),
                            Imagem = dr.GetString(3)
                        };
                    }
                }
            }

            return null;
        }

        public async Task<IEnumerable<Produto>> BuscarVitrine()
        {
            var produtos = new List<Produto>();

            using (var cmd = DbExtensions.BuildCommand("SELECT Id, Descricao, Valor, Imagem FROM Produto"))
            {
                await cmd.Connection.OpenAsync();

                using (var dr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        produtos.Add(new Produto()
                        {
                            Id = dr.GetInt32(0),
                            Descricao = dr.GetString(1),
                            Valor = dr.GetDecimal(2),
                            Imagem = dr.GetString(3)
                        });
                    }
                }
            }

            return produtos;
        }
    }

    public class PedidoDePedidos : IRepositorioDePedido
    {
        public async Task Adicionar(Pedido pedido)
        {
            using (var cmd = DbExtensions.BuildCommand("INSERT INTO Pedido VALUES (@Id, @Data, @Total)"))
            {
                cmd.Parameters.AddWithValue("@Id", pedido.Id);
                cmd.Parameters.AddWithValue("@Data", pedido.Data);
                cmd.Parameters.AddWithValue("@Total", pedido.Total);

                await cmd.Connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

            using (var cmd = DbExtensions.BuildCommand("INSERT INTO PedidoItem (PedidoId, ProdutoId, Quantidade, Total) VALUES (@PedidoId, @ProdutoId, @Quantidade, @Total)"))
            {
                cmd.Parameters.AddWithValue("@PedidoId", pedido.Id);
                cmd.Parameters.Add("@ProdutoId", SqlDbType.Int);
                cmd.Parameters.Add("@Quantidade", SqlDbType.Int);
                cmd.Parameters.Add("@Total", SqlDbType.Decimal);

                await cmd.Connection.OpenAsync();

                foreach (var item in pedido.Itens)
                {
                    cmd.Parameters["@ProdutoId"].Value = item.Produto.Id;
                    cmd.Parameters["@Quantidade"].Value = item.Quantidade;
                    cmd.Parameters["@Total"].Value = item.Total;

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Pedido> BuscarPor(int id)
        {
            #region Consulta
            const string consulta = @"
SELECT
	  p.Id
	, p.Data
	, pr.Id
	, pr.Descricao
	, pr.Valor
	, pr.Imagem
FROM PedidoItem pi 
INNER JOIN Produto pr ON pr.Id = pi.ProdutoId
INNER JOIN Pedido p ON p.Id = pi.PedidoId
WHERE
	p.Id = @Id";
            #endregion

            Pedido pedido = null;

            using (var cmd = DbExtensions.BuildCommand(consulta))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                await cmd.Connection.OpenAsync();

                using (var dr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        if (pedido == null)
                            pedido = new Pedido()
                            {
                                Id = dr.GetGuid(0),
                                Data = dr.GetDateTime(1)
                            };

                        pedido.Adicionar(
                            new Pedido.Item(
                                new Produto()
                                {
                                    Id = dr.GetInt32(2),
                                    Descricao = dr.GetString(3),
                                    Valor = dr.GetDecimal(4),
                                    Imagem = dr.GetString(5)
                                }, dr.GetInt32(0)));
                    }
                }
            }

            return pedido;
        }

        public async Task<IEnumerable<Pedido>> BuscarUltimosIntroducaoAoMediatR()
        {
            var IntroducaoAoMediatR = new List<Pedido>();

            using (var cmd = DbExtensions.BuildCommand("SELECT Id, Data, Total FROM Pedido"))
            {
                await cmd.Connection.OpenAsync();

                using (var dr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (dr.Read())
                    {
                        IntroducaoAoMediatR.Add(new Pedido()
                        {
                            Id = dr.GetGuid(0),
                            Data = dr.GetDateTime(1),
                            Total = dr.GetDecimal(2)
                        });
                    }
                }
            }

            return IntroducaoAoMediatR;
        }
    }

    public class RepositorioDeNotasFiscais : IRepositorioDeNotasFiscais
    {
        public async Task Adicionar(NotaFiscal item)
        {
            using (var cmd = DbExtensions.BuildCommand("INSERT INTO NotaFiscal VALUES (@Codigo, @Data, @Valor)"))
            {
                cmd.Parameters.AddWithValue("@Codigo", item.Codigo);
                cmd.Parameters.AddWithValue("@Data", item.Data);
                cmd.Parameters.AddWithValue("@Valor", item.Valor);

                await cmd.Connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public Task<NotaFiscal> BuscarPor(int id)
        {
            throw new NotImplementedException();
        }
    }

    internal static class DbExtensions
    {
        private const string ConnString = "Data Source=.;Initial Catalog=Loja;Integrated Security=True;Pooling=False";

        internal static SqlCommand BuildCommand(string query) => new SqlCommand(query, new SqlConnection(ConnString));
    }
}