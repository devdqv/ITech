using System;
using System.Linq;
using System.Collections.Generic;
namespace ITech.Models
{   
	public partial class SpecificationRepository
	{
        // Thêm code ở đây
        // vì code ở file này sẽ không bị ghi đè

        public List<Specification> GetPaged(string TenTS, int RowPerPage, int CurrentPage, ref int iTotalrecodsTem, string Field)
        {
            if (string.IsNullOrEmpty(Field))
            {
                Field = "ID";
            }
            ITechEntities context = new ITechEntities();
            //Copy phần where bỏ "p=>"
            var query = from p in context.Specifications
                        where p.ID > 0
                        && (string.IsNullOrEmpty(TenTS) || p.Name.ToLower().Contains(TenTS.ToLower()))
                        orderby p.Name
                        select p;
            //Trả về kết quả
            return Pager.GetPaged(query, CurrentPage, RowPerPage, Field, "1", ref iTotalrecodsTem).ToList();
        }
    }
}