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
    internal class Cliente
    {

        static string ConnectionString = "Password=Bdsql@123;Persist Security Info=True;User ID=Sa;Initial Catalog=ProjetoFinal;Data Source=localhost,1433;TrustServerCertificate=True;";

        public static void menuCliente()
        {
            string menuTexto =
                "   Menu do Cliente\n\n" +
                "   1. Adicionar Cliente(s).\n" +
                "   2. Listar Clientes.\n" +
                "   3. Remover um Cliente.\n" +
                "   4. Editar um Cliente\n\n" +
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
        public static void inserirCliente(string nome, string cidade, string numero, string codPostal, string CC)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "INSERT INTO Cliente(nome, cidade, numero, codPostal, CC) VALUES (@nome, @cidade, @numero, @codPostal, @CC)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlParameter sqlParameterNome = new SqlParameter("@nome", System.Data.SqlDbType.NVarChar, 100);
                    sqlParameterNome.Direction = System.Data.ParameterDirection.Input;
                    sqlParameterNome.Value = nome;
                    cmd.Parameters.Add(sqlParameterNome);

                    SqlParameter sqlParameterCidade = new SqlParameter("@cidade", System.Data.SqlDbType.NVarChar, 100);
                    sqlParameterCidade.Direction = System.Data.ParameterDirection.Input;
                    sqlParameterCidade.Value = cidade;
                    cmd.Parameters.Add(sqlParameterCidade);

                    SqlParameter sqlParameterNumero = new SqlParameter("@numero", System.Data.SqlDbType.Int);
                    sqlParameterNumero.Direction = System.Data.ParameterDirection.Input;
                    sqlParameterNumero.Value = numero;
                    cmd.Parameters.Add(sqlParameterNumero);

                    SqlParameter sqlParameterCodPostal = new SqlParameter("@codPostal", System.Data.SqlDbType.NVarChar, 100);
                    sqlParameterCodPostal.Direction = System.Data.ParameterDirection.Input;
                    sqlParameterCodPostal.Value = codPostal;
                    cmd.Parameters.Add(sqlParameterCodPostal);

                    SqlParameter sqlParameterCC = new SqlParameter("@CC", System.Data.SqlDbType.NVarChar, 100);
                    sqlParameterCC.Direction = System.Data.ParameterDirection.Input;
                    sqlParameterCC.Value = CC;
                    cmd.Parameters.Add(sqlParameterCC);

                    conn.Open();
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    Console.WriteLine(linhasAfetadas);
                }
            }
        }


        public static void listarCliente()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT idCliente, nome, cidade, numero, codPostal, CC FROM Cliente";

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
                            reader["idCliente"],
                            reader["nome"],
                            reader["cidade"],
                            reader["numero"],
                            reader["codPostal"],
                            reader["CC"]
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
                                new Column{ Width = GridLength.Auto },
                                new Column{ Width = GridLength.Auto }
                                },
                                Children =
                                {
                                new Cell("ID"){Color = ConsoleColor.Yellow},
                                new Cell("Nome"){Color = ConsoleColor.Yellow},
                                new Cell("Cidade"){Color = ConsoleColor.Yellow},
                                new Cell("Número"){Color = ConsoleColor.Yellow},
                                new Cell("Código Postal"){Color = ConsoleColor.Yellow},
                                new Cell("CC"){Color = ConsoleColor.Yellow},


                                rows.ConvertAll(r => new[]
                                {
                                    new Cell(r[0]),
                                    new Cell(r[1]),
                                    new Cell(r[2]),
                                    new Cell(r[3]),
                                    new Cell(r[4]),
                                    new Cell(r[5])
                                })
                                }
                            }
                        );

                        ConsoleRenderer.RenderDocument(doc);
                    }
                }
            }
        }


        public static bool apagarCliente(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT COUNT(*) FROM Cliente WHERE idCliente = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    string checkSql = "SELECT COUNT(*) FROM Cliente WHERE idCliente = @id";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@id", id);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            return false;
                        }
                    }

                    string deleteSql = "DELETE FROM Cliente WHERE idCliente = @id";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteSql, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@id", id);
                        deleteCmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
        }

        public static bool editarCliente(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT COUNT(*) FROM Cliente WHERE idCliente = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    string checkSql = "SELECT COUNT(*) FROM Cliente WHERE idCliente = @id";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@id", id);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            return false;
                        }
                    }

                    string nome;
                    string cidade;
                    string codPostal;
                    string CC;
                    int numero;

                    Console.WriteLine("Digite o nome do cliente: ");
                    nome = Console.ReadLine();

                    Console.WriteLine("Digite a cidade do cliente: ");
                    cidade = Console.ReadLine();

                    Console.WriteLine("Digite o numero da cliente: ");
                    numero = int.Parse(Console.ReadLine());

                    Console.WriteLine("Digite o codigo postal do cliente: ");
                    codPostal = Console.ReadLine();

                    Console.WriteLine("Digite o CC do cliente: ");
                    CC = Console.ReadLine();

                    string editSql = "UPDATE Cliente SET nome = @nome, cidade = @cidade, numero = @numero, codPostal = @codPostal, CC = @CC WHERE idCliente = @id";
                    using (SqlCommand editCmd = new SqlCommand(editSql, conn))
                    {
                        editCmd.Parameters.AddWithValue("@id", id);
                        editCmd.Parameters.AddWithValue("@nome", nome);
                        editCmd.Parameters.AddWithValue("@cidade", cidade);
                        editCmd.Parameters.AddWithValue("@numero", numero);
                        editCmd.Parameters.AddWithValue("@codPostal", codPostal);
                        editCmd.Parameters.AddWithValue("@cc", CC);

                        editCmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
        }

        public static bool idClienteExiste(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT COUNT(*) FROM Cliente WHERE idCliente = @id ";

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
