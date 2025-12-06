using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LTW_Ban_Sach.Models
{
    public class ImageEvents
    {
        [Key, Column(Order = 0)]
        public int EventId { get; set; }
        [Key, Column(Order = 1)]
        [Required]
        public string ImageEvent { get; set; }
        public string Description { get; set; }
        public virtual Events Events { get; set; }

    }
}