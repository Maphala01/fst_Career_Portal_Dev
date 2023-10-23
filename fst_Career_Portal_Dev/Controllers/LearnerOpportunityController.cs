using fst_Career_Portal_Dev.Data_Access_Layer;
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
using System.Web.UI.WebControls;

namespace fst_Career_Portal_Dev.Controllers
{
    public class LearnerOpportunityController : Controller
    {
        string WhoLogged_user = dba.WhoLogged_usrnm;
        string WhoLogged_pss = dba.WhoLogged_pwd;
        //private int OppID_;

        // GET: LearnerOpportunity
        public ActionResult Index()
        {
            //learner_opp_ = new DashboardMdl();
            string userName = WhoLogged_user;

            OpportunityMdl viewModels = new OpportunityMdl

            {
                Username = userName,
                Opportunities = GetLearnerOpportunities(),
                //RecentApplication = GetLearnerApplication(WhoLogged_user)


            };


            return View(viewModels);
        }

        private List<OpportunityMdl> GetLearnerOpportunities()
        {
            DataSet ds = new DataSet();
            var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();
            SqlConnection sql_conn = new SqlConnection(connection);

            SqlCommand sql_cmd = new SqlCommand("cpMst_GetDashInformation", sql_conn);

            sql_conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sql_cmd);
            da.Fill(ds);

            List<OpportunityMdl> userlist_ = new List<OpportunityMdl>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                OpportunityMdl uobj = new OpportunityMdl();
                uobj.OpportunityId = (int)ds.Tables[0].Rows[i]["cp_OpportunityId"];
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

        public ActionResult SaveLearnerInterest(List<int> selectedOpportunityIds, int selectedOpportunitiesCount)
        {

            //OpportunityMdl opportunity = new OpportunityMdl();
            //if (selectedOpportunitiesCount > 5) //if (selectedOpportunities.Length > 5)
            //{
            //    TempData["OpportunitySelectionError"] = "Error...You can only select a maximum of 5 opportunities.";
            //    // Reload your view or return an error view
            //    return RedirectToAction("Index", "LearnerOpportunity");
            //}
            //else if (selectedOpportunitiesCount >= 0 && selectedOpportunitiesCount <= 5)
            //{
            //    // Initialize a list to store selected interests
            //    var interestList = new List<Interest>();
            //    foreach (var kvp in selectedOpportunityIds)
            //    {
            //        int opportunityId = kvp.Key;
            //        int selectedValue = kvp.Value;

            //        if (selectedValue == 1)
            //        {
            //            // The checkbox was selected, so add it to the interestList
            //            interestList.Add(new Interest { OpportunityId = opportunityId });
            //        }
            //    }

            //    try
            //    {
            //        var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();
            //        using (SqlConnection sql_conn = new SqlConnection(connection))
            //        {
            //            sql_conn.Open();

            //            foreach (var interest in interestList)
            //            {
            //                using (SqlCommand sql_cmd = new SqlCommand("cpMst_spInsertInterests", sql_conn))
            //                {
            //                    // Specify that you are using a stored procedure
            //                    sql_cmd.CommandType = CommandType.StoredProcedure;

            //                    // Replace "YourInterestTable" with the actual name of your interest table
            //                    sql_cmd.Parameters.AddWithValue("@cpUsrNm", WhoLogged_user);
            //                    sql_cmd.Parameters.AddWithValue("@cpInterestId", interest.OpportunityId);


            //                    // Execute the stored procedure
            //                    sql_cmd.ExecuteNonQuery();
            //                }
            //            }
            //        }
            //    }
            if (selectedOpportunityIds.Count > 5) //if (selectedOpportunities.Length > 5)
            {
                TempData["OpportunitySelectionError"] = "Error...You can only select a maximum of 5 opportunities.";
                // Reload your view or return an error view
                return RedirectToAction("Index", "LearnerOpportunity");
            }
            else
            {
                try
                {
                    var connection = ConfigurationManager.ConnectionStrings["db_connection"].ToString();
                    using (SqlConnection sql_conn = new SqlConnection(connection))
                    {
                        sql_conn.Open();

                        //Place counter in controller, that makes more sense and much easier : DOnt be clever
                        foreach (int opportunityId in selectedOpportunityIds)
                        {
                            using (SqlCommand sql_cmd = new SqlCommand("cpMst_spInsertInterests", sql_conn))
                            {
                                // Specify that you are using a stored procedure
                                sql_cmd.CommandType = CommandType.StoredProcedure;

                                // Replace "YourInterestTable" with the actual name of your interest table
                                sql_cmd.Parameters.AddWithValue("@cpUsrNm", WhoLogged_user);
                                sql_cmd.Parameters.AddWithValue("@cpInterestId", opportunityId);

                                // Execute the stored procedure
                                sql_cmd.ExecuteNonQuery();
                            }
                        }

                    }
                    TempData["OpportunitySuccess"] = "Your interest(s) have been successfully saved..";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "An error occurred while saving interests: " + ex.Message;
                }

                return RedirectToAction("Index", "LearnerOpportunity");
            }
        }
    }
}