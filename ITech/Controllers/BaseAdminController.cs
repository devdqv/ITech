using ITech.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ITech.Controllers
{
    public class BaseAdminController : Controller
    {
       // public Models.Saleman mUserInforBH;
        public System.Globalization.CultureInfo objCultureInfo = new CultureInfo("vi-VN");

        public Models.Administrator mUserInforAd
        {
            get
            {
                Models.Administrator _mUserInfo = Session["admin"] as Models.Administrator;
                if (_mUserInfo == null)
                    _mUserInfo = new Models.Administrator();
                return _mUserInfo;
            }
        }
        // GET: Base
        public class NotAuthorizeAttribute : FilterAttribute
        {
            // Does nothing, just used for decoration
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            object[] attributes = filterContext.ActionDescriptor.GetCustomAttributes(true);
            if (attributes.Any(a => a is NotAuthorizeAttribute)) return;
            if (Session["admin"] == null)
            {
                Session["Return"] = Request.Url.PathAndQuery;
                filterContext.Result = new RedirectResult("/AccountAd/Login?Return="+ Request.Url.PathAndQuery);
            }
            //mUserInforBH = Session["user"] as Models.Saleman;
            base.OnActionExecuting(filterContext);
        }
        public void renderObjMessage(objMessage message)
        {
            var json = new JavaScriptSerializer().Serialize(message);
            Response.Clear();
            Response.ContentType = "text/json";
            Response.Write(json);
            Response.End();

        }
        public string DeleteImage(string imageName)
        {
            try
            {
                string fullPath = Server.MapPath("~/images/" + imageName);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                return "";
            }
            catch (Exception e) { return e.Message; }
        }

        public string UploadImage()
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
        public string UploadImageCommon()
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
                        filename = Path.Combine(Server.MapPath("~/Images/Commons/"), filename);

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