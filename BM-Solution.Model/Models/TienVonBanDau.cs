using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BM_Solution.Model.Models
{
    [Table("TienVonBanDaus")]
    public class TienVonBanDau
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        [StringLength(128)]
        public string UserId { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string DuAnId { get; set; }

        public long TongTien { set; get; }

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        [ForeignKey("DuAnId")]
        public DuAn DuAn { get; set; }

        public bool IsDelete { get; set; }
    }
}