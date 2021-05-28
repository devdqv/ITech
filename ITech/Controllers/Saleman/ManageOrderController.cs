using ITech.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITech.Controllers.Saleman
{
    public class ManageOrderController : BaseController
    {
        public string PathPage { get; private set; }
        public int CurrentPage { get; private set; }
        public int RowPerPage { get; private set; }
        public string ShopID { get; private set; }
        public int Status { get; private set; }
        public DateTime TuNgay { get; private set; }
        public DateTime DenNgay { get; private set; }
        public string MaDH { get; private set; }



     
        public ActionResult ListShopCanXuLy(int type)
        {
            ITechEntities db = new ITechEntities();
            ViewBag.lstShop = db.Shops.Where(x => x.UserID == mUserInforBH.ID && x.Status==HangSo.DaDuyet).ToList();
            ViewBag.type = type;
            return View();
        }
       

        
        public ActionResult ListOrderByShop(string shopID, int type = 0)
        {

            ViewBag.ShopID = shopID;
            ITechEntities db = new ITechEntities();

            ViewBag.iTotalrecodsDaBiHuy = (int)db.sp_CountListOrderByShop(shopID, HangSo.DaBiHuy, "", TuNgay, DenNgay, 0, 0).First().Value;
            ViewBag.iTotalrecodsChoDuyet = (int)db.sp_CountListOrderByShop(shopID, HangSo.ChoXacNhan,"", TuNgay, DenNgay, 0, 0).First().Value;
            ViewBag.iTotalrecodsDaXacNhan = (int)db.sp_CountListOrderByShop(shopID, HangSo.DaXacNhan,"", TuNgay, DenNgay, 0, 0).First().Value;
            ViewBag.type = type+"";
            return View();
        }

        public ActionResult pList()
        {
            List<OrderModel> lstModel = new List<OrderModel>();

            GetParameters();
            int iTotalrecods = 0;
            EFUnitOfWork unitOfWork = new EFUnitOfWork();
            ITechEntities db = new ITechEntities();

            
            List<sp_ListOrderByShop_Result> lstOrder = db.sp_ListOrderByShop(ShopID, Status, MaDH, TuNgay, DenNgay, RowPerPage, CurrentPage-1).ToList();
            foreach (sp_ListOrderByShop_Result x in lstOrder)
            {
                OrderModel model = new OrderModel();
                model.order = new sp_ListOrderByShop_Result();
                model.order = x;
                model.orderItems = db.Database.SqlQuery<sp_ListOrderItemByOrder_Result>("exec sp_ListOrderItemByOrder @OrderID",
                new SqlParameter[] { new SqlParameter("@OrderID", x.ID)}).ToList();
                lstModel.Add(model);
            }

           
            ViewBag.lstModel = lstModel;
             
            //Count ALl từng loại
            iTotalrecods = (int)db.sp_CountListOrderByShop(ShopID, Status, MaDH, TuNgay, DenNgay, RowPerPage, CurrentPage).First().Value;
            //ViewBag.iTotalrecods0 = iTotalrecods0;
            //ViewBag.iTotalrecods_1 = iTotalrecods_1;
            //get html phân trang
            PathPage = string.Format("#ShopID={0}&RowPerPage={1}&Page=", ShopID, RowPerPage);
            ViewBag.ltlPage = Pager.getPage(PathPage, CurrentPage, RowPerPage, iTotalrecods); //cho xac nhan
            ViewBag.ShopID = ShopID;
            return PartialView();
        }

        private void GetParameters()
        {
            PathPage = "#";

            if (!string.IsNullOrEmpty(Request["page"]))
            {
                CurrentPage = Convert.ToInt32(Request["page"]);
            }
            else
                CurrentPage = 1;
            if (!string.IsNullOrEmpty(Request["RowPerPage"]))
            {
                RowPerPage = Convert.ToInt32(Request["RowPerPage"]);
            }
            RowPerPage = (RowPerPage <= 0 || RowPerPage > 200) ? 15 : RowPerPage;

            if (!string.IsNullOrEmpty(Request["shopID"]))
                ShopID = Request["shopID"];
            if (!string.IsNullOrEmpty(Request["status"]))
                Status = int.Parse(Request["status"]);

            if (!string.IsNullOrEmpty(Request["status"]))
                Status = int.Parse(Request["status"]);



            if (!string.IsNullOrEmpty(Request["MaDH"]))
                MaDH = Request["MaDH"];
            else MaDH = "";
            if (!string.IsNullOrEmpty(Request["TuNgay"]))
                TuNgay = Convert.ToDateTime(Request["TuNgay"], objCultureInfo);
            if (!string.IsNullOrEmpty(Request["DenNgay"]))
                DenNgay = Convert.ToDateTime(Request["DenNgay"], objCultureInfo);

        }

        [HttpPost]

        public ActionResult Confirm()
        {
            int stt = int.Parse(Request["stt"]);
            string OrderCode =Request["OrderCode"];
            ITechEntities db = new ITechEntities();
            var o= db.Orders.SingleOrDefault(x => x.OrderCode == OrderCode);
           
            if(o!=null)
            {
                if (stt == 1) //xac nhận
                {
                    o.Status = HangSo.DaXacNhan;
                }
                else if(stt== -1)
                {
                    o.Status = HangSo.DaBiHuy;
                    o.CancelNote = "Hủy bởi người bán";
                }
                db.SaveChanges();
                return Json(new objMessage(false, stt==1?"Xác nhận đơn hàng thành công!":"Hủy đơn hàng thành công !"));

            }
            else
            return Json(new objMessage(true, "Lỗi, Không tìm thấy đơn hàng. Vui lòng thử lại"));
        }
    }

   
}