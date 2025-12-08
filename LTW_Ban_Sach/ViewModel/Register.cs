using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LTW_Ban_Sach.ViewModel
{
    public class Register
    {
        [Required(ErrorMessage = "Không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ!")]
        public string Email { get; set; }



        [Required(ErrorMessage = "Không được để trống")]
        [RegularExpression("^[a-zA-Z0-9]+$",
        ErrorMessage = "Username chỉ được chứa chữ thường, chữ hoa và số (không dấu, không khoảng trắng, không ký tự đặc biệt).")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.Password)]
        public string Password { get; set; }



        [Required(ErrorMessage = "Không được để trống")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không giống nhau.")]
        public string ConfirmPassword { get; set; }



        [Required(ErrorMessage = "Không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Số điện thoại không hợp lệ!")]
        public string PhoneNumber { get; set; }



        [Required(ErrorMessage = "Không được để trống")]
        public string Address { get; set; }
    }
}