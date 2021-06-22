using DataRepository;
using Domain;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ConsoleApp
{
    class Program
    {
        public const string continuar = "Pressione qualquer tecla para exibir o menu principal... ";
        static void Main(string[] args)
        {
            string opcaoMenu;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("pt-BR");

            AtualizarDados();

            do
            {
                MenuPrincipal();
                opcaoMenu = Console.ReadLine();

                switch (opcaoMenu)
                {
                    case "1":
                        MenuInclusao();
                        break;
                    case "2":
                        MenuPesquisar();
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine("Voce escolheu sair do programa! Aguarde...");
                        Thread.Sleep(2000);
                        break;
                    default:
                        Console.WriteLine($"\aOpção inválida! Tente novamente. \n{continuar}");
                        Console.ReadKey();
                        break;
                }

            } while (opcaoMenu != "3");
        }

        static void MenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("************Gerenciador de Bandas***********");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Selecione uma das opções abaixo: ");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("1 - Incluir nova banda");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("2 - Pesquisar, Alterar e Excluir banda");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("3 - Sair do programa!");
            Console.WriteLine("--------------------------------------------");
        }
        public static void MenuInclusao()
        {
            var repoBanda = new BandaRepository();
            repoBanda.ImportRepository();

            Console.Clear();
            Console.WriteLine("**********Menu para incluir novas bandas**********");
            Console.WriteLine("Informe o nome da banda que deseja adicionar: ");
            var nomeBanda = Console.ReadLine();

            if (nomeBanda.Length < 1)
            {
                Console.WriteLine($"\aNome inválido! Dados descartados! \n{continuar}");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Informe o dia de inicio da banda no formato (dd/MM/yyyy):");
            if (!DateTime.TryParse(Console.ReadLine(), out var inicioBanda))
            {
                Console.WriteLine($"\aData inválida! Dados descartados! \n{continuar}");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Informe quantos integrantes a banda possui (Números): ");
            if (!Int32.TryParse(Console.ReadLine(), out var participantesBanda))
            {
                Console.WriteLine($"\aNumero inválido! Dados descartados! \n{continuar}");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Informe se a banda continua fazendo show (true/false): ");
            if (!bool.TryParse(Console.ReadLine(), out var fazendoShow))
            {
                Console.WriteLine($"\aValor inválido! Dados descartados! \n{continuar}");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Os dados estão corretos?");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine($"Nome da banda: {nomeBanda}");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine($"Inicio da banda: {inicioBanda:d}");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine($"Integrantes da banda: {participantesBanda}");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine($"Banda continua fazendo show: {fazendoShow}");
            Console.WriteLine("-----------------------------------------------------------");

            Console.WriteLine("1 - SIM \n2 - NÃO");
            var opcaoselecionada = Console.ReadLine();

            if (opcaoselecionada == "1")
            {
                var identificador = Guid.NewGuid();
                repoBanda.Create(new BandaModel(identificador ,nomeBanda, inicioBanda, participantesBanda, fazendoShow));

                Console.WriteLine($"Dados adicionados com sucesso! \n{continuar}");
                Console.ReadKey();
            }
            else if (opcaoselecionada == "2")
            {
                Console.WriteLine($"Todos os dados foram descartados! \n{continuar}");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"\aOpção inválida! \n{continuar}");
                Console.ReadKey();
            }
        }
        public static void MenuPesquisar()
        {
            var repoBanda = new BandaRepository();
            Console.Clear();
            repoBanda.ImportRepository();

            Console.WriteLine("As 5 ultimas bandas adicionadas:");
            for (int i = 0; i < repoBanda.ExibirCincoUltimos().Count; i++)
            {
                Console.WriteLine($"{i} - {repoBanda.ExibirCincoUltimos()[i].NomeBanda}");
            }
            
            Console.WriteLine("\n**********Menu para pesquisar bandas por nome**********");
            Console.WriteLine("Qual banda deseja pesquisar?");
            Console.WriteLine("Precione a tecla ENTER para listar todas as bandas.");

            var pesquisarBanda = Console.ReadLine();
            var bandasEncontradas = repoBanda.GetAll(pesquisarBanda);

            if (bandasEncontradas.Count > 0)
            {
                Console.WriteLine("Selecione abaixo uma das opções para visualizar os dados das bandas encontradas: ");
                for (var i = 0; i < bandasEncontradas.Count; i++)
                    Console.WriteLine($"{i} - {bandasEncontradas[i].NomeBanda}");

                if (!ushort.TryParse(Console.ReadLine(), out var EscolherI) || EscolherI >= bandasEncontradas.Count)
                {
                    Console.WriteLine($"\aOpção inválida! \n{continuar}");
                    Console.ReadKey();
                    return;
                }

                Console.Clear();
                var bandaNomeEscolhido = bandasEncontradas[EscolherI].NomeBanda;
                Console.WriteLine($"\aBanda selecionada: {bandaNomeEscolhido}.\n");
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine("***Escolha uma das opções abaixo***: ");
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine($"1 - Exibir detalhes da banda {bandaNomeEscolhido}");
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine($"2 - Alterar detalhes da banda {bandaNomeEscolhido}");
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine($"3 - Excluir banda {bandaNomeEscolhido}");
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine("4 - Voltar ao menu principal");
                Console.WriteLine("-----------------------------------------------------------");

                var opcaoselecionada = Console.ReadLine();

                if (opcaoselecionada == "1")
                {
                    if (EscolherI < bandasEncontradas.Count)
                    {
                        var banda = bandasEncontradas[EscolherI];

                        Console.WriteLine("Dados da banda:");
                        Console.WriteLine("-----------------------------------------------------------");
                        Console.WriteLine($"Identificador único: {banda.NomeBanda}");
                        Console.WriteLine("-----------------------------------------------------------");
                        Console.WriteLine($"Nome da banda: {banda.NomeBanda}");
                        Console.WriteLine("-----------------------------------------------------------");
                        Console.WriteLine($"Data da criação: {banda.InicioBanda:d}");
                        Console.WriteLine("-----------------------------------------------------------");
                        Console.WriteLine($"Integrantes da banda: {banda.ParticipantesBanda}");
                        Console.WriteLine("-----------------------------------------------------------");
                        Console.WriteLine($"Banda continua na ativa: {banda.FazendoShow}");
                        Console.WriteLine("-----------------------------------------------------------");

                        var diasDoShowDeAniversario = BandaModel.ShowDeAniversario(banda.InicioBanda);
                        var anosNaAtiva = BandaModel.AnosDeCarreira(banda.InicioBanda);
                        Console.WriteLine(BandaModel.MensagemShowDeAniversario(diasDoShowDeAniversario, anosNaAtiva));

                        Console.Write(continuar);
                        Console.ReadKey();
                    }
                }

                else if (opcaoselecionada == "2")
                {
                    if (EscolherI < bandasEncontradas.Count)
                    {
                        var banda = bandasEncontradas[EscolherI];

                        Console.WriteLine($"Deseja alterar o nome da banda {banda.NomeBanda}");
                        Console.WriteLine("1 - SIM \n2 - NÃO");
                        var opcaoNome = Console.ReadLine();

                        if (opcaoNome == "1")
                        {
                            Console.WriteLine("Digite o nome que deseja:");
                            var nomeDiferente = Console.ReadLine();
                            banda.NomeBanda = nomeDiferente;
            
                            repoBanda.exportRepository();
                            Console.WriteLine("\aNome da banda alterado com sucesso!");
                            Console.ReadKey();
                        }
                        else if (opcaoNome == "2")
                        {
                            Console.WriteLine($"Nome da banda não alterado! \n{banda.NomeBanda}!");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine($"\aOpção inválida! \n{continuar}");
                            Console.ReadKey();
                            return;
                        }

                        Console.WriteLine($"Deseja alterar a quantidade de participantes da banda? {banda.ParticipantesBanda}");
                        Console.WriteLine("1 - SIM \n2 - NÃO");
                        var opcaoIntegrantes = Console.ReadLine();

                        if (opcaoIntegrantes == "1")
                        {
                            Console.WriteLine("Digite a quantidade departicipantes atual:");
                            if (!Int32.TryParse(Console.ReadLine(), out var participantesDiferente))
                            {
                                Console.WriteLine($"\aNumero inválido! Dados descartados!\n{continuar}");
                                Console.ReadKey();
                                return;
                            }
                            banda.ParticipantesBanda = participantesDiferente;
                            repoBanda.exportRepository();
                            Console.WriteLine("\aQuantidade de participantes da banda alterado com sucesso!");
                        }
                        else if (opcaoIntegrantes == "2")
                        {
                            Console.WriteLine($"Quantidade de participantes da banda Não alterada! \nBanda possui {banda.ParticipantesBanda} integrantes.");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine($"\aOpção inválida!\n{continuar}");
                            Console.ReadKey();
                            return;
                        }
                        Console.WriteLine("Alterações concluidas!");
                    }
                }

                else if (opcaoselecionada == "3")
                {
                    if (EscolherI < bandasEncontradas.Count)
                    {
                        var banda = bandasEncontradas[EscolherI];

                        Console.WriteLine($"Tem certeza que deseja EXCLUIR a banda: {banda.NomeBanda}");
                        Console.WriteLine("1 - SIM \n2 - NÃO");
                        var opcaoDeletar = Console.ReadLine();

                        if (opcaoDeletar == "1")
                        {
                            repoBanda.Delete(banda);
                            Console.WriteLine("\aExcluido com sucesso!");
                            Console.ReadKey();

                        }
                        else if (opcaoDeletar == "2")
                        {
                            Console.WriteLine("Banda não será excluida!");
                            Console.ReadKey();

                        }
                        else
                        {
                            Console.WriteLine($"\aOpção inválida!");
                            Console.ReadKey();
                        }


                    }
                    else
                    {
                        Console.WriteLine($"\aOpção inválida!");
                        Console.ReadKey();
                    }
                }

                else if(opcaoselecionada == "4")
                {
                    Console.WriteLine($"\a {continuar}");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Console.WriteLine($"\aOpção inválida!\n{continuar}");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine($"\aNão foi encontrada nenhuma banda! \n{continuar}");
                Console.ReadKey();
            }
        }

        public static void AtualizarDados()
        {
            var repoBanda = new BandaRepository();

            if (repoBanda.ImportList().Any())
            {
                repoBanda.ImportRepository();
                Console.WriteLine("Importando dados do repositorio externo...");
                Thread.Sleep(2000);
                Console.WriteLine("\aDados importados!");
                Console.WriteLine("Precione qualquer tecla para continuar.");
                Console.ReadKey();

            }
            else
            {
                Console.WriteLine("Importando dados do repositorio externo...");
                Thread.Sleep(2000);
                Console.WriteLine("\aSem dados!");
                Console.WriteLine("Precione qualquer tecla para continuar.");
                Console.ReadKey();
            }
        }
    }
}

