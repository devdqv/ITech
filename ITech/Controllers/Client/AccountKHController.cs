using Facebook;
using ITech.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ITech.Controllers.Client
{
    public class AccountKHController : Controller
    {
        public System.Globalization.CultureInfo objCultureInfo = new CultureInfo("vi-VN");

        // GET: AccountKH
        public ActionResult Register()
        {
            return PartialView();
        }

        // GET: AccountKH
        [HttpPost]
        public ActionResult Register(User user, string ConfirmPassword, string DateBirth)
        {
            objMessage obj = new objMessage(true);

            if (user.Password!=ConfirmPassword)
            {
                obj.Message = "Mật khẩu phải khớp nhau";
                return Json(obj);
            }
            else
            {
                ITechEntities db = new ITechEntities();
                if(db.Users.SingleOrDefault(x=>x.Email==user.Email && x.Status==HangSo.OK)!=null)
                {
                    obj.Message = "Email này đã được sử dụng để đăng ký";
                    return Json(obj);
                }
                user.ID = DateTime.Now.ToString("ddMMyyyyhhmmfff");
                user.Status = HangSo.ChuaKichHoat;
                user.Password = Helper.GetMD5(user.Password);
                user.DateOfBirth = Convert.ToDateTime(DateBirth, objCultureInfo);
                db.Users.Add(user);
                db.SaveChanges();
                obj.Error = false;
                SendActivationEmail(user);
            }
            return Json(obj);
        }

        public ActionResult Activation()
        {
            ViewBag.Message = "Không tồn tại mã code.";
            if (RouteData.Values["id"] != null)
            {
                Guid activationCode = new Guid(RouteData.Values["id"].ToString());
                ITechEntities ITechEntities = new ITechEntities();
                User userActivation = ITechEntities.Users.Where(p => p.ActivationCode == activationCode.ToString()).FirstOrDefault();
                if (userActivation != null)
                {
                    // var lstU= ITechEntities.Users.Where(x=>x.Us)
                    // ITechEntities.Users.Remove();
                    userActivation.Status = HangSo.OK;
                    ITechEntities.SaveChanges();
                    ViewBag.Message = "Kích hoạt thành công";
                    ITechEntities.Database.ExecuteSqlCommand("DELETE Users where Email=@Email and ID!=@ID", new SqlParameter[] 
                    {
                        new SqlParameter("@ID", userActivation.ID),
                        new SqlParameter("@Email", userActivation.Email)
                    });
                    Session["user"] = userActivation;
                }
            }

            return Redirect("/");
        }

        private void SendActivationEmail(User user)
        {
            Guid activationCode = Guid.NewGuid();
            ITechEntities db = new ITechEntities();
            var u = db.Users.SingleOrDefault(x => x.ID == user.ID);
            u.ActivationCode = activationCode.ToString();
            db.SaveChanges();
            string email = "vinhqd2311@gmail.com";
            string password = "Doanquangvinh123@";

            var loginInfo = new NetworkCredential(email, password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient("smtp.gmail.com", 587);

            msg.From = new MailAddress(email);
            msg.To.Add(new MailAddress(user.Email));
            msg.Subject = "Kích hoạt tài khoản ITech";
            string body = "Hello " + user.FullName + ",";
            body += "<br /><br />Vui lòng kích vào link dưới đây để kích hoạt tài khoản";
            body += "<br /><a href = '" + string.Format("{0}://{1}/AccountKH/Activation/{2}", Request.Url.Scheme, Request.Url.Authority, activationCode) + "'>Kích vào đây.</a>";
            body += "<br /><br />Cảm ơn bạn!";
            msg.Body = body;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(msg);
           
        }


        // GET: AccountKH
        public ActionResult Login()
        {
            Session["user"] = null;
            Session["admin"] = null;
            return View();
        }

        // GET: AccountKH
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model)
        {
            ITechEntities db = new ITechEntities();
            var pass = Helper.GetMD5(model.Password);
            var user = db.Users.SingleOrDefault(x => x.Email == model.Email && x.Password == pass && x.Status == HangSo.OK);
            if (user != null)
            {
                Session["user"] = user;
                string urlR = "";
                if (Session["Return"] == null)
                {
                    urlR = "/";
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

        private Uri RediredtUri

        {

            get

            {

                var uriBuilder = new UriBuilder(Request.Url);

                uriBuilder.Query = null;

                uriBuilder.Fragment = null;

                uriBuilder.Path = Url.Action("FacebookCallback");

                return uriBuilder.Uri;

            }

        }



        [AllowAnonymous]

        public ActionResult Facebook()

        {

            var fb = new FacebookClient();

            var loginUrl = fb.GetLoginUrl(new

            {



                client_id = "2911366635850545",
                client_secret = "90045e221533512447c6ed5a6ac2f6b7",

                redirect_uri = RediredtUri.AbsoluteUri,

                response_type = "code",

                scope = "email"



            });

            return Redirect(loginUrl.AbsoluteUri);

        }



        public ActionResult FacebookCallback(string code)
        {

            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "2911366635850545",
                client_secret = "90045e221533512447c6ed5a6ac2f6b7",
                redirect_uri = RediredtUri.AbsoluteUri,
                code = code

            });

            var accessToken = result.access_token;
            Session["AccessToken"] = accessToken;
            fb.AccessToken = accessToken;
            dynamic me = fb.Get("me?fields=id,link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");
            string email = me.email;
            string id = me.id;
            ITechEntities db = new ITechEntities();
            ITech.Models.User saleman = db.Users.SingleOrDefault(x => x.ID == id);
            if (saleman == null)
            {
                ITech.Models.User sm = new Models.User()
                {
                    ID = me.id,
                    Email = me.email,
                    Avatar = me.picture.data.url,
                    FullName = me.first_name + " " + me.last_name,
                    Gender= me.gender,
                    Address= me.locale
                };
                Session["user"] = sm;
                db.Users.Add(sm);
                db.SaveChanges();
            }
            else
            {
                Session["user"] = saleman;
            }

            //TempData["email"] = me.email;

            //TempData["first_name"] = me.first_name;

            //TempData["lastname"] = me.last_name;

            //TempData["picture"] = me.picture.data.url;

            FormsAuthentication.SetAuthCookie(email, false);
            string urlR = "";
            if (Session["Return"] == null)
            {
                urlR = "/";
            }
            else
            {
                urlR = Session["Return"] as string;
            }
            return Redirect(urlR);

        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return Redirect("/");



        }
    }
}