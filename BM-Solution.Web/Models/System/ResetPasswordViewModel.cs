using System.ComponentModel.DataAnnotations;

namespace BM_Solution.Web.Models.System
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Id { get; set; }

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