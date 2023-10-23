using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fst_Career_Portal_Dev.Models
{
    public class Interest
    {
        public string Name { get; set; }
        public int OpportunityId { get; set; }
    }
    public class OpportunityMdl
    {
        public int OpportunityId { get; set; }
        public DateTime PostedDate { get; set; }
        public string Province { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public List<OpportunityMdl> Opportunities { get; set; }
        public int ApplicationNo { get; set; }
        public string ApplicationDesc { get; set; }
        public DateTime ApplicationAppliedDate { get; set; }
        public string ApplicationStatus { get; set; }
        public string ApplicationType { get; set; }
        public List<OpportunityMdl> RecentApplication { get; set; }
        public string Username { get; set; }
        public List<Interest> InterestList { get; set; }
    }
}