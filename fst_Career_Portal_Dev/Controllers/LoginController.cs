using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using fst_Career_Portal_Dev.Models;

namespace fst_Career_Portal_Dev.Controllers
{
    public class LoginController : Controller
    {
        Data_Access_Layer.dba dbaLayer = new Data_Access_Layer.dba();
        // GET: Login
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult DropDownControl()
        {
            LoginMdl lrf = new LoginMdl();
            {

                lrf.login_roleList = GetUserRoleList();

            };

            return View(lrf);

        }

        public List<UserRole_list_login> GetUserRoleList()

        {

            var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();

            SqlConnection sql_conn = new SqlConnection(connection);

            SqlCommand sql_cmd = new SqlCommand("Select * as Name From Customers", sql_conn);

            sql_conn.Open();

            SqlDataReader idr = sql_cmd.ExecuteReader();

            List<UserRole_list_login> user_roles_ = new List<UserRole_list_login>();

            if (idr.HasRows)

            {

                while (idr.Read())

                {

                    user_roles_.Add(new UserRole_list_login

                    {

                        Login_RoleID = Convert.ToInt32(idr["customerId"]),

                        Login_RoleName = Convert.ToString(idr["Name"]),

                    });

                }

            }



            sql_conn.Close();

            return user_roles_;

        }

        [HttpPost]
        public ActionResult Index(FormCollection fc,LoginMdl model)
        {
            var btnResult = fc["button"];

            if (btnResult == "Register")
            {
                return RedirectToAction("Index", "Register");
            }
            else if (btnResult == "Login")
            {
                if (string.IsNullOrEmpty(model.Username))
                {
                    TempData["LoginUsernameError"] = "Username is required.";

                    return View("Index"); // Return back to the view
                }
                if (string.IsNullOrEmpty(model.Password))
                {
                    TempData["LoginPasswordError"] = "Password is required.";

                    return View("Index"); // Return back to the view
                }
                int res = dbaLayer.User_Login(fc["username"], fc["password"]);
                LoginMdl lrf = new LoginMdl();
                if (res == 1)
                {
                    TempData["LoginSuccess"] = "Login is successfully!";
                    return RedirectToAction("Index","Dashboard");
                }
            }
            
            TempData["LoginError"] = "Username/Password doesnt exist or is wrong....";
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

    }
}