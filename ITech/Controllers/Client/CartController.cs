using ITech.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITech.Controllers.Client
{
    public class CartController : BaseKHController
    {

        [HttpGet]
        public ActionResult ThemAddressDelivery()
        {

            return PartialView();
        }

        [HttpGet]
        public JsonResult GetAddressDelivery(int id)
        {
            var obj = GetObj.GetAddressDeliver(id);
            AddressResponse response = new AddressResponse()
            {
                Cus_name = obj.Cus_name,
                Cus_phone = obj.Cus_phone,

                ID = obj.ID,
                UserID = obj.UserID,
                Ward = obj.Ward,
                WardCode = obj.WardCode,
                District = obj.District,
                DistrictCode = obj.DistrictCode,
                City = obj.City,
                ApartmentNumber = obj.ApartmentNumber,
                Default = obj.Default
            };
            return Json(new objMessage(false, "", response), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ThemAddressDelivery_POST()
        {
            try
            {
                ITechEntities db = new ITechEntities();
                var address = new AddressDeliver();
                getDataAddressDeliver(address);
                address.UserID = mUserInfor.ID;
                db.AddressDelivers.Add(address);
                db.SaveChanges();
                if (address.Default == 1)
                {
                    db.Database.ExecuteSqlCommand("Update AddressDelivers set [Default]=0 where UserID=@UserID and ID != @ID", new SqlParameter[] {
                        new SqlParameter("@UserID", mUserInfor.ID),
                        new SqlParameter("@ID", address.ID)
                    });
                }
                return Json(new objMessage(false, "Thêm mới địa chỉ thành công"));
            }
            catch (Exception ex)
            {
                return Json(new objMessage(true, "Có lỗi khi thêm mới địa chỉ: " + ex.Message));

            }

        }

        public ActionResult EditAddressDelivery(int id)
        {
            ITechEntities db = new ITechEntities();
            return PartialView(db.AddressDelivers.SingleOrDefault(x => x.ID == id && x.UserID == mUserInfor.ID));
        }

        [HttpPost]
        public ActionResult UpdateAddressDelivery()
        {
            ITechEntities db = new ITechEntities();
            int id = int.Parse(Request["itemID"]);
            AddressDeliver ad = db.AddressDelivers.SingleOrDefault(x => x.ID == id && x.UserID == mUserInfor.ID);

            getDataAddressDeliver(ad);
            if (ad.Default == 1)
            {
                db.Database.ExecuteSqlCommand("Update AddressDelivers set [Default]=0 where UserID=@UserID and ID != @ID", new SqlParameter[] {
                        new SqlParameter("@UserID", mUserInfor.ID),
                        new SqlParameter("@ID", ad.ID)
                    });
            }
            db.SaveChanges();
            return Json(new objMessage(false, "Cập nhật địa chỉ thành công"));
        }

        public void getDataAddressDeliver(AddressDeliver address)
        {
            if (!string.IsNullOrEmpty(Request["Cus_name"]))
                address.Cus_name = Request["Cus_name"];
            if (!string.IsNullOrEmpty(Request["Cus_phone"]))
                address.Cus_phone = Request["Cus_phone"];
            if (!string.IsNullOrEmpty(Request["ApartmentNumber"]))
                address.ApartmentNumber = Request["ApartmentNumber"];
            if (!string.IsNullOrEmpty(Request["Default"]))
                address.Default = 1; //dia chi mac dinh
            else
            {
                address.Default = 0; //ko la dia chi mac dinh

            }
            if (!string.IsNullOrEmpty(Request["hdCity"]))
                address.City = Request["hdCity"].ToString().Trim();
            if (!string.IsNullOrEmpty(Request["hdDistrict"]))
                address.District = Request["hdDistrict"].ToString().Trim();
            if (!string.IsNullOrEmpty(Request["hdWard"]))
                address.Ward = Request["hdWard"].ToString().Trim();

            if (!string.IsNullOrEmpty(Request["ddlCity"]))
                address.CityCode = Request["ddlCity"].ToString().Trim();
            if (!string.IsNullOrEmpty(Request["ddlDistrict"]))
                address.DistrictCode = Request["ddlDistrict"].ToString().Trim();
            if (!string.IsNullOrEmpty(Request["ddlWard"]))
                address.WardCode = Request["ddlWard"].ToString().Trim();
        }

        // GET: Cart
        [HttpPost]
        public ActionResult AddToCart(int ProductID, int Quantity)
        {
            objMessage objMessage = new objMessage(true);
            try
            {
                ITechEntities db = new ITechEntities();
                var pro = db.Products.SingleOrDefault(x => x.ID == ProductID);
                if (pro.Shop.UserID == mUserInfor.ID)
                {
                    objMessage.Message = "Bạn không thể thêm chính sản phẩm của bạn vào giỏ hàng";
                    return Json(objMessage);

                }

                var obj = db.MyCarts.SingleOrDefault(x => x.CustomerID == mUserInfor.ID && x.ProductID == ProductID);
                if (obj != null)
                {
                    obj.Quantity += Quantity;
                    if (obj.Quantity > GetObj.GetProduct(ProductID).Inventory)
                    {
                        objMessage.Message = "Số lượng tồn của sản phẩm này không đủ để cung ứng với số lượng bạn đã đặt";
                        return Json(objMessage);
                    }
                }
                else
                {
                    db.MyCarts.Add(new MyCart() { CustomerID = mUserInfor.ID, ProductID = ProductID, Quantity = Quantity });
                }
                db.SaveChanges();

                objMessage.Error = false;
                objMessage.Message = "Đã thêm sản phẩm vào giỏ";
            }
            catch (Exception ex)
            {
                objMessage.Message = "Không thể thêm sản phẩm này vào giỏ " + ex.Message;
            }
            return Json(objMessage);
        }


        [HttpGet]
        public ActionResult Cart()
        {
            ITechEntities db = new ITechEntities();
            var cart = db.MyCarts.Where(x => x.CustomerID == mUserInfor.ID).ToList();
            ViewBag.Cart = db.MyCarts.Where(x => x.CustomerID == mUserInfor.ID).ToList();
            List<Shop> shops = new List<Shop>(); //tat ca cacs shop trong gio hang của khách
            foreach (var x in cart)
            {
                Shop ss = new Shop();
                shops.Add(db.Shops.FirstOrDefault(s => s.ID == x.Product.ShopID));
            }
            ViewBag.Shops = shops.Where(x => x.Status != HangSo.KhoaByShop).Distinct().ToList();

            ViewBag.Address = db.AddressDelivers.Where(x => x.UserID == mUserInfor.ID).OrderByDescending(x => x.Default).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult UpdateQuantity(int Qty, int Id)
        {
            ITechEntities db = new ITechEntities();
            var objMessage = new objMessage(true);
            try
            {
                //db.Database.ExecuteSqlCommand("update MyCarts set Quantity=@Qty where ID=@Id", new SqlParameter[]
                //{
                //     new SqlParameter("@Qty", Qty),
                //        new SqlParameter("@Id", Id)
                //});
                var obj = db.MyCarts.SingleOrDefault(x => x.ID == Id);
                obj.Quantity = Qty;
                double Price = obj.Product.Price.Value;
                var ThanhTien = string.Format("{0:#,###}", Price * obj.Quantity);
                var ThanhTienVal = Price * obj.Quantity;
                db.SaveChanges();
                objMessage.Error = false;
                objMessage.Data = new { ThanhTien, obj.ID, ThanhTienVal, obj.Quantity };
            }
            catch (Exception ex)
            {
                objMessage.Message = ex.Message;

            }

            return Json(objMessage, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult RemoveProductCart(int Id)
        {
            ITechEntities db = new ITechEntities();
            var objMessage = new objMessage(true);
            try
            {

                var obj = db.MyCarts.SingleOrDefault(x => x.ID == Id);
                objMessage.Error = false;
                objMessage.Data = new { obj.ID, obj.Product.ShopID };
                db.MyCarts.Remove(obj);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                objMessage.Message = ex.Message;

            }

            return Json(objMessage, JsonRequestBehavior.AllowGet);
        }




        public ActionResult CheckoutPage(string ArrID, int DiaChiNhanHang)
        {
            ITechEntities db = new ITechEntities();
            var objMessage = new objMessage(true);
            ViewBag.ArrID = ArrID;
            try
            {
                var Items = ArrID.Split(';').ToList();
                Items.Remove("");
                var itemInt = Items.Select(Int32.Parse).ToList();
                var SanPhamsOrder = db.MyCarts.Where(x => itemInt.Contains(x.ID) && x.CustomerID == mUserInfor.ID).ToList();
                ViewBag.DiaChiNhanHang = db.AddressDelivers.SingleOrDefault(x => x.ID == DiaChiNhanHang && x.UserID == mUserInfor.ID);
                ViewBag.SanPhamsOrder = SanPhamsOrder;
                List<Shop> shops = new List<Shop>(); //tat ca cacs shop trong gio hang của khách
                double totalMoney = 0;
                foreach (var x in SanPhamsOrder)
                {
                    Shop ss = new Shop();
                    shops.Add(db.Shops.FirstOrDefault(s => s.ID == x.Product.ShopID));
                    totalMoney += (double)(x.Quantity) * x.Product.Price.Value;
                }
                ViewBag.totalMoney = totalMoney;
                ViewBag.Address = db.AddressDelivers.Where(x => x.UserID == mUserInfor.ID).OrderByDescending(x => x.Default).ToList(); //lay tat ca dia chi cua user

                ViewBag.Shops = shops.Distinct().ToList();
            }
            catch (Exception ex)
            {
                objMessage.Message = ex.Message;

            }

            return View();
        }

        [HttpPost]
        public ActionResult GetInforCheckout(string ArrID, int DiaChiNhanHang)
        {
            ITechEntities db = new ITechEntities();
            var objMessage = new objMessage(true);

            var Items = ArrID.Split(';').ToList();
            Items.Remove("");
            var itemInt = Items.Select(Int32.Parse).ToList();
            var SanPhamsOrder = db.MyCarts.Where(x => itemInt.Contains(x.ID) && x.CustomerID == mUserInfor.ID)
                .Select(x => new { x.Product.Weight, x.Product.Height, x.Product.Width, x.Product.Length, x.Product.ShopID, x.Quantity, x.Product.Price }).ToList();

            var DC = db.AddressDelivers.SingleOrDefault(x => x.ID == DiaChiNhanHang && x.UserID == mUserInfor.ID);
            //ViewBag.SanPhamsOrder = SanPhamsOrder;

            // double totalMoney = 0;
            List<Shop> shops = new List<Shop>(); //tat ca cacs shop trong gio hang của khách
            foreach (var x in SanPhamsOrder)
            {
                Shop ss = new Shop();
                shops.Add(db.Shops.FirstOrDefault(s => s.ID == x.ShopID));
                // totalMoney += (double)(x.Quantity) * x.Product.Price.Value;
            }
            //ViewBag.totalMoney = totalMoney;
            //ViewBag.Address = db.AddressDelivers.Where(x => x.UserID == mUserInfor.ID).OrderByDescending(x => x.Default).ToList(); //lay tat ca dia chi cua user

            var lstShop = shops.Distinct().Select(x => new { x.WardCode, x.DistrictCode, x.CityCode, x.ID }).ToList();


            return Json(new objMessage(false, "", new { lstShop, SanPhamsOrder, DC.WardCode, DC.DistrictCode, DC.CityCode }));
        }


        [HttpPost]
        public ActionResult Checkout(List<CheckoutModel> request)
        {
            objMessage objMessage = new objMessage(true);
            using (ITechEntities context = new ITechEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (CheckoutModel tmp in request)
                        {
                            var address = GetObj.GetAddressDeliver(int.Parse(tmp.AddressDelivery));
                            Order o = new Order()
                            {
                                CustomerID = mUserInfor.ID,
                                ShopID = tmp.ShopID,
                                OrderDate = Convert.ToDateTime(DateTime.Now, objCultureInfo),
                                Address = address.ApartmentNumber + ", " + address.Ward + ", " + address.District + ", " + address.City,
                                Receiver = address.Cus_name,
                                Phone = address.Cus_phone,
                                Status = HangSo.ChoXacNhan,
                                FeeShip = double.Parse(tmp.FeeShip),
                                OrderCode = address.CityCode.Trim() + Convert.ToDateTime(DateTime.Now, objCultureInfo).ToString("yyyyMMddhhmm") + request.IndexOf(tmp)
                            };
                            context.Orders.Add(o);
                            context.SaveChanges();
                            foreach (MyCart item in tmp.OrderItem)
                            {
                                OrderDetail item2 = new OrderDetail()
                                {
                                    OrderID = o.ID,
                                    ProductID = item.ProductID,
                                    Quantity = item.Quantity
                                };
                                context.OrderDetails.Add(item2);
                                var mc = context.MyCarts.SingleOrDefault(x => x.ID == item.ID);
                                context.MyCarts.Remove(mc);
                            }
                            context.SaveChanges();
                        }


                        transaction.Commit();
                        objMessage.Error = false;
                        objMessage.Message = "Đã đặt hàng thành công";

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        objMessage.Message = "Có lỗi xảy ra khi đăt hàng";


                    }
                }
            }
            return Json(objMessage);
        }

        [HttpPost]
        public ActionResult CancelOrder(string id)
        {
            try
            {
                ITechEntities db = new ITechEntities();
                var o = db.Orders.SingleOrDefault(x => x.OrderCode == id && x.CustomerID == mUserInfor.ID);
                o.Status = HangSo.Xoa;
                o.CancelNote = "Hủy bới người mua hàng";
                db.SaveChanges();
                return Json(new objMessage(false, "Đã hủy đơn hàng " + id));
            }
            catch
            {
                return Json(new objMessage(true, "Hủy đơn hàng " + id + " không thành công"));

            }

        }

        public ActionResult ViewOrders()
        {
            ITechEntities db = new ITechEntities();
            var Orders = db.Orders.Where(x => (x.Status == HangSo.ChoXacNhan || x.Status == HangSo.DaXacNhan)
            && x.CustomerID == mUserInfor.ID)
                .OrderByDescending(x => x.OrderDate).ToList();
            var ods = Orders.Select(x => x.ID);
            ViewBag.OrderDetails = db.OrderDetails.Where(i => ods.Contains(i.OrderID.Value)).ToList();
            ViewBag.Orders = Orders;
            return View();
        }

        public ActionResult HistoryOrders(string tungay, string denngay, int page=1, int pageSize=3)
        {
            DateTime tn, dn; 
            if (!string.IsNullOrEmpty(tungay) && !string.IsNullOrEmpty(denngay))
            {

                tn = Convert.ToDateTime(tungay, objCultureInfo).AddDays(-1).AddHours(23).AddMinutes(59).AddSeconds(59);
                dn = Convert.ToDateTime(denngay, objCultureInfo).AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            else
            {
                tn = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"), objCultureInfo).AddDays(-30).AddHours(23).AddMinutes(59).AddSeconds(59);
                dn = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"), objCultureInfo).AddHours(23).AddMinutes(59).AddSeconds(59);
            }
            ITechEntities db = new ITechEntities();
            var Orders = db.Orders.Where(x => (x.Status != HangSo.ChoXacNhan && x.Status != HangSo.DaXacNhan)
            && x.CustomerID == mUserInfor.ID && (tn <= x.OrderDate && x.OrderDate <= dn))
                .OrderByDescending(x => x.OrderDate).ToPagedList(page, pageSize);
            var ods = Orders.Select(x => x.ID);
            ViewBag.OrderDetails = db.OrderDetails.Where(i => ods.Contains(i.OrderID.Value)).ToList();
            ViewBag.Orders = Orders;
            ViewBag.TuNgay = tn.ToString("dd/MM/yyyy");
            ViewBag.DenNgay = dn.ToString("dd/MM/yyyy");
            return View();


        }

        [HttpPost]
        public ActionResult ConfirmDelivery(string code)
        {
            ITechEntities db = new ITechEntities();
            var order = db.Orders.SingleOrDefault(x => x.OrderCode == code);
            if (order.Status == HangSo.DaXacNhan)
            {
                var items = db.OrderDetails.Where(x => x.OrderID == order.ID).ToList();
                foreach (OrderDetail i in items)
                {
                    var pro = db.Products.SingleOrDefault(x => x.ID == i.ProductID);
                    if (pro != null)
                    {
                        pro.Inventory -= i.Quantity;
                        db.SaveChanges();
                    }
                }
                order.Status = HangSo.DaNhanHang;
                db.SaveChanges();
                return Json(new objMessage(false, "Đã xác nhận đơn hàng " + code + " được giao tới người mua !"));
            }
            else
            {
                return Json(new objMessage(true, "Có lỗi xảy ra khi xác nhận đơn hàng được giao tới người mua"));
            }

        }
    }
}