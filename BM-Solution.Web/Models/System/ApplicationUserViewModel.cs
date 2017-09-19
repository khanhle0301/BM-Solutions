using System.Collections.Generic;

namespace BM_Solution.Web.Models.System
{
    public class AppUserViewModel
    {
        public string Id { set; get; }
        public string FullName { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string UserName { set; get; }
        public bool Status { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}