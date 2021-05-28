using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITech.Controllers.Admin
{
    public class HomeAdController : BaseAdminController
    {
        // GET: HomeAd
        public ActionResult Index()
        {
            return View();
        }
    }
}