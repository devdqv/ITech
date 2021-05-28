using ITech.Common;
using ITech.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ITech.Common
{
    public class Helper
    {
        public static List<SelectListItem> GetListNumber(int number)
        {
            var res = new List<SelectListItem>();
            for (int i = 1; i <= number; i++)
            {
                res.Add(new SelectListItem() { Text = "" + i, Value = "" + i });
            }
            return res;
            
        }
    }
}