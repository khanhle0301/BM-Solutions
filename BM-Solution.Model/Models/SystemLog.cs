using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BM_Solution.Model.Models
{
    [Table("SystemLogs")]
    public class SystemLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        [Column(TypeName = "varchar")]
        public string User { get; set; }

        public DateTime NgayTao { set; get; }

        [StringLength(500)]
        public string NoiDung { set; get; }
    }
}