using BM_Solution.Model.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace BM_Solution.Data
{
    public class BmSolutionsDbContext : IdentityDbContext<AppUser>
    {
        public BmSolutionsDbContext() : base("BmSolutionsConnection")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Error> Errors { set; get; }
        public DbSet<DuAn> DuAns { set; get; }
        public DbSet<ChiTietThuChi> ChiTietThuChi { set; get; }
        public DbSet<DuAnUser> DuAnUsers { set; get; }
        public DbSet<AppRole> AppRoles { set; get; }
        public DbSet<IdentityUserRole> UserRoles { set; get; }

        public static BmSolutionsDbContext Create()
        {
            return new BmSolutionsDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasKey<string>(r => r.Id).ToTable("AppRoles");
            builder.Entity<IdentityUserRole>().HasKey(i => new { i.UserId, i.RoleId }).ToTable("AppUserRoles");
            builder.Entity<IdentityUserLogin>().HasKey(i => i.UserId).ToTable("AppUserLogins");
            builder.Entity<IdentityUserClaim>().HasKey(i => i.UserId).ToTable("AppUserClaims");
        }
    }
}