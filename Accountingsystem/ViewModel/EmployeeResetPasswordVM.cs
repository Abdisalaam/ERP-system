using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accountingsystem.ViewModel
{
    public class EmployeeResetPasswordVM
    {
        public int EmployeeID { get; set; }
        public string UserID { get; set; }
        [Required(ErrorMessage = "يجب ادخال كلمة السر")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
       
        public string Password { get; set; }
        [Required(ErrorMessage = "يجب ادخال كلمة السر مرة اخري")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string  confPassword { get; set; }
    }
}