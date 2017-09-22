using BM_Solution.Data.Infrastructure;
using BM_Solution.Data.Repositories;
using BM_Solution.Model.Models;
using System;
using System.Collections.Generic;

namespace BM_Solutions.Service
{
    public interface IChiTietThuChiService
    {
        ChiTietThuChi Add(ChiTietThuChi chiTietThuChi);

        ChiTietThuChi Delete(int id);

        IEnumerable<ChiTietThuChi> GetAll();

        IEnumerable<ChiTietThuChi> GetByDuAnId(string duaAnId);

        IEnumerable<ChiTietThuChi> GetByDuAnId(string duaAnId, string startDate, string endDate);

        void Save();
    }

    public class ChiTietThuChiService : IChiTietThuChiService
    {
        private readonly IChiTietThuChiRepository _chiTietThuChiRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChiTietThuChiService(IChiTietThuChiRepository colorRepository, IUnitOfWork unitOfWork)
        {
            _chiTietThuChiRepository = colorRepository;
            _unitOfWork = unitOfWork;
        }

        public ChiTietThuChi Add(ChiTietThuChi chiTietThuChi)
        {
            return _chiTietThuChiRepository.Add(chiTietThuChi);
        }

        public ChiTietThuChi Delete(int id)
        {
            return _chiTietThuChiRepository.Delete(id);
        }

        public IEnumerable<ChiTietThuChi> GetAll()
        {
            return _chiTietThuChiRepository.GetMulti(x => x.IsDelete == false);
        }

        public IEnumerable<ChiTietThuChi> GetByDuAnId(string duaAnId)
        {
            return _chiTietThuChiRepository.GetMulti(x => x.DuAnId == duaAnId && x.IsDelete == false, new string[] { "AppUser" });
        }

        public IEnumerable<ChiTietThuChi> GetByDuAnId(string duaAnId, string startDate, string endDate)
        {
            return _chiTietThuChiRepository.GetByDuAnId(duaAnId, startDate, endDate);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}