using ITech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace ITech.Controllers.Admin
{
    public class CustomerManageController : BaseAdminController
    {
        // GET: CustomerManage
        public ActionResult ListCustomer(string txtKeyWord="", int page=1, int pageSize=15)
        {
            ITechEntities db = new ITechEntities();
            ViewBag.lstKH = db.Users.Where(x => x.FullName.ToLower().Trim().Contains(txtKeyWord.ToLower().Trim()) || txtKeyWord.Trim() == ""  || x.ID.Contains(txtKeyWord.Trim())).OrderByDescending(x=>x.FullName).ToPagedList(page, pageSize);
            ViewBag.txtKeyWord = txtKeyWord;
            return View();
        }
    }
}