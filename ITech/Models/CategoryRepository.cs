using System;
using System.Linq;
using System.Collections.Generic;
namespace ITech.Models
{
    public partial class CategoryRepository
    {
        // Thêm code ở đây
        // vì code ở file này sẽ không bị ghi đè
        public List<Category> GetPaged(string TenDM, int RowPerPage, int CurrentPage, ref int iTotalrecodsTem, string Field)
        {
            if (string.IsNullOrEmpty(Field))
            {
                Field = "STT";
            }
            ITechEntities context = new ITechEntities();
            //Copy phần where bỏ "p=>"
            var query = from p in context.Categories
                        where p.ID > 0 && p.isDeleted==1
                        && (string.IsNullOrEmpty(TenDM) || p.Name.ToLower().Contains(TenDM.ToLower()))
                        orderby p.STT
                        select p;
            //Trả về kết quả
            return Pager.GetPaged(query, CurrentPage, RowPerPage, Field, "1", ref iTotalrecodsTem).ToList();
        }

    }
}