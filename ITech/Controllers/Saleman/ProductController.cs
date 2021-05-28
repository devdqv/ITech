using ITech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity;
using System.Data.SqlClient;

namespace ITech.Controllers.Saleman
{
    public class ProductController : BaseController
    {
        EFUnitOfWork unitOfWork = new EFUnitOfWork();

        public ActionResult ListProductCanXL(int type)
        {
            ITechEntities db = new ITechEntities();
            ViewBag.lstShop = db.Shops.Where(x => x.UserID == mUserInforBH.ID && x.Status == HangSo.DaDuyet).ToList();
            ViewBag.type = type;
            return View();
        }

        public ActionResult Before(string ShopID = "")
        {
            CategoryRepository categoryRepository = new CategoryRepository(new EFRepository<Category>(), unitOfWork);
            var lstDM = categoryRepository.All().Where(x => x.ParentCode == 0 && x.isDeleted == HangSo.OK).ToList();
            ViewBag.ShopID = ShopID;
            return View(lstDM);
        }

        [HttpPost]
        public ActionResult getListDMCap2(int ID)
        {
            CategoryRepository categoryRepository = new CategoryRepository(new EFRepository<Category>(), unitOfWork);
            var lstDM = categoryRepository.All().Where(x => x.ParentCode == ID && x.isDeleted == 1).ToList();
            string html = "";
            foreach (var x in lstDM)
            {
                html += "<option value=" + x.ID + ">" + x.Name + "</option>";
            }
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        int itemID { get; set; }
        int itemID2 { get; set; }
        public List<string> anhtmps = new List<string>();
        Product productTmp;
        List<string> lstTSKT;
        // GET: Product

        public ActionResult Index(string ShopID, int ItemID, int ItemID2)
        {
            //BrandRepository brandRepository = new BrandRepository(new EFRepository<Brand>(), unitOfWork);
            SpecificationRepository SpecificationRepository = new SpecificationRepository(new EFRepository<Specification>(), unitOfWork);
            SpecValueRepository SpecValueRepository = new SpecValueRepository(new EFRepository<SpecValue>(), unitOfWork);
            itemID = ItemID;
            itemID2 = ItemID2;
            ViewBag.ItemID = ItemID;
            ViewBag.ItemID2 = ItemID2;
            ViewBag.ShopID = ShopID;
            //ViewBag.Brands = brandRepository.All().Where(x => x.CategoryID == ItemID);
            ViewBag.specs = SpecificationRepository.All().Where(x => x.CategoryID == ItemID2);
            ViewBag.specVals = SpecValueRepository.All();
            return View();
        }

        public ActionResult EditProduct(int id, string ShopID)
        {
            ITechEntities db = new ITechEntities();
            if (db.Shops.SingleOrDefault(x => x.UserID == mUserInforBH.ID && x.ID == ShopID) != null)
            {

                var pro = db.Products.SingleOrDefault(x => x.ID == id && x.ShopID == ShopID);

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
                    //ViewBag.Brands = db.Brands.Where(x => x.CategoryID == itemID2).ToList();
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
                    ViewBag.ShopID = ShopID;
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult EditProduct()
        {
            try
            {
                ITechEntities db = new ITechEntities();
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(Request["ArrID"]))
                            ArrID = int.Parse(Request["ArrID"]);
                        if (!string.IsNullOrEmpty(Request["Approved"]))
                            Approved = int.Parse(Request["Approved"]);
                        if (!string.IsNullOrEmpty(Request["ShopID"]))
                            ShopID = Request["ShopID"];
                        if (db.Shops.SingleOrDefault(x => x.UserID == mUserInforBH.ID && x.ID == ShopID) != null)
                        {
                            productTmp = db.Products.SingleOrDefault(x => x.ID == ArrID && x.ShopID == ShopID);
                            if (Approved == 2)
                            {
                                if (!string.IsNullOrEmpty(Request["GiaCu"]))
                                    productTmp.OldPrice = double.Parse(Request["GiaCu"].Replace(",", ""));
                                if (!string.IsNullOrEmpty(Request["GiaMoi"]))
                                    productTmp.Price = double.Parse(Request["GiaMoi"].Replace(",", ""));
                                if (!string.IsNullOrEmpty(Request["KhoHang"]))
                                    productTmp.Inventory = int.Parse(Request["KhoHang"]);

                            }
                            else if (Approved == HangSo.ChoDuyet|| Approved == HangSo.TuChoi)
                            {
                                GetParameters(productTmp);

                                var filenames = UploadImage();
                                List<string> imgsName = null;
                                if (filenames != "-1" && filenames != "0") //khong co anh hoac error
                                {
                                    imgsName = filenames.Split(';').ToList();
                                    if (string.IsNullOrEmpty(anhtmps[0]))
                                    {
                                        productTmp.Image = imgsName[0];
                                        imgsName.RemoveAt(0);
                                    }
                                }


                                db.SaveChanges();
                                if (lstTSKT != null)
                                {
                                    var lst = db.Product_Spec.Where(x => x.ProductID == ArrID).ToList();
                                    db.Product_Spec.RemoveRange(lst);
                                    db.SaveChanges();
                                    foreach (string x in lstTSKT)
                                    {
                                        if (!string.IsNullOrEmpty(x))
                                        {
                                            Product_Spec ps = new Product_Spec();
                                            ps.ProductID = productTmp.ID;
                                            ps.SpecID = int.Parse(x.Split('=')[0]);
                                            ps.Value = x.Split('=')[1];
                                            db.Product_Spec.Add(ps);
                                        }

                                    }
                                    db.SaveChanges();
                                }


                                var param = new SqlParameter[]
                                    {
                                new SqlParameter("@i1", anhtmps[1]),
                                new SqlParameter("@i2", anhtmps[2]),
                                new SqlParameter("@i3", anhtmps[3]),
                                new SqlParameter("@i4", anhtmps[4]),
                                new SqlParameter("@i5", anhtmps[5])
                                    };
                                var delAnh = db.Galleries.Where(x => x.ProductID == ArrID).Select(x => x.Image).ToList();
                                foreach (var x in delAnh)
                                {
                                    if (!anhtmps.Contains(x))
                                        DeleteImage(x);
                                }

                                var del = db.Database.ExecuteSqlCommand("delete Galleries where Image!=@i1 and Image!=@i2 and Image!=@i3 and Image!=@i4 and Image!=@i1", param);




                                //add tên ảnh vào db
                                if (imgsName != null)
                                {
                                    foreach (var x in imgsName)
                                    {
                                        if (!string.IsNullOrEmpty(x))
                                        {

                                            Gallery g = new Gallery()
                                            {
                                                Image = x,
                                                ProductID = productTmp.ID
                                            };
                                            db.Galleries.Add(g);
                                        }
                                    }
                                    db.SaveChanges();
                                }
                                //Product_Category pc = new Product_Category()
                                //{
                                //    ProductID = productTmp.ID,
                                //    CatID = itemID,
                                //    status = 1
                                //};
                                //Product_Category pc2 = new Product_Category()
                                //{
                                //    ProductID = productTmp.ID,
                                //    CatID = itemID2,
                                //    status = 1
                                //};
                                //db.Product_Category.Add(pc);
                                //db.Product_Category.Add(pc2);
                            }
                            else
                            {
                                return Json(new objMessage() { Error = true, Message = "Bạn không thể cập nhật sản phẩm đã bị khóa" });
                            }

                            db.SaveChanges();

                            //var objTmp = db.Products.SingleOrDefault(x => x.ParrentID == productTmp.ID);
                            //db.Products.Remove(objTmp);
                            ////tạo bản sao để gửi tới quản trị
                            //productTmp.ParrentID = productTmp.ID;
                            //productTmp.ApproveStatus = 1; //chua duyet
                            //db.Products.Add(productTmp);
                            //db.SaveChanges();
                            transaction.Commit();
                            return Json(new objMessage() { Error = false, Message = "Cập nhật thành công" });
                        }
                        else
                        {
                            transaction.Rollback();
                            return Json(new objMessage() { Error = true, Message = "Bạn không có quyền cập nhật sản phẩm này" });

                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Json(new objMessage() { Error = true, Message = "Cập nhật không thành công: " + ex.Message });

                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new objMessage() { Error = true, Message = "Cập nhật không thành công: " + ex.Message });

            }
        }


        [HttpPost]
        public JsonResult AddProduct()
        {
            try
            {
                ITechEntities db = new ITechEntities();
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    List<string> imgsName = new List<string>();
                    productTmp = new Product();
                    GetParameters(productTmp);
                    var filenames = UploadImage();
                    imgsName = filenames.Split(';').ToList();
                    try
                    {

                        productTmp.Image = imgsName[0];
                        productTmp.ApproveStatus = 1; //chua dc duyet
                        productTmp.ShopID = ShopID.Trim();
                        productTmp.CreateDate = Convert.ToDateTime(DateTime.Now, objCultureInfo);
                        productTmp.Status = 1;// Hiển thị sản phẩm
                        imgsName.RemoveAt(0);

                        db.Products.Add(productTmp);
                        db.SaveChanges();
                        if (lstTSKT != null)
                        {
                            foreach (string x in lstTSKT)
                            {
                                if (!string.IsNullOrEmpty(x))
                                {
                                    Product_Spec ps = new Product_Spec();
                                    ps.ProductID = productTmp.ID;
                                    ps.SpecID = int.Parse(x.Split('=')[0]);
                                    ps.Value = x.Split('=')[1];
                                    db.Product_Spec.Add(ps);
                                }

                            }
                            db.SaveChanges();
                        }

                        //add tên ảnh vào db
                        if (imgsName != null)
                        {
                            foreach (var x in imgsName)
                            {
                                if (!string.IsNullOrEmpty(x))
                                {

                                    Gallery g = new Gallery()
                                    {
                                        Image = x,
                                        ProductID = productTmp.ID
                                    };
                                    db.Galleries.Add(g);
                                }
                            }
                            db.SaveChanges();
                        }

                        Product_Category pc = new Product_Category()
                        {
                            ProductID = productTmp.ID,
                            CatID = itemID,
                            status = 1
                        };
                        Product_Category pc2 = new Product_Category()
                        {
                            ProductID = productTmp.ID,
                            CatID = itemID2,
                            status = 1
                        };
                        db.Product_Category.Add(pc);
                        db.Product_Category.Add(pc2);
                        db.SaveChanges();

                        ////tạo bản sao để gửi tới quản trị
                        //productTmp.ParrentID = productTmp.ID;
                        //productTmp.ApproveStatus = 1; //chua duyet
                        //db.Products.Add(productTmp);
                        db.SaveChanges();
                        transaction.Commit();
                        return Json(new objMessage() { Error = false, Message = "Thêm mới thành công" });

                    }
                    catch (Exception ex)
                    {
                        if (imgsName != null)
                        {
                            foreach (var x in imgsName)
                            {
                                if (!string.IsNullOrEmpty(x))
                                {

                                    DeleteImage(x);
                                }
                            }
                        }
                        transaction.Rollback();
                        return Json(new objMessage() { Error = true, Message = "Thêm mới không thành công: " + ex.Message });

                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new objMessage() { Error = true, Message = "Thêm mới không thành công: " + ex.Message });

            }
        }


        [HttpGet]
        public ActionResult ListProduct(string ShopID = "", int page = 1, int pageSize = 15, string tenSP = "", int trangthai = 20, int type=0)
        {
            ViewBag.type = type + "";
            ProductRepository productRepository = new ProductRepository(new EFRepository<Product>(), unitOfWork);
            ITechEntities db = new ITechEntities();
            if (db.Shops.SingleOrDefault(x => x.ID == ShopID && x.UserID == mUserInforBH.ID) != null)
            {

                if (trangthai == 21) //còn hàng
                {
                    ViewBag.ProductAllSearchCount = productRepository.All()
                .Where(x => x.ParrentID == null && x.ShopID == ShopID
                && (string.IsNullOrEmpty(tenSP) || x.Title.Contains(tenSP))
                && x.Inventory > 0
                ).ToList().Count();

                    ViewBag.ProductAllSearch = productRepository.All()
                .Where(x => x.ParrentID == null && x.ShopID == ShopID
                && (string.IsNullOrEmpty(tenSP) || x.Title.Contains(tenSP))
                && x.Inventory > 0
                )
            .OrderByDescending(x => x.Status).ThenByDescending(x => x.CreateDate).ToPagedList(page, pageSize);  //lay trang tim kiem
                }
                else if (trangthai == HangSo.HetHang || type==HangSo.HetHang) //het hang
                {
                    ViewBag.ProductAllSearchCount = productRepository.All()
                .Where(x => x.ParrentID == null && x.ShopID == ShopID
                && (string.IsNullOrEmpty(tenSP) || x.Title.Contains(tenSP))
                && x.Inventory == 0
                ).ToList().Count();


                    ViewBag.ProductAllSearch = productRepository.All()
               .Where(x => x.ParrentID == null && x.ShopID == ShopID
               && (string.IsNullOrEmpty(tenSP) || x.Title.Contains(tenSP))
               && x.Inventory == 0
               )
           .OrderByDescending(x => x.Status).ThenByDescending(x => x.CreateDate).ToPagedList(page, pageSize);  //lay trang tim kiem
                }
                else if (trangthai == HangSo.DaKhoa || type== HangSo.DaKhoa) //tạm khóa
                {
                    ViewBag.ProductAllSearchCount = productRepository.All()
                .Where(x => x.ParrentID == null && x.ShopID == ShopID
                && (string.IsNullOrEmpty(tenSP) || x.Title.Contains(tenSP))
                && x.ApproveStatus == HangSo.DaKhoa
                ).ToList().Count();


                    ViewBag.ProductAllSearch = productRepository.All()
               .Where(x => x.ParrentID == null && x.ShopID == ShopID
               && (string.IsNullOrEmpty(tenSP) || x.Title.Contains(tenSP))
               && x.ApproveStatus == HangSo.DaKhoa
               )
           .OrderByDescending(x => x.Status).ThenByDescending(x => x.CreateDate).ToPagedList(page, pageSize);  //lay trang tim kiem

                }
                else if (trangthai == 24) //tạm ẩn
                {
                    ViewBag.ProductAllSearchCount = productRepository.All()
                .Where(x => x.ParrentID == null && x.ShopID == ShopID
                && (string.IsNullOrEmpty(tenSP) || x.Title.Contains(tenSP))
                && x.Status == 0
                ).ToList().Count();


                    ViewBag.ProductAllSearch = productRepository.All()
               .Where(x => x.ParrentID == null && x.ShopID == ShopID
               && (string.IsNullOrEmpty(tenSP) || x.Title.Contains(tenSP))
               && x.Status == 0
               )
           .OrderByDescending(x => x.Status).ThenByDescending(x => x.CreateDate).ToPagedList(page, pageSize);  //lay trang tim kiem
                }
                else
                {
                    //Tat cả
                    ViewBag.ProductAllSearchCount = productRepository.All()
                    .Where(x => x.ParrentID == null && x.ShopID == ShopID
                    && (string.IsNullOrEmpty(tenSP) || x.Title.Contains(tenSP))
                    )
                .OrderBy(x => x.CreateDate).ToList().Count(); //so ban ghi tim kiem

                    ViewBag.ProductAllSearch = productRepository.All()
                .Where(x => x.ParrentID == null && x.ShopID == ShopID
                && (string.IsNullOrEmpty(tenSP) || x.Title.Contains(tenSP))
                )
            .OrderByDescending(x => x.Status).ThenByDescending(x => x.CreateDate).ToPagedList(page, pageSize);  //lay trang tim kiem
                }




                ViewBag.ProductAll = productRepository.All()
                .Where(x => x.ParrentID == null && x.ShopID == ShopID)
                .OrderBy(x => x.CreateDate).ToList();
            }

            ViewBag.TenSP = tenSP;
            ViewBag.TrangThai = trangthai;
            ViewBag.ShopID = ShopID;
            return View();
        }

        [HttpPost]
        public JsonResult Delete(int id, string ShopID)
        {
            objMessage objMessage = new objMessage();
            objMessage.Error = true;

            ITechEntities db = new ITechEntities();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    var pro = db.Products.SingleOrDefault(x => x.ID == id && x.ShopID == ShopID);
                    if (pro != null)
                        db.Products.Remove(pro);

                    var delAnh = db.Galleries.Where(x => x.ProductID == id).Select(x => x.Image).ToList();
                    foreach (var x in delAnh)
                    {
                        DeleteImage(x);
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    objMessage.Error = false;
                    objMessage.Message = "Xóa thành công sản phẩm " + pro.Title;
                }
                catch
                {
                    transaction.Rollback();
                    objMessage.Message = "Xóa không thành công";
                }
            }
            return Json(objMessage);
        }

        [HttpPost]
        public JsonResult HideShowProduct(int id)
        {
            objMessage objMessage = new objMessage();
            objMessage.Error = true;

            ITechEntities db = new ITechEntities();
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {

                    var pro = db.Products.SingleOrDefault(x => x.ID == id);
                    if (pro != null)
                        pro.Status = pro.Status == 1 ? 0 : 1;

                    db.SaveChanges();
                    transaction.Commit();
                    objMessage.Error = false;
                    if (pro.Status == HangSo.DaAn)
                        objMessage.Message = "Đã ẩn sản phẩm " + pro.Title;
                    else
                        objMessage.Message = "Đã hiển thị sản phẩm " + pro.Title;
                }
                catch
                {
                    transaction.Rollback();
                    objMessage.Message = "Không thể ẩn/hiện sản phẩm này !";
                }
            }
            return Json(objMessage);
        }

        public int ArrID { get; set; }
        public int Approved { get; set; }
        public string ShopID { get; private set; }

        private void GetParameters(Product product)
        {


            if (!string.IsNullOrEmpty(Request["TenSP"]))
                product.Title = Request["TenSP"];
            if (!string.IsNullOrEmpty(Request["ShopID"]))
                ShopID = Request["ShopID"];
            if (!string.IsNullOrEmpty(Request["level1"]))
                itemID = int.Parse(Request["level1"]);
            if (!string.IsNullOrEmpty(Request["level2"]))
                itemID2 = int.Parse(Request["level2"]);
            if (!string.IsNullOrEmpty(Request.Unvalidated["MotaSP"]))
                product.Description = Request.Unvalidated["MotaSP"];
            if (!string.IsNullOrEmpty(Request["lstTSKT"]))
                lstTSKT = Request["lstTSKT"].ToString().Split(';').ToList();

            if (!string.IsNullOrEmpty(Request["GiaCu"]))
                product.OldPrice = double.Parse(Request["GiaCu"].Replace(",", ""));
            if (!string.IsNullOrEmpty(Request["GiaMoi"]))
                product.Price = double.Parse(Request["GiaMoi"].Replace(",", ""));
            if (!string.IsNullOrEmpty(Request["KhoHang"]))
                product.Inventory = int.Parse(Request["KhoHang"]);

            if (!string.IsNullOrEmpty(Request["statusProduct"]))
                product.StatusProduct = Request["statusProduct"];

            if (!string.IsNullOrEmpty(Request["SKUSanPham"]))
                product.SKU = Request["SKUSanPham"];
            if (!string.IsNullOrEmpty(Request["Length"]))
                product.Length = double.Parse(Request["Length"]);
            if (!string.IsNullOrEmpty(Request["Height"]))
                product.Height = double.Parse(Request["Height"]);
            if (!string.IsNullOrEmpty(Request["Weight"]))
                product.Weight = double.Parse(Request["Weight"]);
            if (!string.IsNullOrEmpty(Request["Width"]))
                product.Width = double.Parse(Request["Width"]);



            anhtmps.Add(Request["hdfileuploadA"]);
            anhtmps.Add(Request["hdfileupload1"]);
            anhtmps.Add(Request["hdfileupload2"]);
            anhtmps.Add(Request["hdfileupload3"]);
            anhtmps.Add(Request["hdfileupload4"]);
            anhtmps.Add(Request["hdfileupload5"]);

            product.Status = HangSo.OK;
            product.ApproveStatus = HangSo.ChoDuyet;
        }
    }
}