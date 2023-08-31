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

        // GET: Dashboard
        public ActionResult Index()
        {

            //learner_opp_ = new DashboardMdl();
            string userName = WhoLogged_user;
            
            DashboardMdl viewModels = new DashboardMdl
            
            {
                Username = userName,
                Opportunities = GetLearnerOpportunities(),
                RecentApplication = GetLearnerApplication(WhoLogged_user)

                
            };
           

            return View(viewModels);

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