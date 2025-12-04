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
        public string Id { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int VoucherId { get; set; }
        public virtual ICollection<DetailBills> DetailBills { get; set; }
        public virtual Vouchers Vouchers { get; set; }
    }
}