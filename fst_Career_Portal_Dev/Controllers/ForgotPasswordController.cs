using fst_Career_Portal_Dev.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

namespace fst_Career_Portal_Dev.Controllers
{
    public class ForgotPasswordController : Controller
    {
        // GET: ForgotPassword
        public ActionResult Index(ForgotPasswordMdl model)
        {
            string buttonValue = Request.Form["ForgotPasswrd"];

            if (buttonValue == "ForgotPass")
            {
                try
                {
                    //string userName = WhoLogged_user;
                    // string username = model.username;
                    // string email = model.emailaddress;
                    // string Password = model.NewPasswordChange;
                    // string ConfirmPassword = model.NewConfirmPasswordChange;
                    // int res = 0;

                    // var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();
                    // SqlConnection sql_conn = new SqlConnection(connection);

                    // SqlCommand sql_cmd = new SqlCommand("cpMst_ForgotUserPassword_Main", sql_conn);
                    // sql_cmd.CommandType = CommandType.StoredProcedure;

                    // sql_cmd.Parameters.AddWithValue("@cp_usrNm", username);
                    // sql_cmd.Parameters.AddWithValue("@cpUsrEmail", email);
                    // sql_cmd.Parameters.AddWithValue("@cpUsrNewPsswrd", Password);
                    // sql_cmd.Parameters.AddWithValue("@cpUsrNewCPsswrd", ConfirmPassword);
                    // SqlParameter sql_param = new SqlParameter("@IsVld", SqlDbType.Int);
                    // sql_param.ParameterName = "@IsVld";
                    // sql_param.Direction = ParameterDirection.Output;
                    // sql_cmd.Parameters.Add(sql_param);

                    //res = (int)sql_param.Value;
                    string username = model.username;
                    string email = model.emailaddress;
                    string Password = model.NewPasswordChange;
                    string ConfirmPassword = model.NewConfirmPasswordChange;

                    var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();
                    SqlConnection sql_conn = new SqlConnection(connection);

                    SqlCommand sql_cmd = new SqlCommand("cpMst_ForgotUserPassword_Main1", sql_conn);
                    sql_cmd.CommandType = CommandType.StoredProcedure;

                    sql_cmd.Parameters.AddWithValue("@cp_usrNm", username);
                    sql_cmd.Parameters.AddWithValue("@cpUsrEmail", email);
                    sql_cmd.Parameters.AddWithValue("@cpUsrNewPsswrd", Password);
                    sql_cmd.Parameters.AddWithValue("@cpUsrNewCPsswrd", ConfirmPassword);

                    // Define the output parameter and add it to the command
                    SqlParameter sql_param = new SqlParameter("@IsVld", SqlDbType.Int);
                    sql_param.Direction = ParameterDirection.Output;
                    sql_cmd.Parameters.Add(sql_param);

                    sql_conn.Open();
                    sql_cmd.ExecuteNonQuery();

                    // Now you can safely retrieve the output parameter value
                    int res = (int)sql_param.Value;

                    if (res == 4)
                    {
                        TempData["PasswordChangeSuccess"] = "Password Changed...was successful";
                    }
                    if (res == 3)
                    {
                        TempData["PasswordChangeError3"] = " Error...Password and confirm password dont match";
                    }
                    if (res == 2)
                    {
                        TempData["PasswordChangeError2"] = " Error...Email Address Doesnt Exist";
                    }
                    if (res == 1)
                    {
                        TempData["PasswordChangeError1"] = " Error..Username doesnt correct";
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception )
                {
                    TempData["PasswordChangeError"] = "Password Changed error..please try again";
                    return RedirectToAction("Index");
                }
            }

            return View("Index");
        }

        public ActionResult ChangePassword(ForgotPasswordMdl model)
        {
            try
            {
                //string userName = WhoLogged_user;
                string username = model.username;
                string email = model.emailaddress;
                string Password = model.NewPasswordChange;
                string ConfirmPassword = model.NewConfirmPasswordChange;

                var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();
                SqlConnection sql_conn = new SqlConnection(connection);

                SqlCommand sql_cmd = new SqlCommand("cpMst_ForgotUserPassword_Main", sql_conn);
                sql_cmd.CommandType = CommandType.StoredProcedure;

                sql_cmd.Parameters.AddWithValue("@cp_usrNm", username);
                sql_cmd.Parameters.AddWithValue("@cpUsrEmail", email);
                sql_cmd.Parameters.AddWithValue("@cpUsrNewPsswrd", Password);
                sql_cmd.Parameters.AddWithValue("@cpUsrNewCPsswrd", ConfirmPassword);
                SqlParameter sql_param = new SqlParameter();
                sql_param.ParameterName = "@IsVld";
                sql_param.SqlDbType = SqlDbType.Int;
                sql_param.Direction = ParameterDirection.Output;
                sql_cmd.Parameters.Add(sql_param);

                int res = (int)sql_param.Value;
                sql_conn.Open();
                sql_cmd.ExecuteNonQuery();

                if (res == 4)
                {
                    TempData["PasswordChangeSuccess"] = "Password Changed...was successful";
                }
                if (res == 3)
                {
                    TempData["PasswordChangeError3"] = " Error...Password and confirm password dont match";
                }
                if (res == 2)
                {
                    TempData["PasswordChangeError2"] = " Error...Email Address Doesnt Exist";
                }
                if (res == 1)
                {
                    TempData["PasswordChangeError1"] = " Error..Username doesnt correct";
                }



                return RedirectToAction("Index");
            }
            catch (Exception )
            {
                TempData["PasswordChangeError"] = "Password Changed error..please try again";
                return RedirectToAction("Index");
            }
        }
    }
}