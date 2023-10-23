using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fst_Career_Portal_Dev.Models
{
    public class ForgotPasswordMdl
    {
        public string NewPasswordChange { get; set; }
        public string NewCurrentPasswordChange { get; set; }
        public string NewConfirmPasswordChange { get; set; }
        public string username { get; set; }
        public string emailaddress { get; set; }

    }
}