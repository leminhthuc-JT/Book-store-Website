using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LTW_Ban_Sach.Models
{
    public class ImagesBook
    {

        [Key, Column(Order = 0)]
        public int BookId { get; set; }
        [Key, Column(Order = 1)]
        [Required]
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public virtual Books Books { get; set; }
    }
}