using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITech.Models
{
    public class OrderModelPOCO
    {
        public Order order { get; set; }
        public List<OrderDetail> orderDetails { get; set; }
    }
}