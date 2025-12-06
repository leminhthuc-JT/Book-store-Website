using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace LTW_Ban_Sach.Models
{
    public class Bills
    {
        [Key]
        public int BillId { get; set; }
        [Required]
        public string Id { get; set; } //Này để liên kết với User không dùng ràng buộc khóa ngoại
        [Required]
        public DateTime CreateDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int VoucherId { get; set; }
        public virtual ICollection<DetailBills> DetailBills { get; set; }
        public virtual Vouchers Vouchers { get; set; }
    }
}