using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;

namespace fst_Career_Portal_Dev.Models
{
    public class RegisterMdl
    {

        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username required")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password and password do not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Role")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Role required")]
        public SelectList MobileList { get; set; }

        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name required")]
        public string LastName { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email ID required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Grade")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Only numbers are allowed.")]
        public string Grade { get; set; }

        [Display(Name = "School")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "School required")]
        public string School { get; set; }

        [Display(Name = "Address 1")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Address 1 required")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Address 2 required")]
        public string Address2 { get; set; }

        [Display(Name = "Address 3")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Address 3 required")]
        public string Address3 { get; set; }

        [Display(Name = "Province")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Province required")]
        public string Province { get; set; }
        public int Register_RoleID { get; set; }
        public string Register_RoleName { get; set; }

        public int SelectedRoleId { get; set; }
        public List<RegisterMdl> UserRoleList { get; set; }

        [Display(Name = "National ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "National ID required")]
        public string NationalID { get; set; }

        public string Gender { get; set; }
        public string SelectedGender { get; set; }

        public string SelectedGrade { get; set; }

        public bool AcceptTerms { get; set; }

    }
}