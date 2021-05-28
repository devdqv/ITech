using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Newtonsoft.Json;
using System.Web.Security;
using ITech.Models;

namespace ITech.Controllers
{
    public class AccountController : Controller
    {

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
            ITech.Models.User saleman = db.Users.SingleOrDefault(x => x.ID==id);
            if (saleman == null)
            {
                ITech.Models.User sm = new Models.User()
                {
                    ID=me.id,
                    Email = me.email,
                    Avatar = me.picture.data.url,
                    FullName = me.first_name + " "+ me.last_name
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
            string urlR="";
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

        public ActionResult Logout()
        {
            if(Session["user"]!=null)
            {
                Session.Abandon();
                return Redirect("/dang-nhap-ban-hang");
            }
            else if (Session["admin"] != null)
            {
                Session.Abandon();
                return Redirect("/admin");
            }
            else
            {
                return Redirect("/trang-chu");
            }

        }

    }
}