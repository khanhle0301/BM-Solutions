using BM_Solution.Model.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BM_Solution.Data.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<BM_Solution.Data.BmSolutionsDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BM_Solution.Data.BmSolutionsDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //CreateUser(context);
            //CreateRole(context);
            //AddToRoles(context);
        }

        private void CreateUser(BmSolutionsDbContext context)
        {
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(new BmSolutionsDbContext()));

            var user = new AppUser()
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                FullName = "Technology Education",
            };

            manager.Create(user, "123456");
        }

        private void CreateRole(BmSolutionsDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new BmSolutionsDbContext()));

            roleManager.Create(new IdentityRole { Name = "Admin" });
            roleManager.Create(new IdentityRole { Name = "Member" });
        }

        private void AddToRoles(BmSolutionsDbContext context)
        {
            var manager = new UserManager<AppUser>(new UserStore<AppUser>(new BmSolutionsDbContext()));

            manager.AddToRoles("52d32b51-0ddf-4bb0-a61e-4da30b09187c", new string[] { "Admin" });
        }
    }
}