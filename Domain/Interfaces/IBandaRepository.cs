using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IBandaRepository
    {
        List<BandaModel> GetAll(string search = null);
        void Create(BandaModel bandaModel);
        void Delete(BandaModel bandaModel);
        public List<BandaModel> ImportList();
        public void ImportRepository();
        public void ExportList(List<BandaModel> bandaModels);
        public void exportRepository();
        public List<BandaModel> ExibirCincoUltimos();

    }
}
