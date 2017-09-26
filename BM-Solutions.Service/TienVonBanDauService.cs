using BM_Solution.Data.Infrastructure;
using BM_Solution.Data.Repositories;
using BM_Solution.Model.Models;

namespace BM_Solutions.Service
{
    public interface ITienVonBanDauService
    {
        TienVonBanDau Create(TienVonBanDau tienVonBanDau);

        void Delete(int id);

        void Save();
    }

    public class TienVonBanDauService : ITienVonBanDauService
    {
        private readonly ITienVonBanDauRepository _TienVonBanDauRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TienVonBanDauService(ITienVonBanDauRepository TienVonBanDauRepository, IUnitOfWork unitOfWork)
        {
            _TienVonBanDauRepository = TienVonBanDauRepository;
            _unitOfWork = unitOfWork;
        }

        public TienVonBanDau Create(TienVonBanDau tienVonBanDau)
        {
            return _TienVonBanDauRepository.Add(tienVonBanDau);
        }

        public void Delete(int id)
        {
            var sys = _TienVonBanDauRepository.GetSingleById(id);
            sys.IsDelete = true;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}