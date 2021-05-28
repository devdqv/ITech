using ITech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITech.Controllers.Admin
{
    public class AccountAdController : Controller
    {
        // GET: AccountAd
        
        public ActionResult Login()
        {
            Session["user"] = null;
            Session["admin"] = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Administrator model)
        {

            ITechEntities db = new ITechEntities();
            var pass = Helper.GetMD5(model.Password);
            var user = db.Administrators.SingleOrDefault(x => x.Email == model.Email && x.Password == pass);
             if (user != null)
            {
                Session["admin"] = user;
                string urlR = "";
                if (Session["Return"] == null)
                {
                    urlR = "/trang-quan-tri"; 
                }
                else
                {
                    urlR = Session["Return"] as string;
                }
                return Redirect(urlR);
            } 
            else
            {
                ModelState.AddModelError("", "Tên tài khoản hoặc mật khẩu không đúng");
                ViewBag.model = model;
                return View();
            }

            
        }


    }
}