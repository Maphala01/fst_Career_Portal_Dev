using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fst_Career_Portal_Dev.Models
{
    public class UserProfileMdl
    {
        public string UserName { get; set; }
        public string Fullname { get; set; }
        public string School { get; set; }
        public string Grade { get; set; }
        public string Province { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string ContactNo { get; set; }
        public string emailAddress { get; set; }
        public string CurrentPassword { get; set; }
        public string Interest1 { get; set; }
        public string Interest2 { get; set; }
        public string Interest3 { get; set; }
        public string Interest4 { get; set; }
        public string Interest5 { get; set; }
        public string About { get; set; }
        public string NationalID { get; set; }
        public int SelectedProvinceId { get; set; }
        public List<UserProfileMdl> ProvinceList { get; set; }
        public int Profile_ProvinceID { get; set; }
        public string Profile_ProvinceName { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public int DocumentId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileTypeName { get; set; }
        public byte[] FileContent { get; set; }
        public string FileExtension { get; set; }
        public DateTime UploadDate { get; set; }
        public List<UserProfileMdl> UserDocuments { get; set; }
        public int SelectedDocsId { get; set; }
        public List<UserProfileMdl> DocumentList { get; set; }
        public int Profile_DocumentID { get; set; }
        public string Profile_DocumentName { get; set; }
        public int OpportunityId { get; set; }
        public DateTime PostedDate { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public List<UserProfileMdl> Opportunities { get; set; }
        public int ApplicationNo { get; set; }
        public string ApplicationDesc { get; set; }
        public DateTime ApplicationAppliedDate { get; set; }
        public string ApplicationStatus { get; set; }
        public string ApplicationType { get; set; }
        public List<UserProfileMdl> RecentApplication { get; set; }
        public int OpportunitytId { get; set; }
        public string OpportunityDescription { get; set; }
        public string OpportunityStatus { get; set; }
        public List<UserProfileMdl> OpportunitiesList { get; set; }
        public List<UserProfileMdl> RecentInterestList { get; set; }
        public string InterestID { get; set; }
        public string NewPasswordChange { get; set; }
        public string NewCurrentPasswordChange { get; set; }
        public string NewConfirmPasswordChange { get; set; }
    }
}