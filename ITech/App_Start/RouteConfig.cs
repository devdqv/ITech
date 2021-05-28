using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ITech
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapMvcAttributeRoutes();   


            //Khách hàng

            routes.MapRoute(
     name: "san-pham-cua-hang",
     url: "san-pham-cua-hang",
     defaults: new { controller = "HomeKH", action = "ProductByShop", id = UrlParameter.Optional }
 );

            routes.MapRoute(
      name: "theo-doi-don-hang",
      url: "theo-doi-don-hang",
      defaults: new { controller = "Cart", action = "ViewOrders", id = UrlParameter.Optional }
  );

            routes.MapRoute(
      name: "lich-su-mua-hang",
      url: "lich-su-mua-hang",
      defaults: new { controller = "Cart", action = "HistoryOrders", id = UrlParameter.Optional }
  );

            routes.MapRoute(
      name: "thanh-toan",
      url: "thanh-toan",
      defaults: new { controller = "Cart", action = "CheckoutPage", id = UrlParameter.Optional }
  );

            routes.MapRoute(
      name: "thay-doi-thong-tin",
      url: "thay-doi-thong-tin",
      defaults: new { controller = "UserInfo", action = "ChangeInfor", id = UrlParameter.Optional }
  );

            routes.MapRoute(
        name: "so-sanh-san-pham",
        url: "so-sanh-san-pham",
        defaults: new { controller = "ProductKH", action = "CompareProduct", id = UrlParameter.Optional }
    );

            routes.MapRoute(
          name: "gio-hang",
          url: "gio-hang",
          defaults: new { controller = "Cart", action = "Cart", id = UrlParameter.Optional }
      );

            routes.MapRoute(
           name: "dang-nhap-tai-khoan",
           url: "dang-nhap-tai-khoan",
           defaults: new { controller = "AccountKH", action = "Login", id = UrlParameter.Optional }
       );
            routes.MapRoute(
            name: "tim-kiem",
            url: "tim-kiem",
            defaults: new { controller = "HomeKH", action = "SearchProduct", id = UrlParameter.Optional }
        );

            routes.MapRoute(
             name: "trang-chu",
             url: "trang-chu",
             defaults: new { controller = "HomeKH", action = "Index", id = UrlParameter.Optional }
         );

            routes.MapRoute(
            name: "chi-tiet-san-pham",
            url: "chi-tiet-san-pham",
            defaults: new { controller = "ProductKH", action = "Details", id = UrlParameter.Optional }
        );





            //Quản trị hệ thống
            routes.MapRoute(
              name: "quan-tri-nguoi-dung",
              url: "quan-tri-nguoi-dung",
              defaults: new { controller = "CustomerManage", action = "ListCustomer", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "chi-tiet-san-pham-duyet",
              url: "chi-tiet-san-pham-duyet",
              defaults: new { controller = "ProductAd", action = "ProductDetails", id = UrlParameter.Optional }
          );

            routes.MapRoute(
               name: "san-pham-da-khoa",
               url: "san-pham-da-khoa",
               defaults: new { controller = "ProductAd", action = "ListProductDaKhoa", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "san-pham-da-duyet",
               url: "san-pham-da-duyet",
               defaults: new { controller = "ProductAd", action = "ListProductDaDuyet", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "san-pham-cho-duyet",
              url: "san-pham-cho-duyet",
              defaults: new { controller = "ProductAd", action = "ListProductChoDuyet", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "trang-quan-tri",
              url: "trang-quan-tri",
              defaults: new { controller = "HomeAd", action = "Index", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "admin",
              url: "admin",
              defaults: new { controller = "AccountAd", action = "Login", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "thuong-hieu",
              url: "thuong-hieu",
              defaults: new { controller = "ThuongHieu", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
               name: "gia-tri-tskt",
               url: "gia-tri-tskt",
               defaults: new { controller = "GiaTriTSKT", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "danh-muc-thong-so-ky-thuat",
                url: "danh-muc-thong-so-ky-thuat",
                defaults: new { controller = "DMThongSoKyThuat", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "danh-muc",
                url: "danh-muc",
                defaults: new { controller = "DanhMuc", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "quan-tri-gian-hang-nguoi-dung",
            url: "quan-tri-gian-hang-nguoi-dung",
            defaults: new { controller = "ManageShopAd", action = "ListGianHang", id = UrlParameter.Optional }
        );





            //Bán hàng

            routes.MapRoute(
             name: "danh-sach-don-hang-cua-hang",
             url: "danh-sach-don-hang-cua-hang",
             defaults: new { controller = "ManageOrder", action = "ListOrderByShop", id = UrlParameter.Optional }
         );

            routes.MapRoute(
             name: "danh-sach-cua-hang-co-don-hang",
             url: "danh-sach-cua-hang-co-don-hang",
             defaults: new { controller = "ManageOrder", action = "ListShopCanXuLy", id = UrlParameter.Optional }
         );

            routes.MapRoute(
           name: "danh-sach-cua-hang-co-san-pham",
           url: "danh-sach-cua-hang-co-san-pham",
           defaults: new { controller = "Product", action = "ListProductCanXL", id = UrlParameter.Optional }
       );



            routes.MapRoute(
              name: "cap-nhat-gian-hang",
              url: "cap-nhat-gian-hang",
              defaults: new { controller = "Shop", action = "Edit", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "quan-tri-gian-hang",
              url: "quan-tri-gian-hang",
              defaults: new { controller = "Shop", action = "ListGianHang", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "them-gian-hang",
              url: "them-gian-hang",
              defaults: new { controller = "Shop", action = "ThemGianHang", id = UrlParameter.Optional }
          );


            routes.MapRoute(
              name: "trang-ban-hang",
              url: "trang-ban-hang",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
          );

            routes.MapRoute(
             name: "sua-san-pham",
             url: "sua-san-pham",
             defaults: new { controller = "Product", action = "EditProduct", id = UrlParameter.Optional }
         );
            routes.MapRoute(
              name: "danh-sach-san-pham",
              url: "danh-sach-san-pham",
              defaults: new { controller = "Product", action = "ListProduct", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "dang-nhap-ban-hang",
              url: "dang-nhap-ban-hang",
              defaults: new { controller = "AccountBH", action = "Login", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "them-san-pham-new",
              url: "them-san-pham-new",
              defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "them-san-pham",
              url: "them-san-pham",
              defaults: new { controller = "Product", action = "Before", id = UrlParameter.Optional }
          );
            



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "HomeKH", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
