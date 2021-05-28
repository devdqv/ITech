using System;
using System.Linq;
using System.Collections.Generic;
namespace ITech.Models
{   
	public partial class SpecValueRepository
	{
        // Thêm code ở đây
        // vì code ở file này sẽ không bị ghi đè
        public List<SpecValue> GetPaged(string SpecName, int RowPerPage, int CurrentPage, ref int iTotalrecodsTem, string Field)
        {
            if (string.IsNullOrEmpty(Field))
            {
                Field = "STT";
            }
            ITechEntities context = new ITechEntities();
            var Spec = context.Specifications.Where(x => x.Name.ToLower().Contains(SpecName.ToLower().Trim())).Select(x=>x.ID).ToArray();
            //Copy phần where bỏ "p=>"
            var query = from p in context.SpecValues
                        where p.ID > 0
                        && (SpecName.Trim() == "" || Spec.Contains(p.SpecID.Value))
                        orderby p.ID
                        select p;
            //Trả về kết quả
            return Pager.GetPaged(query, CurrentPage, RowPerPage, Field, "1", ref iTotalrecodsTem).ToList();
        }
    }
}