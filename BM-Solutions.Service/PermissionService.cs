using BM_Solution.Data.Infrastructure;
using BM_Solution.Data.Repositories;
using BM_Solution.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace BM_Solutions.Service
{
    public interface IPermissionService
    {
        ICollection<Permission> GetByFunctionId(string functionId);

        ICollection<Permission> GetByUserId(string userId);

        void Add(Permission permission);

        void DeleteAll(string duAnId);

        void SaveChange();
    }

    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionService(IPermissionRepository permissionRepository, IUnitOfWork unitOfWork)
        {
            this._permissionRepository = permissionRepository;
            this._unitOfWork = unitOfWork;
        }

        public void Add(Permission permission)
        {
            _permissionRepository.Add(permission);
        }

        public void DeleteAll(string duAnId)
        {
            _permissionRepository.DeleteMulti(x => x.DuaAnId == duAnId);
        }

        public ICollection<Permission> GetByFunctionId(string functionId)
        {
            return _permissionRepository
                .GetMulti(x => x.DuaAnId == functionId, new string[] { "AppUser" }).ToList();
        }

        public ICollection<Permission> GetByUserId(string userId)
        {
            return _permissionRepository.GetByUserId(userId);
        }

        public void SaveChange()
        {
            _unitOfWork.Commit();
        }
    }
}