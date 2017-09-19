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
        public string DuaAnId { get; set; }

        public DateTime? NgayTao { set; get; }

        public Decimal? TienChi { set; get; }

        public Decimal? TienThu { set; get; }

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        [ForeignKey("DuaAnId")]
        public DuAn DuAn { get; set; }

        [MaxLength(100)]
        public string GhiChu { get; set; }

        public bool IsDelete { get; set; }
    }
}