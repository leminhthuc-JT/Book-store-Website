using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;
using LTW_Ban_Sach.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace LTW_Ban_Sach.Models
{
    public class Cart
    {
        [Key, Column(Order = 0)]
        public int BookId { get; set; }
        [Key, Column(Order = 1)]
        public string Id { get; set; }
        public int Quantity { get; set; }
        public virtual Books Book { get; set; }
    }
}