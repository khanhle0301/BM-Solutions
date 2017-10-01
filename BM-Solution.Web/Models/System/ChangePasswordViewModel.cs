using System.ComponentModel.DataAnnotations;

namespace BM_Solution.Web.Models.System
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 6)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}