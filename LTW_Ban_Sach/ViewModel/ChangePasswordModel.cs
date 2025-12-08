using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LTW_Ban_Sach.ViewModel
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không giống nhau.")]
        public string ConfirmNewPassword { get; set; }
    }
}