using ITech.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITech.Controllers.Admin
{
    public class ManageShopAdController : BaseAdminController
    {
        // GET: ManageShopAd
        public ActionResult ListGianHang(string TenCH, int tt = 0, int page = 1, int pageSize = 15)
        {
            ITechEntities db = new ITechEntities();
            var lstGianHang = db.Shops.Where(x => (tt == 0 || x.Status == tt) &&
            (x.ShopName.Contains(TenCH) || string.IsNullOrEmpty(TenCH)))
                .OrderByDescending(x => x.Status).ThenByDescending(x => x.ShopName).ToPagedList(page, pageSize);
            ViewBag.TenCH = TenCH;
            ViewBag.TrangThai = tt;
            return View(lstGianHang);
        }

        [HttpPost]
        public ActionResult ChangeStatusShop(string id)
        {
            objMessage objMessage = new objMessage(true);
            ITechEntities db = new ITechEntities();
            var objShop = db.Shops.SingleOrDefault(x => x.ID == id);
            if (objShop != null)
            {
                if (objShop.Status == HangSo.KhoaByAdmin)
                {
                    objShop.Status = HangSo.DaDuyet;
                }
                else if (objShop.Status == HangSo.DaDuyet)
                {
                    objShop.Status = HangSo.KhoaByAdmin;

                }
                db.SaveChanges();
                objMessage.Error = false;
                objMessage.Message = objShop.Status == HangSo.DaDuyet ? "Đã mở khóa cửa hàng" : "Đã khóa cửa hàng tạm thời";
                return Json(objMessage);

            }
            objMessage.Message = "Khóa / mở khóa không thành công";
            return Json(objMessage);
        }


        [HttpPost]
        public ActionResult DuyetShop(string id)
        {
            objMessage objMessage = new objMessage(true);
            ITechEntities db = new ITechEntities();
            var objShop = db.Shops.SingleOrDefault(x => x.ID == id);
            if (objShop != null)
            {

                objShop.Status = HangSo.DaDuyet;

                db.SaveChanges();
                objMessage.Error = false;
                objMessage.Message = "Đã xác nhận cửa hàng đi vào hoặt động";
                return Json(objMessage);

            }
            
            objMessage.Message = "Xác nhận cửa hàng không thành công";
            return Json(objMessage);
        }
    }
}