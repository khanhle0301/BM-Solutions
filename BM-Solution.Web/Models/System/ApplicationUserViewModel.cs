using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BM_Solution.Web.Models.System
{
    public class AppUserViewModel
    {
        public string Id { set; get; }

        [Required(ErrorMessage = "Bạn cần nhập tên.")]
        public string FullName { set; get; }

        [Required(ErrorMessage = "Bạn cần nhập email.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không đúng.")]
        public string Email { set; get; }

        [Required(ErrorMessage = "Bạn cần nhập mật khẩu.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { set; get; }

        [Required(ErrorMessage = "Bạn cần nhập tên đăng nhập.")]
        public string UserName { set; get; }
        public bool Status { get; set; }

        public List<RoleVm> Roles { get; set; }
    }

    public class RoleVm
    {
        public string Name { set; get; }
    }
}