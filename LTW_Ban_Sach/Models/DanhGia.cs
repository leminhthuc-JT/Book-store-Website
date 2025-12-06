using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LTW_Ban_Sach.Models
{
    public class DanhGia
    {
        [Key, Column(Order = 0)]
        public int BookId { get; set; }
        [Key, Column(Order = 1)]
        public string Id { get; set; } //Liên kết với User không dùng ràng buộc khóa ngoại
        [Required]
        public string Comment { get; set; }
        [Range(1, 5)]
        [Required]
        public int Rating { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime ReviewDate { get; set; }
        public string ImageUrl { get; set; }
        public virtual Books Books { get; set; }



    }
}