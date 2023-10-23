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

        public string PersonalPerce { get; set; }
        public string DocumentsPerce { get; set; }
        public string CareerPerce { get; set; }

        public string storyOne_Header { get; set; }
        public string storyOne_Descr { get; set; }
        public string storyOne_NoOf { get; set; }

        public string storyTwo_Header { get; set; }
        public string storyTwo_Descr { get; set; }
        public string storyTwo_NoOf { get; set; }

        public string storyThree_Header { get; set; }
        public string storyThree_Descr { get; set; }
        public string storyThree_NoOf { get; set; }

        public string storyFour_Header { get; set; }
        public string storyFour_Descr { get; set; }
        public string storyFour_NoOf { get; set; }

        public string storyFive_Header { get; set; }
        public string storyFive_Descr { get; set; }
        public string storyFive_NoOf { get; set; }
    }

    public class NewsUpdate
    {
        public int NewsUpdates_Id { get; set; }
        public string NewsUpdates_Head { get; set; }
        public string NewsUpdates_desc { get; set; }
        public string NoOfDaysOrHoursOrMinutes { get; set; }
    }
    }