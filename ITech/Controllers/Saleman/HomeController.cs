using ITech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITech.Controllers.Saleman
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {
            ITechEntities db = new ITechEntities();
            var lstShop = db.Shops.Where(x => x.UserID == mUserInforBH.ID && x.Status==HangSo.DaDuyet ).Select(x=>x.ID); //lấy ra các shopID do user quản lý ko bị khóa
            var orders= db.Orders.Where(x => lstShop.Contains(x.ShopID));
            var products = db.Products.Where(x => lstShop.Contains(x.ShopID));

            ViewBag.ChoXacNhan = orders.Where(x => x.Status == HangSo.ChoXacNhan).Count();
            ViewBag.DaHuy = orders.Where(x => x.Status == HangSo.DaBiHuy).Count();
            ViewBag.DaXacNhan = orders.Where(x => x.Status == HangSo.DaXacNhan).Count();

            ViewBag.SPBiKhoa = products.Where(x => x.ApproveStatus == HangSo.DaKhoa).Count();
            ViewBag.SPHetHang = products.Where(x => x.Inventory == 0).Count();
            return View();
        }
    }
}