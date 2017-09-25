using BM_Solution.Data.Infrastructure;
using BM_Solution.Data.Repositories;
using BM_Solution.Model.Models;
using BM_Solutions.Common.ViewModel;
using System;
using System.Collections.Generic;

namespace BM_Solutions.Service
{
    public interface IChiTietThuChiService
    {
        ChiTietThuChi Add(ChiTietThuChi chiTietThuChi);

        void Delete(int id);

        IEnumerable<ChiTietThuChi> GetAll();

        IEnumerable<ChiTietThuChi> GetByDuAnId(string duaAnId);

        IEnumerable<ChiTietThuChi> GetByDuAnId(string duaAnId, string startDate, string endDate);

        IEnumerable<ChiTietThuChi> NhatKyGiaoDich(IEnumerable<string> role, string userId, string startDate, string endDate);

        IEnumerable<ChiTietThuChi> DuAnThamGia(string userId, IEnumerable<string> role);

        DateRange GetRange(string duAnId, IEnumerable<string> role = null);

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

        public void Delete(int id)
        {
            var chiTiet = _chiTietThuChiRepository.GetSingleById(id);
            chiTiet.IsDelete = true;
        }

        public IEnumerable<ChiTietThuChi> DuAnThamGia(string userId, IEnumerable<string> role)
        {
            return _chiTietThuChiRepository.DuAnThamGia(userId, role);
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

        public DateRange GetRange(string duAnId, IEnumerable<string> role = null)
        {
            return _chiTietThuChiRepository.GetRange(duAnId, role);
        }

        public IEnumerable<ChiTietThuChi> NhatKyGiaoDich(IEnumerable<string> role, string userId, string startDate, string endDate)
        {
            return _chiTietThuChiRepository.NhatKyGiaoDich(role, userId, startDate, endDate);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}