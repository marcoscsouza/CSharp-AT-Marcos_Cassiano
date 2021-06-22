using Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.Repositories
{
    public class ArquivoRepositorio
    {
        public const string path = @"C:\Projects\AT_Git\CSharp_AT_Marcos_Cassiano\DataRepository\Repositories\Bandas.txt";

        public List<BandaModel> ArqivoBanda()
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
                    foreach (string linha in ArquivoCorrigido)
                    {
                        List<string> trocar = linha.Split('|').ToList();

                        BandaModel banda = new BandaModel(Guid.Parse(trocar[0]), trocar[1], DateTime.Parse(trocar[2]), int.Parse(trocar[3]), bool.Parse(trocar[4]));

                        _listaBanda.Add(banda);

                    }
                }
            }
            return _listaBanda;
        }

        public void ExportarArquivo(List<BandaModel> bandaModels)
        {
            Directory.CreateDirectory(path);

            using StreamWriter ImportText = new StreamWriter(path, false);
            foreach (BandaModel bandaModel in bandaModels)
            {
                ImportText.Write(bandaModel.ToString());
            }
        }
    }
}
