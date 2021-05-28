using ITech.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITech.Controllers.Client
{
    public class ProductKHController : Controller
    {
        // GET: ProductKH

        int itemID { get; set; }
        int itemID2 { get; set; }
        public ActionResult Details(int id)
        {
            ITechEntities db = new ITechEntities();
               
            var product = db.Products.SingleOrDefault(x => x.ID == id && x.Status == 1 && x.ApproveStatus == HangSo.DaDuyet);
            if(product!=null)
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
                //ViewBag.Brands = db.Brands.Where(x => x.CategoryID == itemID2).ToList();
                ViewBag.specs = db.Specifications.Where(x => x.CategoryID == itemID2).ToList();
                


            }
            ViewBag.AnhSP = db.Galleries.Where(x => x.ProductID == product.ID).ToList();
            string ShopID = GetObj.GetSaleman(product.ShopID).ID;
            ViewBag.ProductTotal = db.Products.Where(x => x.Status == 1 && x.ApproveStatus == HangSo.DaDuyet && x.ShopID== ShopID).Count();
            ViewBag.Pro_spec = db.Product_Spec.Where(x => x.ProductID == id).ToList();

            EFUnitOfWork unitOfWork = new EFUnitOfWork();
            List<SearchSanPhamTuongTu_Result> sptt = db.SearchSanPhamTuongTu(product.ID).ToList();

            ViewBag.SPTuongTu = sptt;

            return View(product);
        }


        
        public ActionResult CompareProduct(int product1, int product2, int category)
        {
            ITechEntities db = new ITechEntities();
            
            ViewBag.SP1 = db.Products.SingleOrDefault(x => x.ID == product1);
            ViewBag.SP2 = db.Products.SingleOrDefault(x => x.ID == product2);
            ViewBag.ThongSoSP = db.Specifications.Where(x => x.CategoryID == category).ToList();
            ViewBag.ThongSoSP1 = db.Product_Spec.Where(x => x.ProductID == product1).ToList();
            ViewBag.ThongSoSP2 = db.Product_Spec.Where(x => x.ProductID == product2).ToList();
            return View();
        }
       

        
    }
}