using BM_Solution.Data.Infrastructure;
using BM_Solution.Data.Repositories;
using BM_Solution.Model.Models;
using BM_Solutions.Common.Exceptions;
using System.Collections.Generic;
using System;
using System.Linq;
using BM_Solutions.Common.Enums;

namespace BM_Solutions.Service
{
    public interface IDuAnService
    {
        DuAn Add(DuAn duAn);

        void Update(DuAn duAn);

        void Delete(string id);

        IEnumerable<DuAn> GetAll();

        DuAn GetById(string id);

        void UpdateProfit(ChiTietThuChi chiTietThuChi);

        void UpdateProfitEdit(ChiTietThuChi chiTietThuChi);

        void KetThucDuAn(string id);

        IEnumerable<DuAn> GetByUserId(string userId, string keyword, IEnumerable<string> role, List<StatusEnum> listStatus);

        int Count(string userId, IEnumerable<string> role);

        int Count();

        void Save();
    }

    public class DuAnService : IDuAnService
    {
        private readonly IDuAnRepository _duAnRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DuAnService(IDuAnRepository colorRepository, IUnitOfWork unitOfWork)
        {
            _duAnRepository = colorRepository;
            _unitOfWork = unitOfWork;
        }

        public DuAn Add(DuAn duAn)
        {
            if (_duAnRepository.CheckContains(x => x.Id == duAn.Id))
                throw new NameDuplicatedException("Mã dự án đã tồn tại");
            return _duAnRepository.Add(duAn);
        }

        public int Count()
        {
            return _duAnRepository.GetAll().Count();
        }

        public int Count(string userId, IEnumerable<string> role)
        {
            return _duAnRepository.Count(userId, role);
        }

        public void Delete(string id)
        {
            var duAn = _duAnRepository.GetSingleByCondition(x => x.Id == id);
            duAn.IsDelete = true;
        }

        public IEnumerable<DuAn> GetAll()
        {
            return _duAnRepository.GetAll();
        }

        public DuAn GetById(string id)
        {
            return _duAnRepository.GetSingleByCondition(x => x.Id == id);
        }

        public IEnumerable<DuAn> GetByUserId(string userId, string keyword, IEnumerable<string> role, List<StatusEnum> listStatus)
        {
            return _duAnRepository.GetByUserId(userId, keyword, role, listStatus);
        }

        public void KetThucDuAn(string id)
        {
            var duAn = _duAnRepository.GetSingleByCondition(x => x.Id == id);
            duAn.TrangThai = StatusEnum.DaKetThuc;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(DuAn duAn)
        {
            _duAnRepository.Update(duAn);
        }

        public void UpdateProfit(ChiTietThuChi chiTietThuChi)
        {
            var duAn = _duAnRepository.GetSingleByCondition(x => x.Id == chiTietThuChi.DuAnId);
            duAn.TienChiThucTe += chiTietThuChi.TienChi;
            duAn.TienThuThucTe += chiTietThuChi.TienThu;
            var loiNhuan = duAn.TienThuThucTe - duAn.TienChiThucTe;
            if (loiNhuan <= 0)
            {
                if (duAn.TienThuThucTe > 0)
                {
                    duAn.TrangThai = StatusEnum.DaCoTien;
                    duAn.LoiNhuanThucTe = 0;
                }
                else
                {
                    duAn.TrangThai = StatusEnum.DangHoatDong;
                    duAn.LoiNhuanThucTe = 0;
                }
            }
            else
            {
                if (loiNhuan > duAn.LoiNhuanDuTinh)
                {
                    duAn.TrangThai = StatusEnum.CoTheKetThuc;
                    duAn.LoiNhuanThucTe = loiNhuan;
                }
                else
                {
                    duAn.TrangThai = StatusEnum.DaCoLoiNhuan;
                    duAn.LoiNhuanThucTe = loiNhuan;
                }
            }
        }


        public void UpdateProfitEdit(ChiTietThuChi chiTietThuChi)
        {
            var duAn = _duAnRepository.GetSingleByCondition(x => x.Id == chiTietThuChi.DuAnId);
            duAn.TienChiThucTe -= chiTietThuChi.TienChi;
            duAn.TienThuThucTe -= chiTietThuChi.TienThu;
        }
    }
}