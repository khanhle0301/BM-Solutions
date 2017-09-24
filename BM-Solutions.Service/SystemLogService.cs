using BM_Solution.Data.Infrastructure;
using BM_Solution.Data.Repositories;
using BM_Solution.Model.Models;
using System.Collections.Generic;

namespace BM_Solutions.Service
{
    public interface ISystemLogService
    {
        SystemLog Create(SystemLog systemLog);

        void Delete(int id);

        IEnumerable<SystemLog> NhatKyHeThong(string startDate, string endDate);

        void Save();
    }

    public class SystemLogService : ISystemLogService
    {
        private readonly ISystemLogRepository _systemLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SystemLogService(ISystemLogRepository systemLogRepository, IUnitOfWork unitOfWork)
        {
            _systemLogRepository = systemLogRepository;
            _unitOfWork = unitOfWork;
        }

        public SystemLog Create(SystemLog systemLog)
        {
            return _systemLogRepository.Add(systemLog);
        }

        public void Delete(int id)
        {
            var sys = _systemLogRepository.GetSingleById(id);
            sys.IsDelete = true;
        }

        public IEnumerable<SystemLog> NhatKyHeThong(string startDate, string endDate)
        {
            return _systemLogRepository.NhatKyHeThong(startDate, endDate);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}