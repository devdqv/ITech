using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITech.Models
{
    public class OrderXuLyModel
    {
        public Shop shop { get; set; }
        public List<Order> orders { get; set; }
        public List<OrderDetail> orderDetails { get; set; }
    }
}