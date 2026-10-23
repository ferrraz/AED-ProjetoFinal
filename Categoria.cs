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
    internal class Categoria
    {
        static string ConnectionString = "Password=Bdsql@123;Persist Security Info=True;User ID=Sa;Initial Catalog=ProjetoFinal;Data Source=localhost,1433;TrustServerCertificate=True;";

        public static void menuCategoria()
        {
            string menuTexto =
                "   Menu da Categoria\n\n" +
                "   1. Adicionar Categoria(s).\n" +
                "   2. Listar Categorias.\n" +
                "   3. Remover uma Categoria.\n" +
                "   4. Editar uma Categoria\n\n" +
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

        public static void inserirCategoria(string nome)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "INSERT INTO Categoria(nome) VALUES (@nome)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    SqlParameter sqlParameterNome = new SqlParameter("@nome", System.Data.SqlDbType.NVarChar, 100);
                    sqlParameterNome.Direction = System.Data.ParameterDirection.Input;
                    sqlParameterNome.Value = nome;
                    cmd.Parameters.Add(sqlParameterNome);

                    conn.Open();
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    Console.WriteLine(linhasAfetadas);
                }
            }
        }

        public static void listarCategoria()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT idcategoria, nome FROM Categoria";

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
                            reader["idcategoria"],
                            reader["nome"]
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
                                },
                                Children =
                                {
                                new Cell("ID"){Color = ConsoleColor.Yellow},
                                new Cell("Nome"){Color = ConsoleColor.Yellow},

                                rows.ConvertAll(r => new[]
                                {
                                    new Cell(r[0]),
                                    new Cell(r[1]),
                                })
                                }
                            }
                        );

                        ConsoleRenderer.RenderDocument(doc);
                    }
                }
            }
        }
        public static bool editarCategoria(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT COUNT() FROM Categoria WHERE idcategoria = @idcategoria";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    string checkSql = "SELECT COUNT() FROM Categoria WHERE idcategoria = @idcategoria";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@idcategoria", id);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            return false;
                        }
                    }

                    string nome;


                    Console.WriteLine("Digite o nome da categoria: ");
                    nome = Console.ReadLine();


                    string editSql = "UPDATE Categoria SET nome = @nome WHERE idcategoria = @idcategoria";
                    using (SqlCommand editCmd = new SqlCommand(editSql, conn))
                    {
                        editCmd.Parameters.AddWithValue("@idcategoria", id);
                        editCmd.Parameters.AddWithValue("@nome", nome);


                        editCmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
        }

        public static bool removerCategoria(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                string sql = "SELECT COUNT() FROM Categoria WHERE idcategoria = @idcategoria";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    string checkSql = "SELECT COUNT() FROM categoria WHERE idcategoria = @idcategoria";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@id", id);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            return false;
                        }
                    }

                    string deleteSql = "DELETE FROM Categoria WHERE idcategoria = @id";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteSql, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@id", id);
                        deleteCmd.ExecuteNonQuery();
                    }

                    return true;
                }
            }
        }

        public static bool idCategoriaExiste(int id)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string sql = "SELECT COUNT(*) FROM Categoria WHERE idcategoria = @id ";

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
