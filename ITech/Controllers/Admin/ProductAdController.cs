using ITech.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITech.Controllers.Admin
{
    public class ProductAdController : BaseAdminController
    {
        EFUnitOfWork unitOfWork = new EFUnitOfWork();

        [HttpGet]
        public ActionResult ListProductChoDuyet(int page = 1, int pageSize = 15, string tenSP = "")
        {
            ProductRepository productRepository = new ProductRepository(new EFRepository<Product>(), unitOfWork);
            
                //Tat cả sp chờ duyệt với trạng thái không khóa
                ViewBag.ProductAllSearchCount = productRepository.All()
                .Where(x =>  x.ApproveStatus==1 && x.Status==1 
                && (string.IsNullOrEmpty(tenSP) || x.Title.Contains(tenSP))
                )
            .OrderBy(x => x.CreateDate).ToList().Count(); //so ban ghi tim kiem

                ViewBag.ProductAllSearch = productRepository.All()
            .Where(x =>  x.ApproveStatus==1 && x.Status==1
            && (string.IsNullOrEmpty(tenSP) || x.Title.Contains(tenSP))
            )
        .OrderBy(x => x.CreateDate).ToPagedList(page, pageSize);  //lay trang tim kiem
            
            ViewBag.TenSP = tenSP;
            return View();
        }

        [HttpGet]
        public ActionResult ListProductDaDuyet(int page = 1, int pageSize = 15, string tenSP = "")
        {
            ProductRepository productRepository = new ProductRepository(new EFRepository<Product>(), unitOfWork);

            //Tat cả sp chờ duyệt với trạng thái không khóa
            ViewBag.ProductAllSearchCount = productRepository.All()
            .Where(x => x.ApproveStatus == HangSo.DaDuyet && x.Status == 1
            && (string.IsNullOrEmpty(tenSP) || x.Title.Contains(tenSP))
            )
        .OrderBy(x => x.CreateDate).ToList().Count(); //so ban ghi tim kiem

            ViewBag.ProductAllSearch = productRepository.All()
        .Where(x => x.ApproveStatus == HangSo.DaDuyet && x.Status == 1
        && (string.IsNullOrEmpty(tenSP) || x.Title.Contains(tenSP))
        )
    .OrderBy(x => x.CreateDate).ToPagedList(page, pageSize);  //lay trang tim kiem

            ViewBag.TenSP = tenSP;
            return View();
        }

        [HttpGet]
        public ActionResult ListProductDaKhoa(int page = 1, int pageSize = 15, string tenSP = "")
        {
            ProductRepository productRepository = new ProductRepository(new EFRepository<Product>(), unitOfWork);

            //Tat cả sp chờ duyệt với trạng thái không khóa
            ViewBag.ProductAllSearchCount = productRepository.All()
            .Where(x => x.ApproveStatus == HangSo.DaKhoa && x.Status == 1
            && (string.IsNullOrEmpty(tenSP) || x.Title.Contains(tenSP))
            )
        .OrderBy(x => x.CreateDate).ToList().Count(); //so ban ghi tim kiem

            ViewBag.ProductAllSearch = productRepository.All()
        .Where(x => x.ApproveStatus == HangSo.DaKhoa && x.Status == 1
        && (string.IsNullOrEmpty(tenSP) || x.Title.Contains(tenSP))
        )
    .OrderBy(x => x.CreateDate).ToPagedList(page, pageSize);  //lay trang tim kiem

            ViewBag.TenSP = tenSP;
            return View();
        }



        [HttpPost]
        public JsonResult KhoaSP(int id)
        {
            objMessage objMessage = new objMessage();
            objMessage.Error = true;
            try
            {
                ITechEntities db = new ITechEntities();
                var pro = db.Products.SingleOrDefault(x => x.ID == id);

                if (pro != null)
                    pro.ApproveStatus = HangSo.DaKhoa;
               
                db.SaveChanges();
                objMessage.Error = false;
                objMessage.Message = "Đã khóa sản phẩm " + pro.Title;
            }
            catch
            {
                objMessage.Message = "Không thể khóa sản phẩm này";
            }
            return Json(objMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MoKhoaSP(int id)
        {
            objMessage objMessage = new objMessage();
            objMessage.Error = true;
            try
            {
                ITechEntities db = new ITechEntities();
                var pro = db.Products.SingleOrDefault(x => x.ID == id);

                if (pro != null)
                    pro.ApproveStatus = HangSo.ChoDuyet;

                db.SaveChanges();
                objMessage.Error = false;
                objMessage.Message = "Đã mở khóa sản phẩm " + pro.Title;
            }
            catch
            {
                objMessage.Message = "Không thể mở khóa sản phẩm này";
            }
            return Json(objMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DuyetSP(int id)
        {
            objMessage objMessage = new objMessage();
            objMessage.Error = true;
            try
            {
                ITechEntities db = new ITechEntities();
                var pro = db.Products.SingleOrDefault(x => x.ID == id);
                pro.ApproveStatus = HangSo.DaDuyet;

                db.SaveChanges();
                objMessage.Error = false;
                objMessage.Message = "Đã duyệt sản phẩm " + pro.Title;
            }
            catch
            {
                objMessage.Message = "Duyệt không thành công";
            }
            return Json(objMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TuChoiSP(int id)
        {
            objMessage objMessage = new objMessage();
            objMessage.Error = true;
            try
            {
                ITechEntities db = new ITechEntities();
                var pro = db.Products.SingleOrDefault(x => x.ID == id);
                pro.ApproveStatus = HangSo.TuChoi;
                db.SaveChanges();
                objMessage.Error = false;
                objMessage.Message = "Đã từ chối sản phẩm " + pro.Title;
            }
            catch
            {
                objMessage.Message = "Không thể từ chối sản phẩm này";
            }
            return Json(objMessage, JsonRequestBehavior.AllowGet);
        }


        int itemID { get; set; }
        int itemID2 { get; set; }
        public ActionResult ProductDetails(int id, string idShop)
        {
            ITechEntities db = new ITechEntities();
            var pro = db.Products.SingleOrDefault(x => x.ID == id && x.ShopID == idShop);
            if (pro != null)
            {
                var itemPro_cats = db.Product_Category.Where(x => x.ProductID == id && x.status == 1).Select(x => x.CatID).ToList();
                if (itemPro_cats[0] < itemPro_cats[1]) //nhỏ hơn là DM cha
                {
                    itemID = itemPro_cats[0].Value;
                    itemID2 = itemPro_cats[1].Value;
                }
                else
                {
                    itemID = itemPro_cats[1].Value;
                    itemID2 = itemPro_cats[0].Value;
                }

                ViewBag.ItemID = itemID;
                ViewBag.ItemID2 = itemID2;
               
                ViewBag.specs = db.Specifications.Where(x => x.CategoryID == itemID2).ToList();
                ViewBag.specVals = db.SpecValues.ToList();
                ViewBag.product = pro;

                //Hinh anh
                var anhtmps = db.Galleries.Where(x => x.ProductID == id).Select(x => x.Image).ToList();
                string[] Anhs = new string[5];
                for (int i = 0; i < anhtmps.Count(); i++)
                {
                    Anhs[i] = anhtmps[i];
                }
                ViewBag.Anhs = Anhs;
                ViewBag.Pro_spec = db.Product_Spec.Where(x => x.ProductID == id).ToList();
            }
            return View();
        }

       
    }
}