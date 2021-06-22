using DataRepository;
using Domain;
using Domain.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        public const string continuar = "Pressione qualquer tecla para exibir o menu principal... ";
        public const string path = @"C:\Projects\AT_Git\CSharp_AT_Marcos_Cassiano\DataRepository\Repositories\Bandas.txt";
        private IBandaRepository _repositorio;

        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger, IBandaRepository repository)
        {
            _logger = logger;
            _repositorio = repository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string opcaoMenu;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("pt-BR");

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
                        MenuAlteracao();
                        break;
                    case "3":
                        MenuExclusao();
                        break;
                    case "4":
                        MenuPesquisa();
                        break;
                    case "5":
                        Console.Clear();
                        Console.WriteLine("Voce escolheu sair do programa! Aguarde...");
                        break;
                    default:
                        Console.WriteLine($"Opção inválida! Tente novamente. \n{continuar}");
                        Console.ReadKey();
                        break;
                }

            } while (opcaoMenu != "5");
        }
        void MenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("************Gerenciador de Bandas***********");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Selecione uma das opções abaixo: ");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("1 - Incluir nova banda");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("2 - Alterar uma banda");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("3 - Excluir uma banda");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("4 - Pesquisar bandas");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("5 - Sair do programa");
            Console.WriteLine("--------------------------------------------");
        }
        public void MenuInclusao()
        {
            var repoBanda = new BandaRepository();

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

                repoBanda.Create(new BandaModel(identificador, nomeBanda, inicioBanda, participantesBanda, fazendoShow));

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
        void MenuAlteracao()
        {
            var repoBanda = new BandaRepository();

            Console.Clear();
            Console.WriteLine("**********Menu para alterar bandas**********");
        }
        void MenuExclusao()
        {
            var repoBanda = new BandaRepository();

            Console.Clear();
            Console.WriteLine("**********Menu para excluir bandas**********");
        }
        void MenuPesquisa()
        {
            var repoBanda = new BandaRepository();

            Console.Clear();
            Console.WriteLine("**********Menu para pesquisar por bandas**********");

            //StreamReader sr;
            //if (File.Exists(path) == true)
            //{
            //    Console.WriteLine("Id\t\t\t\tBanda\tInicio\tParticipantes\tAtivos");
            //    sr = File.OpenText(path);
            //    string linha;
            //    while ((linha = sr.ReadLine()) != null)
            //    {
            //        Console.WriteLine(linha);
            //    }
            //    sr.Close();
            //}
            //else
            //{
            //    Console.WriteLine("Não existe nenhum ficheiro.");
            //}

            Console.WriteLine("Qual banda deseja pesquisar?");

            string[] textoListaLeitura = File.ReadAllLines(path);
            foreach (string texto in textoListaLeitura)
                Console.WriteLine(texto);

            var pesquisarBanda = Console.ReadLine();
            var bandasEncontradas = repoBanda.GetAll(pesquisarBanda);
            //var bandasEncontradas = repoBanda.listaTexto(pesquisarBanda); 

            if (bandasEncontradas.Count > 0)
            {
                Console.WriteLine("Selecione abaixo uma das opções para visualizar os dados das bandas encontradas: ");
                for (var i = 0; i < bandasEncontradas.Count; i++)
                    Console.WriteLine($"{i} - {bandasEncontradas[i].NomeBanda}");

                if (!ushort.TryParse(Console.ReadLine(), out var EscolherI) || EscolherI >= bandasEncontradas.Count)
                {
                    Console.WriteLine($"Opção inválida! {continuar}");
                    Console.ReadKey();
                    return;
                }

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

                    var dataAtual = DateTime.Now.Year;
                    var calculoInicio = dataAtual - banda.InicioBanda.Year;


                    Console.WriteLine(calculoInicio);

                    Console.Write(continuar);
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine($"Não foi encontrada nenhuma banda! \n{continuar}");
                Console.ReadKey();
            }
        }

        void ExibirUltimos()
        {
            StreamReader sr;
            if (File.Exists(path) == true)
            {
                Console.WriteLine("Id\t\t\t\tBanda\tInicio\tParticipantes\tAtivos");
                sr = File.OpenText(path);
                string linha = "";
                while ((linha = sr.ReadLine()) != null)
                {
                    Console.WriteLine(linha);
                }
                sr.Close();
            }
            else
            {
                Console.WriteLine("Não existe nenhum ficheiro.");
            }
            Console.ReadLine();

        }
    }
}
