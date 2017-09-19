using BM_Solution.Data.Infrastructure;
using BM_Solution.Data.Repositories;
using BM_Solution.Model.Models;
using BM_Solutions.Common.Exceptions;
using System.Collections.Generic;

namespace BM_Solutions.Service
{
    public interface IDuAnService
    {
        DuAn Add(DuAn duAn);

        void Update(DuAn duAn);

        DuAn Delete(string id);

        IEnumerable<DuAn> GetAll();

        IEnumerable<DuAn> GetAll(string keyword);

        DuAn GetById(string id);

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

        public DuAn Delete(string id)
        {
            var duAn = _duAnRepository.GetSingleByCondition(x => x.Id == id);
            return _duAnRepository.Delete(duAn);
        }

        public IEnumerable<DuAn> GetAll()
        {
            return _duAnRepository.GetMulti(x => x.IsDelete == false);
        }

        public IEnumerable<DuAn> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _duAnRepository.GetMulti(x => x.IsDelete == false && x.Ten.Contains(keyword));
            return GetAll();
        }

        public DuAn GetById(string id)
        {
            return _duAnRepository.GetSingleByCondition(x => x.Id == id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(DuAn duAn)
        {
            if (_duAnRepository.CheckContains(x => x.Id == duAn.Id && x.Id != duAn.Id))
                throw new NameDuplicatedException("Mã dự án đã tồn tại");
            _duAnRepository.Update(duAn);
        }
    }
}