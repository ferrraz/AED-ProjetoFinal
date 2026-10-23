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
    internal class Produto
    {
        static string ConnectionString = "Password=Bdsql@123;Persist Security Info=True;User ID=Sa;Initial Catalog=ProjetoFinal;Data Source=localhost,1433;TrustServerCertificate=True;";

        public static void menuProduto()
        {
            string menuTexto =
                "   Menu do Produto\n\n" +
                "   1. Adicionar Produto(s).\n" +
                "   2. Listar Produtos.\n" +
                "   3. Listar um Produto em específico.\n" +
                "   4. Remover um Produto.\n" +
                "   5. Editar um Produto\n\n" +
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

        public static void inserirProduto(string nome, int categoria, int preco)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "INSERT INTO Produto(nome, categoria, preco) VALUES (@nome, @categoria, @preco)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlParameter sqlParameterNome = new SqlParameter("@nome", System.Data.SqlDbType.NVarChar, 100);
                    sqlParameterNome.Direction = System.Data.ParameterDirection.Input;
                    sqlParameterNome.Value = nome;
                    cmd.Parameters.Add(sqlParameterNome);

                    /**/

                  


                    SqlParameter sqlParameterCategoria = new SqlParameter("@categoria", System.Data.SqlDbType.NVarChar, 100);
                    sqlParameterCategoria.Direction = System.Data.ParameterDirection.Input;
                    sqlParameterCategoria.Value = categoria;
                    cmd.Parameters.Add(sqlParameterCategoria);

                    SqlParameter sqlParameterPreco = new SqlParameter("@preco", System.Data.SqlDbType.Int);
                    sqlParameterPreco.Direction = System.Data.ParameterDirection.Input;
                    sqlParameterPreco.Value = preco;
                    cmd.Parameters.Add(sqlParameterPreco);

                    conn.Open();
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    Console.WriteLine(linhasAfetadas);
                }
            }
        }

        public static void listarProduto()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT idproduto, categoria, nome, preco FROM Produto";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var rows = new List<object[]>();

                        while (reader.Read())
                        {
                            rows.Add(new object[]
                            {
                            reader["idproduto"],
                            reader["categoria"],
                            reader["nome"],
                            reader["preco"]
                            });
                        }
                        var doc = new Document(
                            new Grid
                            {
                                Stroke = LineThickness.Single,
                                Columns =
                                {
                                new Column{ Width = GridLength.Auto },
                                new Column{ Width = GridLength.Auto },
                                new Column{ Width = GridLength.Auto },
                                new Column{ Width = GridLength.Auto },
                                },
                                Children =
                                {
                                new Cell("ID"){Color = ConsoleColor.Yellow},
                                new Cell("Categoria"){Color = ConsoleColor.Yellow},
                                new Cell("Nome"){Color = ConsoleColor.Yellow},
                                new Cell("Preço"){Color = ConsoleColor.Yellow},

                                rows.ConvertAll(r => new[]
                                {
                                    new Cell(r[0]),
                                    new Cell(r[1]),
                                    new Cell(r[2]),
                                    new Cell(r[3]),
                                })
                                }
                            }
                        );

                        ConsoleRenderer.RenderDocument(doc);
                    }
                }
            }
        }

        public static bool apagarProduto(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT COUNT(*) FROM Produto WHERE idproduto = @idproduto";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    string checkSql = "SELECT COUNT(*) FROM Produto WHERE idproduto = @idproduto";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@id", id);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            return false;
                        }
                    }

                    string deleteSql = "DELETE FROM Produto WHERE idproduto = @idproduto";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteSql, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@idproduro", id);
                        deleteCmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
        }

        public static bool editarProduto(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT COUNT(*) FROM Produto WHERE idproduto = @idproduto";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    string checkSql = "SELECT COUNT(*) FROM Produto WHERE idproduto = @idproduto";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@idproduto", id);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            return false;
                        }
                    }

                    string nome;
                    int categoria;
                    int preco;

                    Console.WriteLine("Digite o nome do Produto: ");
                    nome = Console.ReadLine();

                    Console.WriteLine("Digite a Categoria do Produto: ");
                    categoria = int.Parse(Console.ReadLine());

                    Console.WriteLine("Digite o preço do Produto: ");
                    preco = int.Parse(Console.ReadLine());

                    string editSql = "UPDATE Produto SET nome = @nome, categoria = @categoria, preco = @preco WHERE idproduto = @idproduto";
                    using (SqlCommand editCmd = new SqlCommand(editSql, conn))
                    {
                        editCmd.Parameters.AddWithValue("@id", id);
                        editCmd.Parameters.AddWithValue("@nome", nome);
                        editCmd.Parameters.AddWithValue("@categoria", preco);

                        editCmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
        }

        public static bool idProdutoExiste(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT COUNT(*) FROM Produto WHERE idproduto = @id ";

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
