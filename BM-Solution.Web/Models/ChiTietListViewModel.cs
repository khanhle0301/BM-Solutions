using BM_Solution.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BM_Solution.Web.Models
{
    public class ChiTietListViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string DuAnId { get; set; }

        public DateTime NgayTao { set; get; }

        public long TienChi { set; get; }

        public long TienThu { set; get; }

        public List<string> MoreImages { set; get; }

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        [ForeignKey("DuaAnId")]
        public DuAn DuAn { get; set; }

        public bool IsDelete { get; set; }
    }
}