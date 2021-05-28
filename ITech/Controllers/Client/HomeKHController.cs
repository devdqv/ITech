using ITech.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITech.Controllers.Client
{
    public class HomeKHController : Controller
    {
        // GET: HomeKH
        public ActionResult Index()
        {
            ITechEntities db = new ITechEntities();

            ViewBag.DanhMucUuTien = db.Categories.Where(x => x.ParentCode == 0 && x.isDeleted == 1).OrderBy(x => x.STT).Take(5).ToList();


            ViewBag.productQC = db.Products.Where(x => x.Shop.Status == HangSo.DaDuyet && x.ApproveStatus == HangSo.DaDuyet && x.Status == 1).OrderByDescending(x => x.Score).Take(10).ToList();


            return View();
        }

        public ActionResult SearchProduct(string TenSP = "", string danhmuc = "", string gia = "", int page = 1, int pageSize = 30)
        {
            ITechEntities db = new ITechEntities();
            Keyword k = db.Keywords.SingleOrDefault(x => x.KeyWord1.Trim().ToLower() == TenSP.Trim().ToLower());
            if(k!=null)
            {
                k.count++;  
            }
            else 
            {
                Keyword tmp = new Keyword();
                tmp.count = 1;
                tmp.KeyWord1 = TenSP;
                db.Keywords.Add(tmp);
            }
            db.SaveChanges();
            List<Product> products = new List<Product>();
            List<string> arrGia = new List<string>();
            if (!string.IsNullOrEmpty(gia))
            {
                arrGia = gia.Split('-').ToList();
            }
            if (arrGia.Count >= 1)
            {
                double giaMin = double.Parse(arrGia[0]);

                double giaMax = arrGia.Count == 1 ? -1 : double.Parse(arrGia[1]);
                if (!string.IsNullOrEmpty(danhmuc))
                {
                    var sp = db.SearchSanPhamByDanhMuc(int.Parse(danhmuc));

                    foreach (var s in sp)
                    {
                        Product tmp = db.Products.SingleOrDefault(x => x.ID == s.ID
                        && (x.Title.ToLower().Contains(TenSP) || string.IsNullOrEmpty(TenSP))
                        && ((x.Price >= giaMin && (giaMax == -1 ? x.Price > 0 : x.Price <= giaMax)))
                        );
                        if (tmp != null)
                        {
                            products.Add(tmp);
                        }
                    }
                    ViewBag.products = products.Where(x => (x.Shop.Status == HangSo.DaDuyet || x.Shop.Status == HangSo.KhoaByShop) && x.ApproveStatus == HangSo.DaDuyet && x.Status == 1).OrderByDescending(x => x.Sold).ToPagedList(page, pageSize);

                }
                else
                {
                    ViewBag.products = db.Products.Where(x => (x.Shop.Status == HangSo.DaDuyet || x.Shop.Status == HangSo.KhoaByShop) && x.ApproveStatus == HangSo.DaDuyet && x.Status == 1 && (x.Title.ToLower().Contains(TenSP.ToLower().Trim()) || string.IsNullOrEmpty(TenSP))
                     && ((x.Price >= giaMin && (giaMax == -1 ? x.Price > 0 : x.Price <= giaMax)))

                    )
                        .OrderByDescending(x => x.Sold).ToPagedList(page, pageSize);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(danhmuc))
                {
                    var sp = db.SearchSanPhamByDanhMuc(int.Parse(danhmuc));
                    foreach (var s in sp)
                    {
                        Product tmp = db.Products.SingleOrDefault(x => x.ID == s.ID
                        && (x.Title.ToLower().Contains(TenSP) || string.IsNullOrEmpty(TenSP))
                        );
                        if (tmp != null)
                        {
                            products.Add(tmp);
                        }
                    }
                    ViewBag.products = products.Where(x => (x.Shop.Status == HangSo.DaDuyet || x.Shop.Status == HangSo.KhoaByShop ) && x.ApproveStatus == HangSo.DaDuyet && x.Status==1).OrderByDescending(x => x.Sold).ToPagedList(page, pageSize);

                }
                else
                {
                    ViewBag.products = db.Products.Where(x =>(x.Shop.Status == HangSo.DaDuyet || x.Shop.Status == HangSo.KhoaByShop) && x.ApproveStatus == HangSo.DaDuyet && x.Status == 1 && (x.Title.ToLower().Contains(TenSP.ToLower().Trim()) || string.IsNullOrEmpty(TenSP))
                    )
                        .OrderByDescending(x => x.Sold).ToPagedList(page, pageSize);
                }
            }

            ViewBag.TenSP = TenSP;
            ViewBag.fieldSearch = TenSP;
            ViewBag.danhmuc = danhmuc;

            ViewBag.productQC = db.Products.Where(x=> x.Shop.Status == HangSo.DaDuyet && x.ApproveStatus == HangSo.DaDuyet && x.Status == 1).OrderByDescending(x => x.Score).Take(10).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult GetProducts(string key = "")
        {
            ITechEntities db = new ITechEntities();
            List<Keyword> keys = new List<Keyword>();
            if(!string.IsNullOrEmpty(key))
            {
                keys = db.Keywords.Where(x => x.count >= 20 && x.KeyWord1.Trim().ToLower().Contains(key.Trim().ToLower())).OrderByDescending(x=>x.count).ToList();
            }
                   
                    
            return Json(keys.Select(x => x.KeyWord1));
        }
        public ActionResult MenuPartial(string danhmuc)
        {
            ITechEntities db = new ITechEntities();
            ViewBag.danhmucs = db.Categories.Where(x => x.ParentCode != 0 && x.isDeleted == 1).ToList();
            ViewBag.danhmucC1 = db.Categories.Where(x => x.ParentCode == 0 && x.isDeleted == 1).ToList();
            ViewBag.danhmuc = danhmuc; //danhmuc timf kiem
            return PartialView();
        }


        public ActionResult ProductByShop(string ShopID, string TenSP = "", string gia = "", int page = 1, int pageSize = 30)
        {
            ITechEntities db = new ITechEntities();
            ViewBag.Shop = db.Shops.SingleOrDefault(x => x.ID == ShopID);
            ViewBag.ProductByShop = db.Products.Where(x => x.ShopID == ShopID).ToList();

            List<Product> products = new List<Product>();
            List<string> arrGia = new List<string>();
            if (!string.IsNullOrEmpty(gia))
            {
                arrGia = gia.Split('-').ToList();
            }
            if (arrGia.Count >= 1)
            {
                double giaMin = double.Parse(arrGia[0]);

                double giaMax = arrGia.Count == 1 ? -1 : double.Parse(arrGia[1]);
               
                    ViewBag.products = db.Products.Where(x => (x.Shop.Status == HangSo.DaDuyet || x.Shop.Status == HangSo.KhoaByShop) && (x.Title.ToLower().Contains(TenSP.ToLower().Trim()) || string.IsNullOrEmpty(TenSP) ) && x.ShopID == ShopID
                     && ((x.Price >= giaMin && (giaMax == -1 ? x.Price > 0 : x.Price <= giaMax)))
                    )
                        .OrderByDescending(x => x.Sold).ToPagedList(page, pageSize);
            }
            else
            {
                ViewBag.products = db.Products.Where(x => (x.Shop.Status == HangSo.DaDuyet || x.Shop.Status == HangSo.KhoaByShop) && (x.Title.ToLower().Contains(TenSP.ToLower().Trim()) || string.IsNullOrEmpty(TenSP)) && x.ShopID == ShopID)
                        .OrderByDescending(x => x.Sold).ToPagedList(page, pageSize);
            }

            ViewBag.TenSP = TenSP;
            ViewBag.fieldSearch = TenSP;

            ViewBag.productQC = db.Products.Where(x=> x.Shop.Status == HangSo.DaDuyet).OrderByDescending(x => x.Score).Take(10).ToList();
            return View();
        }

    }
}