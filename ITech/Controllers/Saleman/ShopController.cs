using ITech.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITech.Controllers.Saleman
{
    public class ShopController : BaseController
    {
        // GET: Shop
        [HttpGet]
        public ActionResult ThemGianHang()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemGianHang(Shop model)
        {
            string img = "";
            ITechEntities db = new ITechEntities();
            objMessage objMessage = new objMessage();
            objMessage.Error = true;
            try
            {
                model.Status = HangSo.OK;
                model.ID = DateTime.Now.ToString("ddMMyyyyhhmmssfff");
                var file = UploadImageCommon();
                if (file == "0" || file == "-1")
                {
                    objMessage.Message = "Bạn phải chọn ảnh";
                    return Json(objMessage);
                }
                else
                {
                    string[] filenames = file.Split(';');
                    if (filenames.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(filenames[0]))
                        {
                            model.Avatar = filenames[0];
                            img = filenames[0];
                        }
                        else
                        {
                            objMessage.Message = "Bạn phải chọn ảnh";
                            return Json(objMessage);

                        }
                    }
                    else
                    {
                        objMessage.Message = "Bạn phải chọn ảnh";
                        return Json(objMessage);

                    }
                }

                model.UserID = mUserInforBH.ID;
                model.CityCode = Request["ddlCity"].ToString().Trim();
                model.DistrictCode = Request["ddlDistrict"].ToString().Trim();
                model.WardCode = Request["ddlWard"].ToString().Trim();
                model.Status = HangSo.ChoDuyet;
                db.Shops.Add(model);
                db.SaveChanges();
                objMessage.Error = false;
                objMessage.Message = "Thêm mới thành công ? Cửa hàng sẽ được xác nhận bởi người quản trị !";
            }
            catch (Exception ex)
            {
                DeleteImageCommon(img);
                objMessage.Message = "Có lỗi khi thêm mới";
            }

            return Json(objMessage);
        }


        public ActionResult ListGianHang(string TenCH, int page = 1, int pageSize = 15)
        {
            ITechEntities db = new ITechEntities();
            var lstGianHang = db.Shops.Where(x => x.UserID == mUserInforBH.ID
            && (x.ShopName.Contains(TenCH) || string.IsNullOrEmpty(TenCH)))
                .OrderByDescending(x => x.Status).ThenByDescending(x => x.ShopName).ToPagedList(page, pageSize);
            ViewBag.TenCH = TenCH;
            return View(lstGianHang);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            objMessage objMessage = new objMessage(true);
            ITechEntities db = new ITechEntities();
            var objShop = db.Shops.SingleOrDefault(x => x.ID == id && x.UserID == mUserInforBH.ID);
            if (objShop != null)
            {
                db.Shops.Remove(objShop);
                db.SaveChanges();
                DeleteImageCommon(objShop.Avatar);

                objMessage.Error = false;
                objMessage.Message = "Xóa thành công";
                return Json(objMessage);

            }
            objMessage.Message = "Xóa không thành công";
            return Json(objMessage);
        }

        [HttpPost]
        public ActionResult ChangeStatusShop(string id)
        {
            objMessage objMessage = new objMessage(true);
            ITechEntities db = new ITechEntities();
            var objShop = db.Shops.SingleOrDefault(x => x.ID == id && x.UserID == mUserInforBH.ID);
            if (objShop != null)
            {
                if (objShop.Status == HangSo.KhoaByShop)
                {
                    objShop.Status = HangSo.DaDuyet;
                }
                else if(objShop.Status == HangSo.DaDuyet)
                {
                    objShop.Status = HangSo.KhoaByShop;

                }
                db.SaveChanges();
                objMessage.Error = false;
                objMessage.Message = objShop.Status==HangSo.DaDuyet?"Đã mở khóa cửa hàng":"Đã khóa cửa hàng tạm thời";
                return Json(objMessage);

            }
            objMessage.Message = "Khóa / mở khóa không thành công";
            return Json(objMessage);
        }


        public ActionResult Edit(string id)
        {
            objMessage objMessage = new objMessage(true);
            ITechEntities db = new ITechEntities();
            var objShop = db.Shops.SingleOrDefault(x => x.ID == id);

            return View(objShop);
        }
        [HttpPost]
        public ActionResult Edit(Shop model)
        {
            string img = "";
            ITechEntities db = new ITechEntities();
            objMessage objMessage = new objMessage();
            objMessage.Error = true;
            try
            {
                var obj = db.Shops.SingleOrDefault(x => x.ID == model.ID);

                var file = UploadImageCommon();
                if (file == "0" || file == "-1")
                {

                }
                else
                {
                    string[] filenames = file.Split(';');
                    if (filenames.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(filenames[0]))
                        {
                            DeleteImageCommon(obj.Avatar);
                            obj.Avatar = filenames[0];
                            img = filenames[0];
                        }

                    }

                }
                getDataShop(obj);
                obj.Phone = model.Phone;
                obj.ShopName = model.ShopName;
                obj.Email = model.Email;
                obj.Description = model.Description;
                //obj.Address1 = model.Address1;
                // obj.Address2 = model.Address2;
                db.SaveChanges();
                objMessage.Error = false;
                objMessage.Message = "Cập nhật thành công";
            }
            catch (Exception ex)
            {
                DeleteImageCommon(img);
                objMessage.Message = "Có lỗi khi cập nhật";
            }

            return Json(objMessage);
        }

        private void getDataShop(Shop shop)
        {
            if (!string.IsNullOrEmpty(Request["hdCity"]))
                shop.CityName = Request["hdCity"].ToString();
            if (!string.IsNullOrEmpty(Request["hdDistrict"]))
                shop.DistrictName = Request["hdDistrict"].ToString();
            if (!string.IsNullOrEmpty(Request["hdWard"]))
                shop.WardName = Request["hdWard"].ToString();

            if (!string.IsNullOrEmpty(Request["ddlCity"]))
                shop.CityCode = Request["ddlCity"];
            if (!string.IsNullOrEmpty(Request["ddlDistrict"]))
                shop.DistrictCode = Request["ddlDistrict"];
            if (!string.IsNullOrEmpty(Request["ddlWard"]))
                shop.WardCode = Request["ddlWard"];
        }
    }
}