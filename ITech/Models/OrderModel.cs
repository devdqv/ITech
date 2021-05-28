using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITech.Models
{
    public class OrderModel
    {
        public sp_ListOrderByShop_Result order { get; set; }

        public List<sp_ListOrderItemByOrder_Result> orderItems { get; set; }
    }
}