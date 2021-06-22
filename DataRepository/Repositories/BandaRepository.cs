using Domain;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataRepository
{
    public class BandaRepository : IBandaRepository
    {
        private static List<BandaModel> _listaBanda = new List<BandaModel>();

        public const string path = @"C:\Projects\AT_Git\CSharp_AT_Marcos_Cassiano\DataRepository\Repositories\Bandas.txt";

        public List<BandaModel> GetAll(string search = null)
        {
            var pesquisa = _listaBanda.Where(x =>
            x.NomeBanda.ToLower().Contains(search.ToLower())).ToList();

            return pesquisa;
        }

        public void Create(BandaModel bandaModel)
        {
            _listaBanda.Add(bandaModel);

            exportRepository();
        }

        public void Delete(BandaModel bandaModel)
        {
            _listaBanda.Remove(bandaModel);

            exportRepository();

        }

        public List<BandaModel> ImportList()
        {
            List<BandaModel> _listaBanda = new List<BandaModel>();
            
            if (File.Exists(path))
            {
                List<string> bandaArquivo = File.ReadAllLines(path).ToList();

                List<string> ArquivoCorrigido = new List<string>();

                foreach (string linha in bandaArquivo)
                {
                    if (!(string.IsNullOrEmpty(linha)))
                    {
                        ArquivoCorrigido.Add(linha);
                    }
                }

                if (ArquivoCorrigido.Any())
                {
                    foreach (string linhaCorrigida in ArquivoCorrigido)
                    {
                        List<string> trocar = linhaCorrigida.Split('|').ToList();

                        BandaModel banda = new BandaModel(Guid.Parse(trocar[0]), trocar[1], DateTime.Parse(trocar[2]), int.Parse(trocar[3]), bool.Parse(trocar[4]));

                        _listaBanda.Add(banda);
                    }
                }
            }

            if (!File.Exists(path))
            {
                File.CreateText(path);
            }

            return _listaBanda;
        }

        public void ImportRepository()
        {
            _listaBanda = ImportList();
        }

        public void ExportList(List<BandaModel> bandaModels)
        {
            using StreamWriter escrever = new StreamWriter(path, false);
            foreach (BandaModel bandaModel in bandaModels)
            {
                escrever.WriteLine(bandaModel.ToString());
            }
        }

        public void exportRepository()
        {
            ExportList(_listaBanda);
        }

        public List<BandaModel> ExibirCincoUltimos()
        {
            var bandasEncontradas = _listaBanda.TakeLast(5).ToList();
            return bandasEncontradas;
        }
    }
}
