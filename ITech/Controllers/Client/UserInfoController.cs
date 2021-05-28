using ITech.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITech.Controllers.Client
{
    public class UserInfoController : BaseKHController
    {

        public string anhtmps = "";

        // GET: AccountKH
        public ActionResult ChangeInfor()
        {

            return View(mUserInfor);
        }

        // GET: AccountKH
        [HttpPost]
        public ActionResult UpdateInfor()
        {
            ITechEntities db = new ITechEntities();
            var utmp = mUserInfor;
            var u = db.Users.SingleOrDefault(x => x.ID == utmp.ID);
            GetDataUser(u);
            if (string.IsNullOrEmpty(anhtmps))
            {
                DeleteImageAvatar(u.Avatar);
                u.Avatar = "";
            }
            var filenames = UploadImageAvatar();
            List<string> imgsName = null;
            if (filenames != "-1" && filenames != "0") //khong co anh hoac error
            {
                imgsName = filenames.Split(';').ToList();
                if (!string.IsNullOrEmpty(imgsName[0]))
                {
                    u.Avatar = "/images/" + imgsName[0];
                }
            }

            db.SaveChanges();
            Session["user"] = u;
            return Json(new objMessage(false, "Cập nhật thông tin thành công"));
        }

        private void GetDataUser(User user)
        {

            var str = Request["DateBirth"].ToString();
            if (!string.IsNullOrEmpty(Request["FullName"]))
                user.FullName = Request["FullName"];
            if (!string.IsNullOrEmpty(Request["DateBirth"]))
                user.DateOfBirth =Convert.ToDateTime(Request["DateBirth"], objCultureInfo);
            if (!string.IsNullOrEmpty(Request["Phone"]))
                user.Phone = Request["Phone"];
            if (!string.IsNullOrEmpty(Request["Gender"]))
                user.Gender = int.Parse(Request["Gender"]);
            if (!string.IsNullOrEmpty(Request["Address"]))
                user.Address = Request["Address"];

            if (!string.IsNullOrEmpty(Request["hdfileuploadA"]))
                anhtmps = Request["hdfileuploadA"];


        }

        public string DeleteImageAvatar(string imageName)
        {
            try
            {
                string fullPath = Server.MapPath("~/" + imageName);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                return "";
            }
            catch (Exception e) { return e.Message; }
        }

        public string UploadImageAvatar()
        {
            string fileNames = "";
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {

                        HttpPostedFileBase file = files[i];
                        string filename = Path.GetFileNameWithoutExtension(file.FileName);
                        filename = filename.Replace(" ", "");
                        string extension = Path.GetExtension(file.FileName);
                        filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                        fileNames += filename + ";";
                        filename = Path.Combine(Server.MapPath("~/Images/"), filename);

                        // Get the complete folder path and store the file inside it.  
                        file.SaveAs(filename);
                    }
                    // Returns message that successfully uploaded  
                    return fileNames;
                }
                catch (Exception ex)
                {
                    return "-1";
                }
            }
            return "0";
        }
    }
}