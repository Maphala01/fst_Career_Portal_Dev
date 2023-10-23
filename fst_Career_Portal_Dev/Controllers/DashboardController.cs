using fst_Career_Portal_Dev.Data_Access_Layer;
using fst_Career_Portal_Dev.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;

namespace fst_Career_Portal_Dev.Controllers
{
    public class DashboardController : Controller
    {
        string WhoLogged_user = dba.WhoLogged_usrnm;
        string WhoLogged_pss = dba.WhoLogged_pwd;

        //Data Access
        Data_Access_Layer.dba dbaLayer = new Data_Access_Layer.dba();

        DashboardMdl learner_opp_;

        //DashboardMdl learner_opp_;

        private string GetNeonBorderClass(int percentageCompleted)
        {
            DashboardMdl dm = new DashboardMdl();
            if (percentageCompleted >= 0 && percentageCompleted < 49)
            {
                
                ViewBag.PersonalPerce = percentageCompleted;

                return "neon-border-0-24"; //Red neon
            }

            else if (percentageCompleted >= 50 && percentageCompleted < 79)
            {
                ViewBag.PersonalPerce = percentageCompleted;
                
                return "neon-border-25-49"; //Amber neon
            }
            else if (percentageCompleted >= 80 && percentageCompleted <= 100)
            {
                ViewBag.PersonalPerce = percentageCompleted;

                return "neon-border-75-100"; //Green neon
            }
            else
            {
                return ""; // Handle invalid percentages
            }
        }

        private string GetNeonBorderClass2(int percentageCompleted2)
        {
            DashboardMdl dm = new DashboardMdl();
            if (percentageCompleted2 >= 0 && percentageCompleted2 < 49)
            {
               
                ViewBag.DocumentsPerce = percentageCompleted2;
                
                return "neon-border2-0-24";
            }
            else if (percentageCompleted2 >= 50 && percentageCompleted2 < 79)
            {              
                ViewBag.DocumentsPerce = percentageCompleted2; 
              
                return "neon-border2-25-49";
            }
            else if (percentageCompleted2 >= 80 && percentageCompleted2 <= 100)
            {
                ViewBag.DocumentsPerce = percentageCompleted2;

                return "neon-border2-75-100";
            }
            else
            {
                return ""; // Handle invalid percentages
            }
        }

        private string GetNeonBorderClass3(int percentageCompleted3)
        {
            DashboardMdl dm = new DashboardMdl();
            if (percentageCompleted3 == 40)
            {

                ViewBag.CareerPerce = percentageCompleted3; ;

                return "neon-border3-0-24";
            }
            //else if (percentageCompleted3 == 34)
            //{
            //    ViewBag.CareerPerce = "34";

            //    return "neon-border2-25-49";
            //}
            else if (percentageCompleted3 == 70)
            {
                ViewBag.CareerPerce = percentageCompleted3;

                return "neon-border3-50-74";
            }
            else if (percentageCompleted3 == 100)
            {
                ViewBag.CareerPerce = percentageCompleted3;

                return "neon-border3-75-100";
            }
            else
            {
                return ""; // Handle invalid percentages
            }
        }

        // GET: Dashboard
        public ActionResult Index()
        {

            //learner_opp_ = new DashboardMdl();
            string userName = WhoLogged_user;
            GetWizard_PersonalInfo();
            GetWizard_Documents();
            GetWizard_CareerPath();
            GetNewsUpdates();

            DashboardMdl viewModels = new DashboardMdl
            
            {
                Username = userName,
                Opportunities = GetLearnerOpportunities(),
                RecentApplication = GetLearnerApplication(WhoLogged_user)

                
            };
           

            return View(viewModels);

        }

        public ActionResult GetWizard_PersonalInfo()
        {
            string username = WhoLogged_user;

            int percentageCompleted = GetPercentageCompletedForUser(username);

            // Determine the CSS class based on the percentage 
            string neonBorderClass = GetNeonBorderClass(percentageCompleted);

            // Pass the CSS class to the view model or ViewBag
            ViewBag.NeonBorderClass = neonBorderClass;
            return View("Index");
        }

        public ActionResult GetNewsUpdates()
        {
            List<NewsUpdate> newsUpdates = GetNewsUpdatesFromDatabase();
            return View(newsUpdates);
        }

        public List<NewsUpdate> GetNewsUpdatesFromDatabase()
        {
            List<NewsUpdate> newsUpdates = new List<NewsUpdate>();

            var connectMe = ConfigurationManager.ConnectionStrings["db_connection"].ToString();
            using (SqlConnection connection = new SqlConnection(connectMe))
            {
                connection.Open();

                string query = "SELECT [NewsUpdates_Id], [NewsUpdates_Head], [NewsUpdates_desc], [NewsUpdates_Image], [DateCreated], [DateUpdated], [NoOfDaysOrHoursOrMinutes] FROM [mst_cpNewsUpdates_tbl]";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NewsUpdate newsUpdate = new NewsUpdate
                            {
                                NewsUpdates_Id = Convert.ToInt32(reader["NewsUpdates_Id"]),
                                NewsUpdates_Head = reader["NewsUpdates_Head"].ToString(),
                                NewsUpdates_desc = reader["NewsUpdates_desc"].ToString(),
                                NoOfDaysOrHoursOrMinutes = reader["NoOfDaysOrHoursOrMinutes"].ToString()
                                // Add more properties as needed
                            };

                            newsUpdates.Add(newsUpdate);
                        }
                    }
                }
            }

            return newsUpdates;
        }


        public ActionResult GetWizard_Documents()
        {
            string username = WhoLogged_user;

            int percentageCompleted2 = GetPercentageCompletedForUser2(username);

            // Determine the CSS class based on the percentage GetNeonBorderClass2
            string neonBorderClass2 = GetNeonBorderClass2(percentageCompleted2);

            // Pass the CSS class to the view model or ViewBag
            ViewBag.NeonBorderClass2 = neonBorderClass2;
            return View("Index");
        }

        public ActionResult GetWizard_CareerPath()
        {
            string username = WhoLogged_user;

            int percentageCompleted3 = GetPercentageCompletedForUser3(username);

            // Determine the CSS class based on the percentage GetNeonBorderClass2
            string neonBorderClass3 = GetNeonBorderClass3(percentageCompleted3);

            // Pass the CSS class to the view model or ViewBag
            ViewBag.NeonBorderClass3 = neonBorderClass3;
            return View("Index");
        }

        private int GetPercentageCompletedForUser(string usrnm)
        {
            int percentageCompleted = 0;

            var connectMe = ConfigurationManager.ConnectionStrings["db_connection"].ToString();

            using (SqlConnection sqlconn = new SqlConnection(connectMe))
            {
                sqlconn.Open();

                using (SqlCommand command = new SqlCommand("GetPercentageCompletedForUser", sqlconn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Input parameter for the username
                    command.Parameters.AddWithValue("@cpUsrNm", usrnm);

                    // Output parameter for the percentage completed
                    SqlParameter outputParameter = new SqlParameter();
                    outputParameter.ParameterName = "@PercentageNotCompleted";
                    outputParameter.SqlDbType = SqlDbType.Decimal;
                    outputParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParameter);

                    command.ExecuteNonQuery();

                    // Retrieve the percentage from the output parameter
                    if (outputParameter.Value != DBNull.Value)
                    {
                        percentageCompleted = Convert.ToInt32(outputParameter.Value);
                    }
                }
            }

            return percentageCompleted;
        }

        private int GetPercentageCompletedForUser2(string usrnm)
        {
            int percentageCompleted = 0;

            var connectMe = ConfigurationManager.ConnectionStrings["db_connection"].ToString();

            using (SqlConnection sqlconn = new SqlConnection(connectMe))
            {
                sqlconn.Open();


                using (SqlCommand command = new SqlCommand("[GetPercentageCompletedForUser2]", sqlconn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Input parameter for the username
                    command.Parameters.AddWithValue("@cpUsrNm", usrnm);

                    // Output parameter for the percentage completed
                    SqlParameter outputParameter = new SqlParameter();
                    outputParameter.ParameterName = "@PercentageCompleted";
                    outputParameter.SqlDbType = SqlDbType.Decimal;
                    outputParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParameter);

                    command.ExecuteNonQuery();

                    // Retrieve the percentage from the output parameter
                    if (outputParameter.Value != DBNull.Value)
                    {
                        percentageCompleted = Convert.ToInt32(outputParameter.Value);
                    }
                }
            }

            return percentageCompleted;
        }

        private int GetPercentageCompletedForUser3(string usrnm)
        {
            int percentageCompleted = 0;

            var connectMe = ConfigurationManager.ConnectionStrings["db_connection"].ToString();

            using (SqlConnection sqlconn = new SqlConnection(connectMe))
            {
                sqlconn.Open();


                using (SqlCommand command = new SqlCommand("[GetPercentageCompletedForUser3]", sqlconn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Input parameter for the username
                    command.Parameters.AddWithValue("@cpUsrNm", usrnm);

                    // Output parameter for the percentage completed
                    SqlParameter outputParameter = new SqlParameter();
                    outputParameter.ParameterName = "@PercentageCompleted";
                    outputParameter.SqlDbType = SqlDbType.Decimal;
                    outputParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParameter);

                    command.ExecuteNonQuery();

                    // Retrieve the percentage from the output parameter
                    if (outputParameter.Value != DBNull.Value)
                    {
                        percentageCompleted = Convert.ToInt32(outputParameter.Value);
                    }
                }
            }

            return percentageCompleted;
        }



        private List<DashboardMdl> GetLearnerOpportunities()
        {
            DataSet ds = new DataSet();
            var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();
            SqlConnection sql_conn = new SqlConnection(connection);

            SqlCommand sql_cmd = new SqlCommand("cpMst_GetDashInformation", sql_conn);

            sql_conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sql_cmd);
            da.Fill(ds);

            List<DashboardMdl> userlist_ = new List<DashboardMdl>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DashboardMdl uobj = new DashboardMdl();
                uobj.PostedDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["cp_PostedDate"]);
                uobj.Province = ds.Tables[0].Rows[i]["cp_Province"].ToString();
                uobj.Description = ds.Tables[0].Rows[i]["cp_Description"].ToString();
                uobj.Type = ds.Tables[0].Rows[i]["cp_Type"].ToString();
                uobj.Status = ds.Tables[0].Rows[i]["cp_Status"].ToString();

                userlist_.Add(uobj);
            }
            sql_conn.Close();
            return userlist_;
        }

        [HttpGet]
        public ActionResult RecentUserApplication()
        {
            
            learner_opp_ = new DashboardMdl();
            learner_opp_.Opportunities = GetLearnerApplication(WhoLogged_user);

            return View(learner_opp_.RecentApplication);

        }

        private List<DashboardMdl> GetLearnerApplication(string UserName)
        {
            DataSet ds = new DataSet();
            var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();
            SqlConnection sql_conn = new SqlConnection(connection);

            SqlCommand sql_cmd = new SqlCommand("cpMst_GetUserApplication", sql_conn);
            sql_cmd.CommandType = CommandType.StoredProcedure;
            sql_cmd.Parameters.AddWithValue("@UserName", UserName);
            //sql_cmd.Parameters.AddWithValue("@UserPassword", Userpassword);
            sql_conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sql_cmd);
            da.Fill(ds);

            List<DashboardMdl> userApp_ = new List<DashboardMdl>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                DashboardMdl uobj = new DashboardMdl();

                // Assuming your DashboardMdl properties have the same names as columns in the database
                uobj.ApplicationAppliedDate = Convert.ToDateTime(row["cp_DateApplied"]);
                uobj.ApplicationStatus = row["cp_ApplicationStatus"].ToString();
                uobj.ApplicationDesc = row["cp_Description"].ToString();
                uobj.ApplicationType = row["cp_Type"].ToString();

                userApp_.Add(uobj);
            }

            sql_conn.Close();
            return userApp_;
        }
    }
}