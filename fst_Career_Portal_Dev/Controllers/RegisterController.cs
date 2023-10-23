using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml.Linq;
using fst_Career_Portal_Dev.Models;
using static fst_Career_Portal_Dev.Models.RegisterMdl;

namespace fst_Career_Portal_Dev.Controllers
{

    public class RegisterController : Controller
    {
        //Data Access
        Data_Access_Layer.dba dbaLayer = new Data_Access_Layer.dba();
        // GET: Register

        [HttpGet]
        public ActionResult Index()
        {
            RegisterMdl user_role_ = new RegisterMdl();
            user_role_.UserRoleList = GetDropdownOptionsFromDatabase();

            return View(user_role_);
        }

        private List<RegisterMdl> GetDropdownOptionsFromDatabase()
        {

            var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();

            SqlConnection con = new SqlConnection(connection);

            SqlCommand cmd = new SqlCommand("SELECT cp_usrRlId, cp_usrRlNm FROM [Career_Portal_Dev_1].[dbo].[mst_cpUserRole_tbl]", con);

            con.Open();

            SqlDataReader idr = cmd.ExecuteReader();

            List<RegisterMdl> user_roles = new List<RegisterMdl>();

            if (idr.HasRows)

            {
                while (idr.Read())
                {
                    user_roles.Add(new RegisterMdl

                    {
                        Register_RoleID = Convert.ToInt32(idr["cp_usrRlId"]),

                        Register_RoleName = Convert.ToString(idr["cp_usrRlNm"]),
                    });
                }
            }
            con.Close();

            return user_roles;
        }


        [HttpPost]
        public ActionResult Index(FormCollection fc,RegisterMdl model)
        {

            if (string.IsNullOrEmpty(model.Username))
            {
                TempData["RegisterUsernameError"] = "Username is required.";
                // Other validation logic 9169315254
                return View("Index"); // Return back to the view
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                TempData["RegisterPasswordError"] = "Password is required.";
                // Other validation logic
                return View(model); // Return back to the view
            }
            if (string.IsNullOrEmpty(model.ConfirmPassword))
            {
                TempData["RegisterPasswordConfirmError"] = "Confirm Password is required.";
                // Other validation logic
                return View(model); // Return back to the view
            }
            if (string.IsNullOrEmpty(model.Email))
            {
                TempData["RegisterEmailError"] = "Email is required.";
                // Other validation logic
                return View(model); // Return back to the view
            }
            if (string.IsNullOrEmpty(model.FirstName))
            {
                TempData["RegisterFirstNameError"] = "First Name is required.";
                // Other validation logic
                return View(model); // Return back to the view
            }
            if (string.IsNullOrEmpty(model.LastName))
            {
                TempData["RegsiterLastNameError"] = "Last Name is required.";
                // Other validation logic
                return View(model); // Return back to the view
            }
            if (string.IsNullOrEmpty(model.NationalID))
            {
                TempData["RegisterNationalIDError"] = "National ID is required.";
                // Other validation logic
                return View(model); // Return back to the view
            }
            if (string.IsNullOrEmpty(model.SelectedGrade))
            {
                TempData["RegisterGradeError"] = "Grade is required.";
                // Other validation logic
                return View(model); // Return back to the view
            }
            if (string.IsNullOrEmpty(model.School))
            {
                TempData["RegisterSchoolError"] = "School is required.";
                // Other validation logic
                return View(model); // Return back to the view
            }
            if (string.IsNullOrEmpty(model.SelectedGender))
            {
                TempData["RegisterGenderError"] = "Gender is required.";
                // Other validation logic
                return View(model); // Return back to the view
            }
            int selectedRoleId = model.SelectedRoleId;

            string selectedG = model.SelectedGender;

            string selectedGrade = model.SelectedGrade;

            int res = dbaLayer.User_Registration(fc["username"], fc["password"], selectedRoleId, fc["FirstName"], fc["LastName"],
                                                  fc["Email"], selectedGrade, selectedG, fc["School"], fc["Province"], fc["NationalID"]);
            LoginMdl lrf = new LoginMdl();
            RegisterMdl user_role_ = new RegisterMdl();
            //user_role_.UserRoleList = GetDropdownOptionsFromDatabase();
            if (res == 88)
            {
                TempData["ToastMessageSuccess"] = "Successful...Your registration has been successful.";
                user_role_.UserRoleList = GetDropdownOptionsFromDatabase();
                ViewBag.Message = string.Format("Hello {0}.\\nCurrent Date and Time: {1}", fc["FirstName"], DateTime.Now.ToString());
                return RedirectToAction("Index", "Login");
            }
            else if (res == 2)
            {
                TempData["ToastMessage"] = "Error...User input email exists already.";
            }
            else if (res == 3)
            {
                TempData["ToastMessage"] = "Error...User input ID already exists.";
            }
            else if (res == 4 ) 
            {
                TempData["ToastMessage"] = "Error...Registration has gone wrong,notify the adminstrator.";
            }
            else if (res == 5)
            {
                TempData["ToastMessage"] = "Error...Registration has gone wrong,notify the adminstrator.";
            }
            else if (res == 6)
            {
                TempData["ToastMessage"] = "Error..ID citizenship is not zero (non-South Africa)";
            }
            else if (res == 7)
            {
                TempData["ToastMessage"] = "Error...Something has gone wrong...notify the adminstrator.";
            }
            else if (res == 8)
            {
                TempData["ToastMessage"] = "Error...Input ID is wrong.";
            }
            return View(user_role_);
        }
    }
}
