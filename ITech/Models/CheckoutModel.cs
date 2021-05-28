using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITech.Models
{
    public class CheckoutModel
    {
        public string FeeShip { get; set; }

        public string ShopID { get; set; }

        public string AddressDelivery { get; set; }

        public List<MyCart> OrderItem { get; set; }
        

    }
}