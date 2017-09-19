using System.ComponentModel.DataAnnotations;

namespace BM_Solution.Web.Models.System
{
    public class ApplicationRoleViewModel
    {
        public string Id { set; get; }

        [Required(ErrorMessage = "Bạn phải nhập tên")]
        public string Name { set; get; }

        public string Description { set; get; }
    }
}