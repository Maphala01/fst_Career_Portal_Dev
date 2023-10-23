using fst_Career_Portal_Dev.Data_Access_Layer;
using fst_Career_Portal_Dev.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Web;
using System.Web.DynamicData;
using System.Web.Helpers;
using System.Web.Mvc;

namespace fst_Career_Portal_Dev.Controllers
{
    public class ProfileController : Controller
    {
        string WhoLogged_user = dba.WhoLogged_usrnm;
        string WhoLogged_pss = dba.WhoLogged_pwd;
        Data_Access_Layer.dba dbaLayer = new Data_Access_Layer.dba();
        //int flag = 0;
        //UserProfileMdl learner_opp_;
        // GET: Profile
        public ActionResult Index(UserProfileMdl model)
        {
                        var emptyDocumentList = new List<UserProfileMdl>();
            var documentSelectList = new SelectList(emptyDocumentList, "Profile_DocumentID", "Profile_DocumentName");

            UserProfileMdl Profiledata_ = new UserProfileMdl();
            string userName = WhoLogged_user;
            DeleteItem(userName);

            if (!string.IsNullOrEmpty(userName))
            {
                Profiledata_ = GetDataFromDatabase(userName);
                Profiledata_.ProvinceList = GetDropdownOptionsFromDatabase_Province();
                Profiledata_.DocumentList = GetDropdownOptionsFromDatabase_DocType(WhoLogged_user);
                Profiledata_.RecentInterestList = GetLearnerInterest(WhoLogged_user);
                // Only fetch UserDocuments if userData has been successfully populated
                if (Profiledata_ != null)
                {
                    Profiledata_.UserDocuments = GetDocsFromDatabase(WhoLogged_user);

                    UserProfileMdl viewModels = new UserProfileMdl
                    {
                        Opportunities = LearnerOpportunities(),
                        //RecentApplication = GetLearnerApplication(WhoLogged_user)
                    };

                    // Update the properties of Profiledata_ with values from viewModels
                    Profiledata_.Opportunities = viewModels.Opportunities;
                    Profiledata_.RecentApplication = viewModels.RecentApplication;
                }
            }

            return View(Profiledata_);
        }
        public ActionResult ChangePassword(UserProfileMdl model)
        {
            try
            {
                string userName = WhoLogged_user;
                string password = model.NewPasswordChange;

                var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();
                SqlConnection sql_conn = new SqlConnection(connection);

                SqlCommand sql_cmd = new SqlCommand("cpMst_ForgotUserPassword", sql_conn);
                sql_cmd.CommandType = CommandType.StoredProcedure;

                sql_cmd.Parameters.AddWithValue("@cp_usrNm", userName);
                sql_cmd.Parameters.AddWithValue("@cpUsrPsswrd", password);

                sql_conn.Open();
                sql_cmd.ExecuteNonQuery();

                TempData["PasswordChangeSuccess"] = "Password Changed...was successful";

                return RedirectToAction("Index");
            }
            catch (Exception )
            {
                TempData["PasswordChangeError"] = "Password Changed error..please try again";
                return RedirectToAction("Index");
            }
        }

        public ActionResult DeleteItem(string user)
        {
            int id;
            if (int.TryParse(Request.Form["OpportunityId"], out id))
            {
                // Assuming you have a connection string in your configuration
                string connectionString = ConfigurationManager.ConnectionStrings["db_connection"].ConnectionString;

                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Define your SQL command to delete the record
                    string sql = "DELETE FROM mst_cpUserInterest WHERE cp_usrInterestId = @OpportunityId";

                    // Create the SQL command with parameters
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Add the OpportunityId parameter
                        command.Parameters.AddWithValue("@OpportunityId", id);

                        // Open the database connection
                        connection.Open();

                        // Execute the SQL command to delete the record
                        command.ExecuteNonQuery();
                    }
                }
                TempData["InterestDeletionSuccess"] = "Interest deletion...was successful";
            }
            //else
            //{
            //    TempData["InterestDeletionError"] = "Interest deletion...contact administrator";
            //}

            return RedirectToAction("Index");
        }

        private List<UserProfileMdl> GetLearnerInterest(string UserName)
        {
            DataSet ds = new DataSet();
            var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();
            SqlConnection sql_conn = new SqlConnection(connection);

            SqlCommand sql_cmd = new SqlCommand("cpMst_spGetUserInterests", sql_conn);
            sql_cmd.CommandType = CommandType.StoredProcedure;
            sql_cmd.Parameters.AddWithValue("@cpUsrNm", UserName);

            sql_conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sql_cmd);
            da.Fill(ds);

            List<UserProfileMdl> userApp_ = new List<UserProfileMdl>();
            //int OppID = 1;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                UserProfileMdl uobj = new UserProfileMdl();
                string Description = row["cp_Description"].ToString();
                int OpportunityID = Convert.ToInt32(row["cp_usrInterestId"]);
                // Assuming your DashboardMdl properties have the same names as columns in the database
                uobj.ApplicationDesc = Description;
                uobj.ApplicationAppliedDate = Convert.ToDateTime(row["cp_DateApplied"]);
                //uobj.ApplicationDesc = row["cp_Description"].ToString();
                //uobj.ApplicationType = row["cp_Type"].ToString();
                uobj.OpportunityId = OpportunityID;

                userApp_.Add(uobj);
            }

            sql_conn.Close();
            return userApp_;
        }


        private List<UserProfileMdl> LearnerOpportunities()
        {
            DataSet ds = new DataSet();
            var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();
            SqlConnection sql_conn = new SqlConnection(connection);

            SqlCommand sql_cmd = new SqlCommand("cpMst_GetDashInformation", sql_conn);

            sql_conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sql_cmd);
            da.Fill(ds);

            List<UserProfileMdl> opplist_ = new List<UserProfileMdl>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                UserProfileMdl uobj = new UserProfileMdl();
                uobj.PostedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["cp_PostedDate"]);
                uobj.Province = ds.Tables[0].Rows[i]["cp_Province"].ToString();
                uobj.Description = ds.Tables[0].Rows[i]["cp_Description"].ToString();
                uobj.Type = ds.Tables[0].Rows[i]["cp_Type"].ToString();
                uobj.Status = ds.Tables[0].Rows[i]["cp_Status"].ToString();

                opplist_.Add(uobj);
            }
            sql_conn.Close();
            return opplist_;
        }

        private List<UserProfileMdl> GetDocsFromDatabase(string loggedUser)
        {
            var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();

            using (SqlConnection sql_conn = new SqlConnection(connection))
            {
                SqlCommand sql_cmd = new SqlCommand("cpMst_GetDocsFromDatabase", sql_conn);
                sql_cmd.CommandType = CommandType.StoredProcedure;
                sql_cmd.Parameters.AddWithValue("@UserName", loggedUser);

                sql_conn.Open();

                using (SqlDataReader idr = sql_cmd.ExecuteReader())
                {
                    List<UserProfileMdl> documents_list = new List<UserProfileMdl>();

                    if (idr.HasRows)
                    {
                        while (idr.Read())
                        {
                            string fileType = Convert.ToString(idr["fileType"]);
                            string fileExtension = Path.GetExtension(fileType);
                            string fileTypeName = Convert.ToString(idr["fileTypeName"]);
                            int documentId = Convert.ToInt32(idr["cp_documentId"]);
                            documents_list.Add(new UserProfileMdl
                            {
                                FileName = Convert.ToString(idr["docFileName"]),
                                FileType = Convert.ToString(idr["fileType"]),
                                FileContent = (byte[])idr["fileContent"],
                                FileExtension = fileExtension,
                                FileTypeName = fileTypeName,
                                DocumentId = documentId
                            });
                        }
                    }

                    return documents_list;
                }
            }
        }

        public ActionResult DeleteDocument(int documentId)
        {
            try
            {
                int DocID = documentId;
                string connectionString = ConfigurationManager.ConnectionStrings["db_connection"].ConnectionString;

                // Create a connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Define your SQL command to delete the record
                    string sql = "DELETE FROM mst_cpusrDocs WHERE cp_documentId = @documentId";

                    // Create the SQL command with parameters
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Add the OpportunityId parameter
                        command.Parameters.AddWithValue("@documentId", documentId);

                        // Open the database connection
                        connection.Open();

                        // Execute the SQL command to delete the record
                        command.ExecuteNonQuery();
                    }
                }
                TempData["DocDeleteSuccess"] = "Document Delete...was successful";
            //else
            //{
            //    TempData["InterestDeletionError"] = "Interest deletion...contact administrator";
            //}

            return RedirectToAction("Index");

            }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during the deletion process.
                    TempData["DocDeleteError"] = "An error occurred while deleting the document: " + ex.Message;

                // You can choose to redirect the user to an error page or another appropriate action.
                return RedirectToAction("Index"); // Replace "Error" with the actual action name.
            }
            }



        private List<UserProfileMdl> GetDropdownOptionsFromDatabase_Province()
        {

            var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();

            SqlConnection con = new SqlConnection(connection);

            SqlCommand cmd = new SqlCommand("SELECT cp_provinceID, cp_provinceName FROM [Career_Portal_Dev_1].[dbo].[mst_cpProvinces]", con);

            con.Open();

            SqlDataReader idr = cmd.ExecuteReader();

            List<UserProfileMdl> province_list = new List<UserProfileMdl>();

            if (idr.HasRows)

            {
                while (idr.Read())
                {
                    province_list.Add(new UserProfileMdl

                    {
                        Profile_ProvinceID = Convert.ToInt32(idr["cp_provinceID"]),

                        Profile_ProvinceName = Convert.ToString(idr["cp_provinceName"]),
                    });
                }
            }
            con.Close();

            return province_list;
        }
        private UserProfileMdl GetDataFromDatabase(string User)
        {
            UserProfileMdl userData = new UserProfileMdl();
            if (User == null)
            {
                // Return an error view or appropriate response
                RedirectToAction("Index", "Login");
            }
            else if (User.Length > 1)
            {

                var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();
                SqlConnection sql_conn = new SqlConnection(connection);

                SqlCommand sql_cmd = new SqlCommand("cpMst_GetUserProfile", sql_conn);
                sql_cmd.CommandType = CommandType.StoredProcedure;

                sql_cmd.Parameters.AddWithValue("@UserName", User);

                sql_conn.Open();

                SqlDataReader idr = sql_cmd.ExecuteReader();
                if (idr.HasRows)


                {
                    while (idr.Read())
                    {
                        //int fullNameIndex = idr.GetOrdinal("FullName");
                        string Province = Convert.ToString(idr["cp_provinceName"]);
                        //userData.Fullname = idr.GetString(fullNameIndex);
                        int fullNameIndex = 1;
                        userData.About = idr["cp_usrAbout"].ToString();
                        userData.Fullname = idr[fullNameIndex].ToString();
                        userData.School = idr["cp_usrCrrntSchl"].ToString();
                        //userData.Grade = Convert.ToInt32(idr["cp_usrCrrntGrd"]);
                        userData.Grade = idr["cp_usrCrrntGrd"].ToString();
                        userData.NationalID = idr["cp_usrIdNo"].ToString();
                        userData.Province = Province;
                        //userData.Profile_ProvinceID = (int)idr["Profile_ProvinceID"];
                        userData.Address1 = idr["cp_usrAddrss1"].ToString();
                        userData.Address2 = idr["cp_usrAddrss2"].ToString();
                        userData.Address3 = idr["cp_usrAddrss3"].ToString();
                        userData.ContactNo = idr["cp_usrContactNo"].ToString();
                        userData.emailAddress = idr["cp_usrEmlAddrss"].ToString();
                    }
                }
            }

            return userData;
        }

        private List<UserProfileMdl> GetDropdownOptionsFromDatabase_DocType(string loggedUser)
        {
            var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();

            using (SqlConnection sql_conn = new SqlConnection(connection))
            {
                SqlCommand sql_cmd = new SqlCommand("cpMst_GetUserDocs", sql_conn);
                sql_cmd.CommandType = CommandType.StoredProcedure;

                sql_cmd.Parameters.AddWithValue("@loggedUser", loggedUser); // Use loggedUser instead of User

                sql_conn.Open();

                SqlDataReader idr = sql_cmd.ExecuteReader();

                List<UserProfileMdl> document_list = new List<UserProfileMdl>();

                if (idr.HasRows)
                {
                    while (idr.Read())
                    {
                        document_list.Add(new UserProfileMdl
                        {
                            Profile_DocumentID = Convert.ToInt32(idr["fileTypeID"]),
                            Profile_DocumentName = Convert.ToString(idr["fileTypeName"]),
                        });
                    }
                }

                idr.Close(); // Close the data reader
                sql_conn.Close();

                return document_list;
            }
        }


        //public ActionResult SaveChanges(FormCollection fc, UserProfileMdl model)
        public ActionResult SaveChanges(UserProfileMdl e, string submitButton)
        {
            UserProfileMdl Profiledata_ = new UserProfileMdl();
            //int flag = 0;
            if (string.IsNullOrEmpty(e.Fullname))
            {
                TempData["EditFullnameError"] = "Full Name is required.";
                // Other validation logic 9169315254
                return View("Index"); // Return back to the view
            }
            
            if (string.IsNullOrEmpty(e.About))
            {
                TempData["EditAboutError"] = "About is required.";
                // Other validation logic 9169315254
                return View("Index"); // Return back to the view
            }
            if (string.IsNullOrEmpty(e.School))
            {
                TempData["EditSchoolError"] = "Current School is required.";
                // Other validation logic 9169315254
                return View("Index"); // Return back to the view
            }
            //if (e.Grade.ToString().Length == 0 || e.Grade == 0)
            if (string.IsNullOrEmpty(e.Grade) || (int.TryParse(e.Grade, out int gradeValue) && gradeValue == 0))

            {
                Profiledata_ = GetDataFromDatabase(WhoLogged_user);
                TempData["EditGradeError"] = "Current Grade is required/Grade can't be zero.";
                // Other validation logic 9169315254
                return View("Index"); // Return back to the view
            }
            if (string.IsNullOrEmpty(e.Address1))
            {
                TempData["EditAddress1Error"] = "Address 1 is required.";
                // Other validation logic 9169315254
                return View("Index"); // Return back to the view
            }
            if (string.IsNullOrEmpty(e.Address2))
            {
                TempData["EditAddress2Error"] = "Address 2 is required.";
                // Other validation logic 9169315254
                return View("Index"); // Return back to the view
            }
            if (string.IsNullOrEmpty(e.Address3))
            {
                TempData["EditAddress3Error"] = "Address 3 is required.";
                // Other validation logic 9169315254
                return View("Index"); // Return back to the view
            }
            if (string.IsNullOrEmpty(e.ContactNo))
            {
                TempData["EditAPhoneError"] = "Contact No is required.";
                // Other validation logic 9169315254
                return View("Index"); // Return back to the view
            }
            if (string.IsNullOrEmpty(e.emailAddress))
            {
                TempData["EditEmailAddressError"] = "Email Address is required.";
                // Other validation logic 9169315254
                return View("Index"); // Return back to the view
            }

            UserProfileMdl userData = new UserProfileMdl();
            int SelectedProvinceId = e.SelectedProvinceId;
            if (string.IsNullOrEmpty(e.NationalID))
            {

                TempData["EditNationalIDError"] = "National ID is required.";
                
                return View("Index"); // Return back to the view

            }
            else if (submitButton == "updateProfile")
            {
                int res = dbaLayer.SaveUserChanges(e, WhoLogged_user, SelectedProvinceId);
                
                 userData = GetDataFromDatabase(WhoLogged_user);
                userData.DocumentList = GetDropdownOptionsFromDatabase_DocType(WhoLogged_user);
                if (res != -1)
                {
                    userData.ProvinceList = GetDropdownOptionsFromDatabase_Province();
                    userData.DocumentList = GetDropdownOptionsFromDatabase_DocType(WhoLogged_user);
                    userData.UserDocuments = GetDocsFromDatabase(WhoLogged_user);
                    userData.Opportunities = LearnerOpportunities();
                    userData.RecentInterestList = GetLearnerInterest(WhoLogged_user);
                    TempData["ProfileUpdated"] = "Profile updated successfully!";

                    return View("Index", userData);
                   

                }
                TempData["ErrorMessage"] = "Profile updating was not successful!";

                return View("Index");
                
            }

            return RedirectToAction("Index");

            //Currently using this one--------------------------------------------

            //UserProfileMdl userP = new UserProfileMdl
            //{
            //    Fullname = formCollection["Fullname"],

            //    School = formCollection["School"],
            //    Grade = Convert.ToInt32(formCollection["Grade"]),
            //    Province = formCollection["Province"],
            //    emailAddress = formCollection["emailAddress"],
            //    ContactNo = formCollection["ContactNo"],
            //    // Add other properties from the formCollection as needed
            //};

            //int res = dbaLayer.SaveUserChanges(userP);

            //if (res == 88)
            //{
            //    // Redirect to another action and controller
            //    return RedirectToAction("Index", "Dashboard");
            //}
            //return View();

            //Currently using this one--------------------------------------------
        }

        //public ActionResult ProfileDocuments()
        //{
        //    List<UserProfileMdl> userDocuments = GetUserDocumentsFromDatabase(); // Implement this method to get user documents
        //    var model = new UserProfileMdl
        //    {
        //        UserDocuments = userDocuments
        //    };

        //    return View(model);
        //}

        public ActionResult UploadDocuments(UserProfileMdl model,IEnumerable<HttpPostedFileBase> fileUpload)
        {
            foreach (var file in fileUpload)
            {
                if (file != null && file.ContentLength > 0)
                {
                    // Save the file and related information to the database
                    
                   dbaLayer.SaveUserDocumentToDatabase(file.FileName, file.ContentType, file.InputStream, WhoLogged_user, model.SelectedDocsId); // Implement this method

                }
            }

            // Redirect back to the profile page after uploading
            //return RedirectToAction("Profile");
            TempData["FileSaved"] = "Document(s) successfully uploaded!";
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult SubmitInterests(List<int> selectedOpportunities)
        {
            if (selectedOpportunities != null && selectedOpportunities.Count <= 5)
            {
                foreach (var opportunityId in selectedOpportunities)
                {
                    // Save the selected opportunity to the database
                    // Example:
                    // dbaLayer.SaveSelectedOpportunity(opportunityId);
                }
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "You can only select up to 5 opportunities.";
                return RedirectToAction("Index");
            }
        }

        //private List<UserProfileMdl> GetLearnerApplication(string UserName)
        //{
        //    DataSet ds = new DataSet();
        //    var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();
        //    SqlConnection sql_conn = new SqlConnection(connection);

        //    SqlCommand sql_cmd = new SqlCommand("cpMst_GetUserApplication", sql_conn);
        //    sql_cmd.CommandType = CommandType.StoredProcedure;
        //    sql_cmd.Parameters.AddWithValue("@UserName", UserName);
        //    //sql_cmd.Parameters.AddWithValue("@UserPassword", Userpassword);
        //    sql_conn.Open();

        //    SqlDataAdapter da = new SqlDataAdapter(sql_cmd);
        //    da.Fill(ds);

        //    List<DashboardMdl> userApp_ = new List<DashboardMdl>();
        //    foreach (DataRow row in ds.Tables[0].Rows)
        //    {
        //        DashboardMdl uobj = new DashboardMdl();

        //        // Assuming your DashboardMdl properties have the same names as columns in the database
        //        uobj.ApplicationAppliedDate = Convert.ToDateTime(row["cp_DateApplied"]);
        //        uobj.ApplicationStatus = row["cp_ApplicationStatus"].ToString();
        //        uobj.ApplicationDesc = row["cp_Description"].ToString();
        //        uobj.ApplicationType = row["cp_Type"].ToString();

        //        userApp_.Add(uobj);
        //    }

        //    sql_conn.Close();
        //    return userApp_;
        //}






    }
}

