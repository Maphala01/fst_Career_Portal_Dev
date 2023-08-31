using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace fst_Career_Portal_Dev.Models
{
    public class LoginMdl
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password \"{0}\" must have {2} character", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{6,}$", ErrorMessage = "Password must contain: Minimum 8 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet, 1 Number and 1 Special Character")]
        public string Password { get; set; }

        [Display(Name = "UserRole")]
        [Required(ErrorMessage = "Please select User Role")]
        public List<UserRole_list_login> login_roleList { get; set; }
   
    }
    public class UserRole_list_login

    {

        public int Login_RoleID { get; set; }

        public string Login_RoleName { get; set; }

    }
}