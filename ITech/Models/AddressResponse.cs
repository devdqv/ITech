using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITech.Models
{
    public class AddressResponse
    {
        public int ID { get; set; }
        public string Cus_name { get; set; }
        public string Cus_phone { get; set; }
        public string ApartmentNumber { get; set; }
        public Nullable<int> Default { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string UserID { get; set; }
        public string WardCode { get; set; }
        public string DistrictCode { get; set; }
        public string CityCode { get; set; }
    }
}