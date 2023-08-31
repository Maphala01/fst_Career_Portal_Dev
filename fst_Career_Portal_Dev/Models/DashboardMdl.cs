using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fst_Career_Portal_Dev.Models
{
    public class DashboardMdl
    {
        public int OpportunityId { get; set; }
        public DateTime PostedDate { get; set; }
        public string Province { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public List<DashboardMdl> Opportunities { get; set; }
        public int ApplicationNo { get; set; }
        public string ApplicationDesc { get; set; } 
        public DateTime ApplicationAppliedDate { get; set; }
        public string ApplicationStatus { get; set; }
        public string ApplicationType { get; set; }
        public List<DashboardMdl> RecentApplication { get; set; }
        public string Username { get; set;}

    }
}