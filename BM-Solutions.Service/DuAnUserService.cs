using BM_Solution.Data.Infrastructure;
using BM_Solution.Data.Repositories;
using BM_Solution.Model.Models;
using System.Collections.Generic;
using System;

namespace BM_Solutions.Service
{
    public interface IDuAnUserService
    {
        List<string> GetUserByDuAnId(string duAnId);

        List<string> GetDuAnByUserId(string userId);

        void Add(DuAnUser duAnUser);

        void Update(DuAnUser duAnUser);

        void DeleteAll(string duAnId, string userId);

        IEnumerable<DuAnUser> GetDuAnUserByDuAnId(string duAnId);

        IEnumerable<AppUser> GetByNotInDuAnId(string duAnId);

        void SaveChange();
    }

    public class DuAnUserService : IDuAnUserService
    {
        private readonly IDuAnUserRepository _duAnUserRepository;
        private readonly IDuAnRepository _duAnRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DuAnUserService(IDuAnUserRepository duAnUserRepository, IDuAnRepository duAnRepository,
            IUnitOfWork unitOfWork)
        {
            _duAnRepository = duAnRepository;
            _duAnUserRepository = duAnUserRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(DuAnUser duAnUser)
        {
            _duAnUserRepository.Add(duAnUser);
            var duAn = _duAnRepository.GetSingleByCondition(x => x.Id == duAnUser.DuAnId);
            duAn.TienVonBanDau += duAnUser.TienVonBanDau;
        }


        public void Update(DuAnUser duAnUser)
        {
            var _duAnUser = _duAnUserRepository.GetSingleByCondition(x => x.DuAnId == duAnUser.DuAnId && x.UserId == duAnUser.UserId);
            _duAnUser.NgayTao = duAnUser.NgayTao;
            _duAnUser.TienVonBanDau = duAnUser.TienVonBanDau;
            _duAnUser.PhanTramHoaHong = duAnUser.PhanTramHoaHong;
            var duAn = _duAnRepository.GetSingleByCondition(x => x.Id == duAnUser.DuAnId);
            duAn.TienVonBanDau += duAnUser.TienVonBanDau;
        }

        public void DeleteAll(string duAnId, string userId)
        {
            var duAnUser = _duAnUserRepository.GetSingleByCondition(x => x.DuAnId == duAnId && x.UserId == userId);
            duAnUser.IsDelete = true;
            var duAn = _duAnRepository.GetSingleByCondition(x => x.Id == duAnUser.DuAnId);
            duAn.TienVonBanDau -= duAnUser.TienVonBanDau;
        }

        public List<string> GetUserByDuAnId(string duAnId)
        {
            return _duAnUserRepository.GetUserByDuAnId(duAnId);
        }

        public void SaveChange()
        {
            _unitOfWork.Commit();
        }

        public List<string> GetDuAnByUserId(string userId)
        {
            return _duAnUserRepository.GetDuAnByUserId(userId);
        }

        public IEnumerable<DuAnUser> GetDuAnUserByDuAnId(string duAnId)
        {
            return _duAnUserRepository.GetMulti(x => x.IsDelete == false && x.DuAnId == duAnId, new string[] { "AppUser" });
        }

        public IEnumerable<AppUser> GetByNotInDuAnId(string duAnId)
        {
            return _duAnUserRepository.GetByNotInDuAnId(duAnId);
        }
    }
}