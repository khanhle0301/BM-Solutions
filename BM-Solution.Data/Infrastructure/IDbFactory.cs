using System;

namespace BM_Solution.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        BmSolutionsDbContext Init();
    }
}