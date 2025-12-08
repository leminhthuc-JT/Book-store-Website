using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LTW_Ban_Sach.ViewModel
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Không được để trống")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}