using ITech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITech.Controllers.Saleman
{
    public class AccountBHController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            Session["user"] = null;
            Session["admin"] = null;
            return View();
        }

        // GET: Account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Models.User model)
        {

            ITechEntities db = new ITechEntities();
            var pass = Helper.GetMD5(model.Password);
            var user = db.Users.SingleOrDefault(x => x.Email == model.Email && x.Password == pass && x.Status==HangSo.OK);
            if (user != null)
            {
                Session["user"] = user;
                string urlR = "";
                if (Session["Return"] == null)
                {
                    urlR = "/trang-ban-hang";
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
                return View(model);
            }


        }
    }
}