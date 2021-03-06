﻿using BM_Solution.Data.Infrastructure;
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

        IEnumerable<ChiTietThuChi> GetByDuAnId(string duaAnId, DateTime startDate, DateTime endDate);

        IEnumerable<ChiTietThuChi> NhatKyGiaoDich(IEnumerable<string> role, string userId, DateTime startDate, DateTime endDate);

        DateRange GetRangeByDuAnId(string duAnId);

        DateRange GetRange();

        ChiTietThuChi GetById(int id);

        void Update(ChiTietThuChi chiTietThuChi);

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
            _chiTietThuChiRepository.Delete(id);
        }

        public IEnumerable<ChiTietThuChi> GetAll()
        {
            return _chiTietThuChiRepository.GetAll();
        }

        public IEnumerable<ChiTietThuChi> GetByDuAnId(string duaAnId)
        {
            return _chiTietThuChiRepository.GetMulti(x => x.DuAnId == duaAnId, new[] { "AppUser" });
        }

        public IEnumerable<ChiTietThuChi> GetByDuAnId(string duaAnId, DateTime startDate, DateTime endDate)
        {
            return _chiTietThuChiRepository.GetByDuAnId(duaAnId, startDate, endDate);
        }

        public ChiTietThuChi GetById(int id)
        {
            return _chiTietThuChiRepository.GetSingleById(id);
        }

        public DateRange GetRange()
        {
            return _chiTietThuChiRepository.GetRange();
        }

        public DateRange GetRangeByDuAnId(string duAnId)
        {
            return _chiTietThuChiRepository.GetRangeByDuAnId(duAnId);
        }

        public IEnumerable<ChiTietThuChi> NhatKyGiaoDich(IEnumerable<string> role, string userId, DateTime startDate, DateTime endDate)
        {
            return _chiTietThuChiRepository.NhatKyGiaoDich(role, userId, startDate, endDate);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ChiTietThuChi chiTietThuChi)
        {
            _chiTietThuChiRepository.Update(chiTietThuChi);
        }
    }
}