using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BM_Solution.Model.Models
{
    [Table("ChiTietThuChis")]
    public class ChiTietThuChi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string DuAnId { get; set; }

        public DateTime NgayTao { set; get; }

        public long TienChi { set; get; }

        public long TienThu { set; get; }

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        [ForeignKey("DuAnId")]
        public DuAn DuAn { get; set; }

        public bool IsDelete { get; set; }
    }
}