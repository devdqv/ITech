using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ITech.Models
{
    public static class Pager
    {
        public static string getPage(string strPathPage, int intCurrentPage, int intRowPerPage, int intTotalRecord)
        {
            List<int> ltsRowPerpage = new List<int>() { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 75, 100, 150 };
            if (!ltsRowPerpage.Contains(intRowPerPage))
                ltsRowPerpage.Add(intRowPerPage);
            ltsRowPerpage.Sort();

            int intTotalPage = (intTotalRecord % intRowPerPage == 0) ? intTotalRecord / intRowPerPage : ((intTotalRecord - (intTotalRecord % intRowPerPage)) / intRowPerPage) + 1; ;
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("<div class=\"bottom-pager row\">\r\n");
            strBuilder.Append("    <div class=\"left col-8\">\r\n");
            if (intCurrentPage > 1)
            {
                strBuilder.AppendFormat("        <a id=\"btnfirst\" href =\"{0}1\"  onclick=\"firstPageClick()\" class=\"first\" title=\"Trang đầu\"><button class=\"btn btn-danger\"><i class=\"fa fa-fast-backward\"></i></button></a>\r\n", strPathPage);
                strBuilder.AppendFormat("        <a  id=\"btnpre\" href =\"{0}{1}\" onclick=\"prevPageClick()\" class=\"pre\" title=\"Trang trước\"><button class=\"btn btn-danger\"><i class=\"fa fa-step-backward\"></i></button></a>\r\n", strPathPage, intCurrentPage - 1);
            }
            else
            {
                strBuilder.Append("        <a id=\"btnfirst\" href=\"javascript:;\" class=\"first-disable\" title=\"Trang đầu\"><button class=\"btn\"><i class=\"fa fa-fast-backward\"></i></button></a>\r\n");
                strBuilder.Append("        <a id=\"btnpre\" href=\"javascript:;\" class=\"pre-disable\" title=\"Trang trước\"><button class=\"btn\"><i class=\"fa fa-step-backward\"></i></button></a>\r\n");
            }
            strBuilder.Append("        <span>Trang</span>\r\n");
            strBuilder.AppendFormat("        <input type=\"button\" class=\"btn btn-light\" name=\"page\" value=\"{0}\" />\r\n", intCurrentPage);
            strBuilder.AppendFormat("        <input type=\"hidden\"  id=\"txtCurrentPage\" type =\"text\" name=\"page\" value=\"{0}\" />", intCurrentPage);
            strBuilder.AppendFormat("        <input  id=\"lblLastPage\" type =\"hidden\" value=\"{0}\" />\r\n", intTotalPage);
            strBuilder.AppendFormat("        /<button disabled class=\"btn btn-ligth\">{0}</button>\r\n", intTotalPage);

            if (intCurrentPage < intTotalPage)
            {
                strBuilder.AppendFormat("        <a href=\"{0}{1}\" onclick=\"nextPageClick()\" class=\"next\" title=\"Trang tiếp\"><button class=\"btn btn-danger\"><i class=\"fa fa-step-forward\"></i></button></a>\r\n", strPathPage, intCurrentPage + 1);
                strBuilder.AppendFormat("        <a href=\"{0}{1}\" onclick=\"lastPageClick()\" class=\"last\" title=\"Trang cuối\"><button class=\"btn btn-danger\"><i class=\"fa fa-forward\"></i></button></a>\r\n", strPathPage, intTotalPage);
            }
            else
            {
                strBuilder.Append("        <a href=\"javascript:;\" class=\"next-disable\" title=\"Trang tiếp\"><button class=\"btn\"><i class=\"fa fa-step-forward\"></i></button></a>\r\n");
                strBuilder.Append("        <a href=\"javascript:;\" class=\"last-disable\" title=\"Trang cuối\"><button class=\"btn\"><i class=\"fa fa-forward\"></i></button></a>\r\n");
            }
            strBuilder.Append("    </div>\r\n");
            strBuilder.Append("    <div class=\"right col-4\">\r\n");
            strBuilder.Append("       <div class=\"row\"><div class=\"col-6 btn\"> <span>Kết quả trên 1 trang:</span></div>\r\n");
            strBuilder.Append("       <div class=\"col-3\"> <select id=\"sltRowPerPage\" class=\"form-control\" onchange=\"onRowPerPageChange()\"  name =\"RowPerPage\">\r\n");
            foreach (var item in ltsRowPerpage)
            {
                strBuilder.AppendFormat("            <option value=\"{0}\"{1}>{2}</option>\r\n", item, (item == intRowPerPage) ? " selected" : "", item);
            }
            strBuilder.Append("        </select></div>\r\n");
            strBuilder.AppendFormat("       <div class=\"col-3 btn\"> <span>/ Tổng số: {0}</span></div>\r\n", intTotalRecord);
            strBuilder.Append("    </div>\r\n"); //row 
            strBuilder.Append("    </div>\r\n");
            strBuilder.Append("</div>\r\n");
            return strBuilder.ToString();
        }

        public static IQueryable<T> GetPaged<T>(this IQueryable<T> query,
                               int pageNum, int pageSize,
                              string strColumnName,
                               string strOrder, ref int rowsCount)
        {
            
            if (pageSize <= 0) pageSize = 20;

            rowsCount = query.Count();

            if (rowsCount <= pageSize || pageNum <= 0) pageNum = 1;

            int excludedRows = (pageNum - 1) * pageSize;

            return query.Skip(excludedRows).Take(pageSize);
        }
    }
}