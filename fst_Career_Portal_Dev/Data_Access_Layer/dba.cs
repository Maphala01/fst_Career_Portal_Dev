using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;
using System.Reflection;
using fst_Career_Portal_Dev.Models;
using System.Runtime.Remoting.Messaging;
using System.Net;
using System.IO;

namespace fst_Career_Portal_Dev.Data_Access_Layer
{
    public class dba
    {
        SqlConnection sql_conn = new SqlConnection(ConfigurationManager.ConnectionStrings["db_connection"].ConnectionString);

        public static string WhoLogged_usrnm { get; set; }
        public static string WhoLogged_pwd { get; set; }
        public int User_Login(string Username, string Password)
        {
            int res = 0;
            SqlCommand sql_cmd = new SqlCommand("cpMst_spUsrLgn1", sql_conn);
            sql_cmd.CommandType = CommandType.StoredProcedure;
            sql_cmd.Parameters.AddWithValue("@cpUsrNm", Username);
            sql_cmd.Parameters.AddWithValue("@cpUsrPsswrd", Password);
            //sql_cmd.Parameters.AddWithValue("@cpUsrRl", userRol);
            sql_cmd.Parameters.AddWithValue("@TrnsNm", "Login");
            SqlParameter sql_param = new SqlParameter();
            sql_param.ParameterName = "@IsVld";
            sql_param.SqlDbType = SqlDbType.Bit;
            sql_param.Direction = ParameterDirection.Output;
            sql_cmd.Parameters.Add(sql_param);
            sql_conn.Open();
            sql_cmd.ExecuteNonQuery();
            res = Convert.ToInt32(sql_param.Value);

            WhoLogged_usrnm = Username;
            WhoLogged_pwd = Password;
            return res;

        }

        //public int User_Registration(string Username, string Password, string Role, string FirstName, string LastName,
        //                           DateTime DateOfBirth, string Email, string Grade, string school, string Province)

        public int User_Registration(string Username, string Password, int selectedRoleId, string FirstName, string LastName,
                                     string Email, string Grade, string Gender, string school, string Province, string NationalID)

        {
            RegisterMdl regMdl = new RegisterMdl();

            int res = 0;
            SqlCommand sql_cmd = new SqlCommand("cpMst_spUsrRgstrtn", sql_conn);
            sql_cmd.CommandType = CommandType.StoredProcedure;
            sql_cmd.Parameters.AddWithValue("@cpUsrNm", Username);
            sql_cmd.Parameters.AddWithValue("@cpUsrPsswrd", Password);
            //sql_cmd.Parameters.AddWithValue("@cpUsrRl", selectedRoleId);
            sql_cmd.Parameters.AddWithValue("@cpUsrEml", Email);
            sql_cmd.Parameters.AddWithValue("@cpUsrFrstNm", FirstName);
            sql_cmd.Parameters.AddWithValue("@cpUsrLstNm", LastName);
            sql_cmd.Parameters.AddWithValue("@cpUsrGrd", Grade);
            //sql_cmd.Parameters.AddWithValue("@cpUsrGndr", Gender);
            sql_cmd.Parameters.AddWithValue("@cpUsrSchl", school);
            sql_cmd.Parameters.AddWithValue("@cp_usrIdNo", NationalID);
            sql_cmd.Parameters.AddWithValue("@TrnsNm", "Register");
            SqlParameter sql_param = new SqlParameter();
            sql_param.ParameterName = "@IsVld";
            sql_param.SqlDbType = SqlDbType.Int;
            sql_param.Direction = ParameterDirection.Output;
            sql_cmd.Parameters.Add(sql_param);
            sql_conn.Open();
            sql_cmd.ExecuteNonQuery();
            res = Convert.ToInt32(sql_param.Value);
            return res;
        }
                                    
        public int SaveUserChanges(UserProfileMdl e, string UserName,int selectedProvinceId)
        {
            UserProfileMdl profMdl = new UserProfileMdl();
            int res = 0;
            
            SqlCommand sql_cmd = new SqlCommand("cpMst_spUsrPrflUpdate", sql_conn);
            sql_cmd.CommandType = CommandType.StoredProcedure;
            sql_cmd.Parameters.AddWithValue("@cpAbout",e.About);
            sql_cmd.Parameters.AddWithValue("@cpFullName", e.Fullname);
            sql_cmd.Parameters.AddWithValue("@cpUsrNatID", e.NationalID);
            sql_cmd.Parameters.AddWithValue("@cpSchool", e.School);
            sql_cmd.Parameters.AddWithValue("@cpGrade", e.Grade);
            sql_cmd.Parameters.AddWithValue("@cpProvince", selectedProvinceId);
            sql_cmd.Parameters.AddWithValue("@cpAddress1", e.Address1);
            sql_cmd.Parameters.AddWithValue("@cpAddress2", e.Address2);
            sql_cmd.Parameters.AddWithValue("@cpAddress3", e.Address3);           
            sql_cmd.Parameters.AddWithValue("@cpContact" , e.ContactNo);
            sql_cmd.Parameters.AddWithValue("@cpUsrEml", e.emailAddress);
            sql_cmd.Parameters.AddWithValue("@UserName", UserName);
            sql_cmd.Parameters.AddWithValue("@TrnsNm", "Update_UserProfile");

            SqlParameter sql_param = new SqlParameter();
            sql_param.ParameterName = "@IsVld";
            sql_param.SqlDbType = SqlDbType.Int;
            sql_param.Direction = ParameterDirection.Output;
            sql_cmd.Parameters.Add(sql_param);
            sql_conn.Open();
            sql_cmd.ExecuteNonQuery();
            res = Convert.ToInt32(sql_param.Value);
            return res;
        }

        public void SaveUserDocumentToDatabase(string fileName, string fileType, Stream fileStream, string Username,int selectedDocsId)
        {
            UserProfileMdl profMdl = new UserProfileMdl();
            //using (var connection = new SqlConnection("YourConnectionString"))
            //{
            //    connection.Open();

            //    using (var command = new SqlCommand("INSERT INTO UserDocuments (FileName, FileType, FileContent) VALUES (@FileName, @FileType, @FileContent)", connection))
            //    {
            SqlCommand sql_cmd = new SqlCommand("cpMst_spInsertUserDocument", sql_conn);
            sql_cmd.CommandType = CommandType.StoredProcedure;
            //sql_cmd.Parameters.AddWithValue("@cpAbout", e.About);
            //sql_cmd.Parameters.AddWithValue("@cpFullName", e.Fullname);


            sql_cmd.Parameters.AddWithValue("@FileName", fileName);
            sql_cmd.Parameters.AddWithValue("@FileType", fileType);
            sql_cmd.Parameters.AddWithValue("@FileUser", Username);
            sql_cmd.Parameters.AddWithValue("@SelectedDocsId", selectedDocsId); // Add this line

            byte[] fileContent;
            using (var memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                fileContent = memoryStream.ToArray();
            }

            sql_cmd.Parameters.AddWithValue("@FileContent", fileContent);
            sql_conn.Open();
            sql_cmd.ExecuteNonQuery();
        }

     
    }
}
