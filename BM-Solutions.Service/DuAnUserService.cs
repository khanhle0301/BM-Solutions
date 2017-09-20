using BM_Solution.Data.Infrastructure;
using BM_Solution.Data.Repositories;
using BM_Solution.Model.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BM_Solutions.Service
{
    public interface IDuAnUserService
    {
        List<string> GetUserByDuAnId(string duAnId);
        List<DuAnUser> GetDuAnByUserId(string userId);
        void Add(DuAnUser duAnUser);

        void DeleteAll(string duAnId);
        void SaveChange();
    }

    public class DuAnUserService : IDuAnUserService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DuAnUserService(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork)
        {
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(DuAnUser duAnUser)
        {
            _permissionRepository.Add(duAnUser);
        }

        public void DeleteAll(string duAnId)
        {
            _permissionRepository.DeleteMulti(x => x.DuaAnId == duAnId);
        }

        public List<string> GetUserByDuAnId(string duAnId)
        {
            return _permissionRepository.GetUserByDuAnId(duAnId);
        }

        public void SaveChange()
        {
            _unitOfWork.Commit();
        }

        List<DuAnUser> IDuAnUserService.GetDuAnByUserId(string userId)
        {
            return _permissionRepository.GetDuAnByUserId(userId);
        }
    }
}