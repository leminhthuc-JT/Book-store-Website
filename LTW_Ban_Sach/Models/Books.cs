using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace LTW_Ban_Sach.Models
{
    public class Books
    {
        [Key]
        [Required (ErrorMessage = "Không được để trống")]
        
        public int BookId { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public string BookName { get; set; }
        public string Author { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        public decimal Price { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime PublicationYear { get; set; }
        public string Publisher { get; set; }
        public string Description { get; set; }
        public int LuotMua { get; set; }
        public int CateId { get; set; }
        public string mainImage { get; set; }
        public virtual Categories Categories { get; set; }
        public virtual ICollection<DetailBills> DetailBills { get; set; }
        public virtual ICollection<ImagesBook> ImagesBooks { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
    }
}