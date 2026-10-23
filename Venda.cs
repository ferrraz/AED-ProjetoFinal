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
    internal class Venda
    {
        static string ConnectionString = "Password=Bdsql@123;Persist Security Info=True;User ID=Sa;Initial Catalog=ProjetoFinal;Data Source=localhost,1433;TrustServerCertificate=True;";
        public static void menuVenda()
        {
            string menuTexto =
                "   Menu de Venda\n\n" +
                "   1. Criar Venda(s).\n" +
                "   2. Listar Vendas.\n" +
                "   3. Editar uma Venda.\n" +
                "   4. Remover uma Venda.\n" +
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

        public static void inserirVenda(int cliente, DateTime data, double desconto)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "INSERT INTO Venda(cliente, data, desconto) VALUES (@cliente, @data, @desconto)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlParameter sqlParameterCliente = new SqlParameter("@cliente", System.Data.SqlDbType.Int);
                    sqlParameterCliente.Direction = System.Data.ParameterDirection.Input;
                    sqlParameterCliente.Value = cliente;
                    cmd.Parameters.Add(sqlParameterCliente);

                    SqlParameter sqlParameterData = new SqlParameter("@data", System.Data.SqlDbType.Date);
                    sqlParameterData.Direction = System.Data.ParameterDirection.Input;
                    sqlParameterData.Value = data;
                    cmd.Parameters.Add(sqlParameterData);

                    SqlParameter sqlParameterDesconto = new SqlParameter("@desconto", System.Data.SqlDbType.Decimal, 100);
                    sqlParameterDesconto.Direction = System.Data.ParameterDirection.Input;
                    sqlParameterDesconto.Value = desconto;
                    cmd.Parameters.Add(sqlParameterDesconto);

                    conn.Open();
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    Console.WriteLine(linhasAfetadas);
                }
            }
        }

        public static void listarVenda()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT idVenda, cliente, data, desconto FROM Venda";

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
                            reader["idVenda"],
                            reader["cliente"],
                            reader["data"],
                            reader["desconto"]
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
                                new Cell("ID do Cliente"){Color = ConsoleColor.Yellow},
                                new Cell("Data"){Color = ConsoleColor.Yellow},
                                new Cell("Desconto"){Color = ConsoleColor.Yellow},

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

        public static bool apagarVenda(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT COUNT(*) FROM Venda WHERE idVenda = @idVenda";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    string checkSql = "SELECT COUNT(*) FROM Venda WHERE idVenda = @idVenda";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@idVenda", id);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            return false;
                        }
                    }

                    string deleteSql = "DELETE FROM Venda WHERE idVenda = @idVenda";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteSql, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@id", id);
                        deleteCmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
        }

        public static bool editarVenda(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT COUNT(*) FROM Venda WHERE idVenda = @idVenda";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    string checkSql = "SELECT COUNT(*) FROM Venda WHERE idVenda = @idVenda";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@idVenda", id);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            return false;
                        }
                    }

                    string cliente;
                    string date;
                    decimal desconto;

                    Console.WriteLine("Digite o nome do cliente: ");
                    cliente = Console.ReadLine();

                    Console.WriteLine("Digite a data da venda: ");
                    date = Console.ReadLine();
                    DateTime data = DateTime.Parse(date);

                    Console.WriteLine("Digite o desconto na Venda: ");
                    desconto = decimal.Parse(Console.ReadLine());

                    string editSql = "UPDATE Venda SET cliente = @cliente, data = @data, desconto = @desconto WHERE idVenda = @idVenda";
                    using (SqlCommand editCmd = new SqlCommand(editSql, conn))
                    {
                        editCmd.Parameters.AddWithValue("@idVenda", id);
                        editCmd.Parameters.AddWithValue("@cliente", cliente);
                        editCmd.Parameters.AddWithValue("@data", data);
                        editCmd.Parameters.AddWithValue("@desconto", desconto);

                        editCmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
        }

        public static bool idVendaExiste(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT COUNT(*) FROM Venda WHERE idVenda = @id ";

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
