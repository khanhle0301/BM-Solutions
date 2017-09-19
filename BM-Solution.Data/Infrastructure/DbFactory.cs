namespace BM_Solution.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private BmSolutionsDbContext dbContext;

        public BmSolutionsDbContext Init()
        {
            return dbContext ?? (dbContext = new BmSolutionsDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}