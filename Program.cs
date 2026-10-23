namespace ProjetoFinal
{
    using Alba.CsConsoleFormat;
    using Alba.CsConsoleFormat.Fluent;
    using Microsoft.Data.SqlClient;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Reflection.PortableExecutable;
    using static System.ConsoleColor;

    internal class Program
    {

        public static void menu()
        {
            string menuTexto =
                "   Sistema de Gestão de Loja\n\n" +
                "   1. Menu Clientes\n" +
                "   2. Menu Produtos\n" +
                "   3. Menu Categorias\n" +
                "   4. Vendas\n" +
                "   5. Linhas de Venda\n\n" +
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

        static void Main(string[] args)
        {
            int escolha;

            do
            {
                Console.Clear();
                menu();
                Console.WriteLine("\n");
                escolha = int.Parse(Console.ReadLine());

                switch (escolha)
                {
                    case 1:
                        int escolhab;
                        Console.Clear();
                        
                        do
                        {
                            Console.Clear();
                            Cliente.menuCliente();
                            Console.WriteLine("\n");
                            escolhab = int.Parse(Console.ReadLine());

                            switch (escolhab)
                            {
                                case 1:
                                    Console.Clear();
                                    string numero;
                                    string nome;
                                    string cidade;
                                    string codPostal;
                                    string CC;
                                    int contadorpes;

                                    do
                                    {
                                        Console.WriteLine("Digite quantos clientes quer inserir");
                                        contadorpes = int.Parse(Console.ReadLine());

                                    }while(contadorpes <= 0);

                                    for (int i = 1; i <= contadorpes; i++)
                                    {
                                        Console.WriteLine($"Digite o nome do cliente {i}: ");
                                        nome = Console.ReadLine();

                                        Console.WriteLine($"Digite a cidade do cliente {i}: ");
                                        cidade = Console.ReadLine();

                                        do
                                        {
                                            Console.WriteLine($"Digite o número do cliente {i}: ");
                                            numero = Console.ReadLine();
                                        } while (numero.Length != 9 || !numero.All(char.IsDigit));

                                        do
                                        {
                                            Console.WriteLine($"Digite o código postal do cliente {i}: ");
                                            codPostal = Console.ReadLine();

                                        } while (codPostal.Length != 7 || !codPostal.All(c => char.IsDigit(c) || c == '-'));

                                        do
                                        {
                                            Console.WriteLine($"Digite o CC do cliente {i}: ");
                                            CC = Console.ReadLine();
                                        } while (codPostal.Length != 8 || !codPostal.All(char.IsDigit));

                                        Cliente.inserirCliente(nome, cidade, numero, codPostal, CC);
                                    }
                                    break;

                                case 2:
                                    Console.Clear();
                                    Cliente.listarCliente();
                                    Console.ReadLine();
                                    break;

                                case 3:
                                    int id;
                                    Console.Clear();

                                    Cliente.listarCliente();
                                    do
                                    {
                                        Console.WriteLine("Digite o id do cliente que quer remover: ");
                                        id = int.Parse(Console.ReadLine());
                                    }while(!Cliente.idClienteExiste(id));

                                    if (Cliente.apagarCliente(id))
                                    {
                                        Console.WriteLine("Cliente removido com sucesso.");
                                        Console.ReadLine();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Cliente não removido.");
                                        Console.ReadLine();
                                    }
                                    break;

                                case 4:
                                    int id2;
                                    Console.Clear();

                                    Cliente.listarCliente();
                                    do
                                    {
                                        Console.WriteLine("Digite o id do cliente que quer editar: ");
                                        id2 = int.Parse(Console.ReadLine());

                                    }while(!Cliente.idClienteExiste(id2));

                                    if (Cliente.editarCliente(id2))
                                    {
                                        Console.WriteLine("Cliente editado com sucesso.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Cliente não editado.");
                                    }
                                        break;

                                case 0:
                                    Console.WriteLine("A sair.");
                                    break;

                                default:
                                    Console.WriteLine("Opção invalida.");
                                    break;
                            }
                        } while (escolhab != 0);

                    break;

                    case 2:
                        int escolhac;
                        Console.Clear();

                        do
                        {
                            Console.Clear();
                            Produto.menuProduto();
                            Console.WriteLine("\n");
                            escolhac = int.Parse(Console.ReadLine());

                            switch (escolhac)
                            {
                                case 1:
                                    string nome;
                                    int categoria;
                                    int preco;
                                    int contadorpes;

                                    do
                                    {
                                        Console.WriteLine("Digite quantos produtos quer inserir");
                                        contadorpes = int.Parse(Console.ReadLine());
                                    } while (contadorpes <= 0);

                                    for (int i = 1; i <= contadorpes; i++)
                                    {
                                        Console.WriteLine($"Digite o nome do produto {i}: ");
                                        nome = Console.ReadLine();

                                        Categoria.listarCategoria();

                                        do
                                        {
                                            Console.WriteLine($"Digite o id da categoria do produto {i}: ");
                                            categoria = int.Parse(Console.ReadLine());
                                        } while (!Categoria.idCategoriaExiste(categoria));

                                        do
                                        {
                                            Console.WriteLine($"Digite o preço do produto {i}: ");
                                            preco = int.Parse(Console.ReadLine());
                                        }while( preco <= 0 );

                                        Produto.inserirProduto(nome, categoria, preco);
                                    }
                                    break;

                                case 2:
                                    Console.Clear();
                                    Produto.listarProduto();
                                    Console.ReadLine();
                                    break;

                                case 3:
                                    int id;
                                    Console.Clear();

                                    Produto.listarProduto();
                                    do
                                    {
                                        Console.WriteLine("Digite o id do produto que quer remover: ");
                                        id = int.Parse(Console.ReadLine());
                                    } while (!Produto.idProdutoExiste(id));

                                    if (Produto.apagarProduto(id))
                                    {
                                        Console.WriteLine("Produto removido com sucesso.");
                                        Console.ReadLine();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Produto não removido.");
                                        Console.ReadLine();
                                    }
                                    break;

                                case 4:
                                    int id2;
                                    Console.Clear();

                                    Produto.listarProduto();
                                    do
                                    {
                                        Console.WriteLine("Digite o id do cliente que quer editar: ");
                                        id2 = int.Parse(Console.ReadLine());
                                    }while(!Produto.idProdutoExiste(id2));

                                    if (Produto.editarProduto(id2))
                                    {
                                        Console.WriteLine("Produto editado com sucesso.");
                                        Console.ReadLine();
                                    }
                                    else{
                                        Console.WriteLine("Produto não editado.");
                                        Console.ReadLine();
                                    }
                                    break;

                                case 0:
                                    Console.WriteLine("A sair.");
                                    break;

                                default:
                                    Console.WriteLine("Opção invalida.");
                                    break;
                            }
                        } while (escolhac != 0);

                        break;

                    case 3:
                        int escolhad;
                        Console.Clear();

                        do
                        {
                            Console.Clear();
                            Categoria.menuCategoria();
                            Console.WriteLine("\n");
                            escolhad = int.Parse(Console.ReadLine());

                            switch (escolhad)
                            {
                                case 1:
                                    Console.Clear();
                                    string nome;
                                    int contadorpes;

                                    do
                                    {
                                        Console.WriteLine("Digite quantas categorias quer inserir");
                                        contadorpes = int.Parse(Console.ReadLine());
                                    }while(contadorpes <= 0);
               
                                    for (int i = 1; i <= contadorpes; i++)
                                    {
                                        Console.WriteLine($"Digite o nome da categoria {i}: ");
                                        nome = Console.ReadLine();

                                        Categoria.inserirCategoria(nome);
                                    }
                                    break;

                                case 2:
                                    Console.Clear();
                                    Categoria.listarCategoria();
                                    Console.ReadLine();
                                    break;

                                case 3:
                                    int id;
                                    Console.Clear();

                                    Categoria.listarCategoria();
                                    do
                                    {
                                        Console.WriteLine("Digite o id da categoria que quer remover: ");
                                        id = int.Parse(Console.ReadLine());
                                    }while(!Categoria.idCategoriaExiste(id));


                                    if (Categoria.removerCategoria(id))
                                    {
                                        Console.WriteLine("Categoria removida com sucesso.");
                                        Console.ReadLine();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Categoria não removida.");
                                        Console.ReadLine();
                                    }
                                    break;

                                case 4:
                                    int id2;
                                    Console.Clear();

                                    Categoria.listarCategoria();
                                    do
                                    {
                                        Console.WriteLine("Digite o id da categoria que quer editar: ");
                                        id2 = int.Parse(Console.ReadLine());
                                    } while (!Categoria.idCategoriaExiste(id2));
                                    
                                    if (Categoria.editarCategoria(id2))
                                    {
                                        Console.WriteLine("Categoria editada com sucesso.");
                                        Console.ReadLine();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Categoria não editada");
                                        Console.ReadLine();
                                    }
                                        break;

                                case 0:
                                    Console.WriteLine("A sair.");
                                    break;

                                default:
                                    Console.WriteLine("Opção invalida.");
                                    break;
                            }
                        } while (escolhad != 0);

                        break;

                    case 4:
                        int escolhae;
                        Console.Clear();

                        do
                        {
                            Console.Clear();
                            Venda.menuVenda();
                            Console.WriteLine("\n");
                            escolhae = int.Parse(Console.ReadLine());

                            switch (escolhae)
                            {
                                case 1:
                                    Console.Clear();
                                    int cliente;
                                    double desconto;
                                    string date;
                                    int contadorpes;

                                    do
                                    {
                                        Console.WriteLine("Digite quantas vendas quer inserir: ");
                                        contadorpes = int.Parse(Console.ReadLine());
                                    }while(contadorpes <= 0);

                                    for (int i = 1; i <= contadorpes; i++)
                                    {
                                        Cliente.listarCliente();

                                        do
                                        {
                                            Console.WriteLine($"Digite o id do cliente da venda {i}: ");
                                            cliente = int.Parse(Console.ReadLine());
                                        } while (!Cliente.idClienteExiste(cliente));

                                        Console.WriteLine($"Digite a data da venda {i}: ");
                                        date = Console.ReadLine();
                                        DateTime data = DateTime.Parse(date);

                                        do
                                        {
                                            Console.WriteLine($"Digite o deconto da venda {i}: ");
                                            desconto = float.Parse(Console.ReadLine());
                                        }while( desconto <= 0);

                                        Venda.inserirVenda(cliente, data, desconto);
                                    }
                                    break;

                                case 2:
                                    Console.Clear();
                                    Venda.listarVenda();
                                    Console.ReadLine();
                                    break;

                                case 3:
                                    int id;
                                    Console.Clear();

                                    Venda.listarVenda();
                                    do
                                    {
                                        Console.WriteLine("Digite o id da venda que quer remover: ");
                                        id = int.Parse(Console.ReadLine());
                                    }while(!Venda.idVendaExiste(id));

                                    if (Venda.apagarVenda(id))
                                    {
                                        Console.WriteLine("Venda removida com sucesso.");
                                        Console.ReadLine();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Venda não removida");
                                        Console.ReadLine();
                                    }
                                        break;

                                case 4:
                                    int id2;
                                    Console.Clear();

                                    Venda.listarVenda();
                                    do
                                    {
                                        Console.WriteLine("Digite o id da venda que quer editar: ");
                                        id2 = int.Parse(Console.ReadLine());
                                    }while(!Venda.idVendaExiste(id2));

                                    if (Venda.editarVenda(id2))
                                    {
                                        Console.WriteLine("Venda editada com sucesso.");
                                        Console.ReadLine();
                                    }
                                    else
                                    {
                                        Console.WriteLine("Venda não editada.");
                                        Console.ReadLine();
                                    }
                                        break;

                                case 0:
                                    Console.WriteLine("A sair.");
                                    break;

                                default:
                                    Console.WriteLine("Opção invalida.");
                                    break;
                            }
                        } while (escolhae != 0);

                        break;

                    case 5:
                        int escolhaf;
                        Console.Clear();

                        do
                        {
                            Console.Clear();
                            LinhaVenda.menuLinhaVenda();
                            Console.WriteLine("\n");
                            escolhaf = int.Parse(Console.ReadLine());

                            switch (escolhaf)
                            {
                                case 1:
                                    int produto;
                                    int venda;
                                    int quantidade;
                                    int contadorpes;

                                    Console.WriteLine("Digite quantas linhas de vendas quer inserir");
                                    contadorpes = int.Parse(Console.ReadLine());

                                    for (int i = 1; i <= contadorpes; i++)
                                    {
                                        Produto.listarProduto();

                                        Console.WriteLine($"Digite o id do produto da linha de venda {i}: ");
                                        produto = int.Parse(Console.ReadLine());

                                        Venda.listarVenda();

                                        Console.WriteLine($"Digite o id da venda da linha de venda {i}: ");
                                        venda = int.Parse(Console.ReadLine());

                                        Console.WriteLine($"Digite a quantidade da linha de venda {i}: ");
                                        quantidade = int.Parse(Console.ReadLine());

                                        LinhaVenda.inserirLinhaVenda(produto, venda, quantidade);
                                    }
                                    break;

                                case 2:
                                    Console.Clear();
                                    LinhaVenda.listarLinhaVenda();
                                    Console.ReadLine();
                                    break;

                                case 3:
                                    int id;
                                    Console.Clear();

                                    LinhaVenda.listarLinhaVenda();
                                    Console.WriteLine("Digite o id da linha de venda que quer remover: ");
                                    id = int.Parse(Console.ReadLine());

                                    if (LinhaVenda.removerLinhaVenda(id))
                                    {
                                        Console.WriteLine("Linha de Venda removida com sucesso.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Linha de Venda não removida.");
                                    }
                                        break;

                                case 4:
                                    int id2;
                                    Console.Clear();

                                    LinhaVenda.listarLinhaVenda();
                                    Console.WriteLine("Digite o id da linha de venda que quer editar: ");
                                    id2 = int.Parse(Console.ReadLine());

                                    if (LinhaVenda.editarLinhaVenda(id2))
                                    {
                                        Console.WriteLine("Linha de Venda editada com sucesso.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Linha de Venda não editada.");
                                    }
                                        break;

                                case 0:
                                    Console.WriteLine("A sair.");
                                    break;

                                default:
                                    Console.WriteLine("Opção invalida.");
                                    break;
                            }
                        } while (escolhaf != 0);

                        break;
                    case 0:
                        Console.WriteLine("A desligar o sistema.");
                        break;

                    default:
                        Console.WriteLine("Opção invalida.");
                        break;
                }
            } while( escolha != 0);
        }
    }
}
