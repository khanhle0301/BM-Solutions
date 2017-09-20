using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BM_Solution.Model.Models
{
    [Table("DuAnUsers")]
    public class DuAnUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string DuaAnId { get; set; }

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        [ForeignKey("DuaAnId")]
        public DuAn DuAn { get; set; }
    }
}