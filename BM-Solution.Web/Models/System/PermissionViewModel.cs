namespace BM_Solution.Web.Models.System
{
    public class PermissionViewModel
    {
        public int Id { get; set; }

        public string RoleId { get; set; }

        public string FunctionId { get; set; }

        public bool CanCreate { set; get; }

        public bool CanRead { set; get; }

        public bool CanUpdate { set; get; }

        public bool CanDelete { set; get; }

        public ApplicationRoleViewModel AppRole { get; set; }
    }
}