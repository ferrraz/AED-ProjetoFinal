using Alba.CsConsoleFormat;
using Alba.CsConsoleFormat.Fluent;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using static System.ConsoleColor;

namespace ProjetoFinal
{
    internal class LinhaVenda
    {
        static string ConnectionString = "Password=Bdsql@123;Persist Security Info=True;User ID=Sa;Initial Catalog=ProjetoFinal;Data Source=localhost,1433;TrustServerCertificate=True;";

        public static void menuLinhaVenda()
        {
            string menuTexto =
                "   Menu da Linha de Venda\n\n" +
                "   1. Criar Linha de Venda(s).\n" +
                "   2. Listar Linhas de Vendas.\n" +
                "   3. Editar uma Linha de Venda.\n" +
                "   4. Remover uma Linha de Venda.\n" +
                "   0. Sair\n";

            var doc = new Document(
                new Grid
                {
                    Stroke = LineThickness.None,
                    Columns =
                    {
        new Column { Width = GridLength.Star(1) }
                    },
                    Children =
                    {
        new Cell(menuTexto)
        {
            Color = ConsoleColor.White,
            Align = Align.Left,
            Padding = new Thickness(4)
        }
                    }
                }
            );

            ConsoleRenderer.RenderDocument(doc);
        }

        public static void inserirLinhaVenda(int Venda, int Produto, int quantidade)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "INSERT INTO Cliente(venda, produto, quantidade) VALUES (@venda, @produto, @quantidade)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlParameter sqlParametervenda = new SqlParameter("@venda", System.Data.SqlDbType.Int);
                    sqlParametervenda.Direction = System.Data.ParameterDirection.Input;
                    sqlParametervenda.Value = Venda;
                    cmd.Parameters.Add(sqlParametervenda);

                    SqlParameter sqlParameterprodutos = new SqlParameter("@produto", System.Data.SqlDbType.Int);
                    sqlParameterprodutos.Direction = System.Data.ParameterDirection.Input;
                    sqlParameterprodutos.Value = Produto;
                    cmd.Parameters.Add(sqlParameterprodutos);

                    SqlParameter sqlParameterQuantidade = new SqlParameter("@numero", System.Data.SqlDbType.Int);
                    sqlParameterQuantidade.Direction = System.Data.ParameterDirection.Input;
                    sqlParameterQuantidade.Value = quantidade;
                    cmd.Parameters.Add(sqlParameterQuantidade);

                    conn.Open();
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    Console.WriteLine(linhasAfetadas);
                }
            }
        }

        public static void listarLinhaVenda()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT venda, produto, quantidade FROM LinhaVenda";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                    }
                }
            }
        }

        public static bool removerLinhaVenda(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT COUNT() FROM LinhaVenda WHERE venda = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    string checkSql = "SELECT COUNT() FROM LinhaVenda WHERE venda = @id";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@id", id);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            return false;
                        }
                    }

                    string deleteSql = "DELETE FROM LinhaVenda WHERE venda = @id";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteSql, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@id", id);
                        deleteCmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
        }

        public static bool editarLinhaVenda(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT COUNT() FROM LinhaVenda WHERE venda = @venda";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    string checkSql = "SELECT COUNT() FROM LinhaVenda WHERE venda = @venda";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@venda", id);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            return false;
                        }
                    }

                    string nome;


                    Console.WriteLine("Digite o nome da LinhaVendaa: ");
                    nome = Console.ReadLine();


                    string editSql = "UPDATE LinhaVenda SET nome = @nome WHERE venda = @venda";
                    using (SqlCommand editCmd = new SqlCommand(editSql, conn))
                    {
                        editCmd.Parameters.AddWithValue("@venda", id);
                        editCmd.Parameters.AddWithValue("@nome", nome);


                        editCmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
        }

        public static bool idLinhaVendaExiste(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT COUNT(*) FROM Venda WHERE venda = @id ";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@id", id);

                    int count = (int)cmd.ExecuteScalar();

                    return count > 0;
                }
            }
        }
    }
}
