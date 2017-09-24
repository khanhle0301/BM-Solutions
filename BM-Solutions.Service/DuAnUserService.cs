using BM_Solution.Data.Infrastructure;
using BM_Solution.Data.Repositories;
using BM_Solution.Model.Models;
using System.Collections.Generic;

namespace BM_Solutions.Service
{
    public interface IDuAnUserService
    {
        List<string> GetUserByDuAnId(string duAnId);

        List<string> GetDuAnByUserId(string userId);

        void Add(DuAnUser duAnUser);

        void DeleteAll(string duAnId, string userId);

        void SaveChange();
    }

    public class DuAnUserService : IDuAnUserService
    {
        private readonly IDuAnUserRepository _duAnUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DuAnUserService(IDuAnUserRepository duAnUserRepository, IUnitOfWork unitOfWork)
        {
            _duAnUserRepository = duAnUserRepository;
            _unitOfWork = unitOfWork;
        }

        public void Add(DuAnUser duAnUser)
        {
            _duAnUserRepository.Add(duAnUser);
        }

        public void DeleteAll(string duAnId, string userId)
        {
            _duAnUserRepository.DeleteMulti(x => x.DuaAnId == duAnId && x.UserId == userId);
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
    }
}