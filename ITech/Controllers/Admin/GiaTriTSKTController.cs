using ITech.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITech.Controllers.Admin
{
    public class GiaTriTSKTController : BaseAdminController
    {

        public List<Specification> lstLoaiTS { get; set; }
        public List<SpecValue> lstGiaTri { get; set; }
        public List<Category> lstDM { get; set; }


        public objMessage message;
        public SpecValue specValue;
        public SpecValue objSpecValue;

        public string DoAction { get; set; }
        public int RowPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string PathPage { get; set; }

        EFUnitOfWork unitOfWork = new EFUnitOfWork();
        SpecificationRepository specificationRepository;
        SpecValueRepository specValueRepository;

        public string ArrID { get; set; }
        public string ddlTSKT { get; set; }
        public string ValueChoose { get; set; }
        public string lstItemID { get; private set; }




        // [Route("{danh-muc-thong-so-ky-thuat}")]
        public ActionResult Index()
        {
            SpecificationRepository sRep = new SpecificationRepository(new EFRepository<Specification>(), unitOfWork);
            lstLoaiTS = sRep.All().Distinct().ToList();
            ViewBag.lstLoaiTS = lstLoaiTS;
            return View();
        }

        public ActionResult pList()
        {
            GetParameters();
            int iTotalrecods = 0;
            EFUnitOfWork unitOfWork = new EFUnitOfWork();
            SpecValueRepository objDM_SpecValue = new SpecValueRepository(new EFRepository<SpecValue>(), unitOfWork);
            lstGiaTri = objDM_SpecValue.GetPaged(ddlTSKT, RowPerPage, CurrentPage, ref iTotalrecods, "");
            //get html phân trang
            PathPage = string.Format("#ten={0}&RowPerPage={1}&Page=", ddlTSKT, RowPerPage);
            ViewBag.ltlPage = Pager.getPage(PathPage, CurrentPage, RowPerPage, iTotalrecods);
            return PartialView(lstGiaTri);
        }


        public ActionResult pForm()
        {
            specificationRepository = new SpecificationRepository(new EFRepository<Specification>(), unitOfWork);

            lstLoaiTS = specificationRepository.All().ToList();
            ViewBag.lstLoaiTS = lstLoaiTS;
            ViewBag.lstDM = new CategoryRepository(new EFRepository<Category>(), unitOfWork).All().Where(x => x.isDeleted == 1 && x.ParentCode != 0);
            getParams();
            return PartialView();
        }

        [HttpPost]
        public ActionResult GetListTSKT(string itemID)
        {
            List<int> ArrID = JsonConvert.DeserializeObject<List<int>>(itemID);
            ITechEntities db = new ITechEntities();
            List<Specification> lstTSKT = new List<Specification>();
            var lstSpecID = db.SpecValues.Select(x => x.SpecID);
            foreach(var t in ArrID)
            {
                lstTSKT.Add(new Specification() { ID = -1, CategoryID = t, Name = GetObj.GetCategory(t).Name }); //Tên Nhãn
                var listTSKTtmp = db.Specifications.Where(x => x.CategoryID == t && !lstSpecID.Contains(x.ID));
                lstTSKT.AddRange(listTSKTtmp);
                
            }
            
            return Json(lstTSKT.Select(x => new { x.Name, x.ID, x.CategoryID }), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetListTSKTEdit(string itemID)
        {
            List<int> ArrID = JsonConvert.DeserializeObject<List<int>>(itemID);
            ITechEntities db = new ITechEntities();
            List<Specification> lstTSKT = new List<Specification>();
            foreach (var t in ArrID)
            {
                lstTSKT.Add(new Specification() { ID = -1, CategoryID = t, Name = GetObj.GetCategory(t).Name }); //Tên Nhãn
                var listTSKTtmp = db.Specifications.Where(x => x.CategoryID == t);
                lstTSKT.AddRange(listTSKTtmp);

            }

            return Json(lstTSKT.Select(x => new { x.Name, x.ID, x.CategoryID }), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult getSpecificationsByCategory(int ID)
        {
            SpecificationRepository SpecificationRepository = new SpecificationRepository(new EFRepository<Specification>(), unitOfWork);
            var lstSP = SpecificationRepository.All().Where(x => x.CategoryID == ID).ToList();
            string html = "<option value=" + 0 + ">--Tất cả--</option>";
            var objSpecValue = Session["objSpecValue"] as SpecValue;
            foreach (var x in lstSP)
            {
                if (objSpecValue == null ? false : (objSpecValue.SpecID == x.ID ? true : false))
                {
                    html += "<option selected='selected' value=" + x.ID + ">" + x.Name + "</option>";
                }
                else
                {
                    html += "<option value=" + x.ID + ">" + x.Name + "</option>";
                }
            }
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult pAction()
        {
            message = new objMessage();
            specValue = new SpecValue();
            getParams();
            switch (DoAction)
            {
                case "add":
                    message = add();
                    break;
                case "delete":
                    message = delete();
                    break;
                case "edit":
                    message = update();
                    break;
                default:
                    break;

            }
            renderObjMessage(message);
            return View();
        }
        private objMessage update()
        {
            try
            {
                //getFromData(specValue);
                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    ArrID = Request["id"].ToString();
                }
                if (!string.IsNullOrEmpty(Request["ValueChoose"]))
                {
                    ValueChoose = Request["ValueChoose"].ToString();
                }
                specValueRepository = new SpecValueRepository(new EFRepository<SpecValue>(), unitOfWork);
                int arrID = int.Parse(ArrID);
                ValueChoose = ValueChoose.Substring(0, ValueChoose.Length - 1);
                SpecValue c = specValueRepository.All().SingleOrDefault(x => x.ID == arrID);
                //c.Name = specification.Name;
                c.Value = ValueChoose;

                specValueRepository.Save();
                message.Error = false;
                message.Message = "Cập nhật thành công";

            }
            catch (Exception e)
            {
                message.Error = true;
                message.Message = "Có lỗi trong quá trình cập nhật : " + e.Message;

            }
            return message;
        }

        private objMessage delete()
        {
            try
            {

                ITechEntities db = new ITechEntities();
                if (!string.IsNullOrEmpty(Request["id"]))
                {
                    ArrID = Request["id"].ToString();
                }
                int arrID = int.Parse(ArrID);
                var obj = db.SpecValues.SingleOrDefault(m => m.ID == arrID);
                db.SpecValues.Remove(obj);
                db.SaveChanges();
                message.Error = false;
                message.Message = "Xóa Thành Công";
            }
            catch (Exception e)
            {
                message.Error = true;
                message.Message = "Xóa không Thành Công: " + e.Message;
            }
            return message;
        }

        private objMessage add()
        {
            try
            {
                getFromData(specValue);
                ITechEntities db = new ITechEntities();
                ValueChoose = ValueChoose.Substring(0, ValueChoose.Length - 1);

                var arr = lstItemID.Split(';').ToList();
                arr.Remove("");
                foreach(var r in arr)
                {
                    var ar2 = r.Split('-');
                    db.SpecValues.Add(new SpecValue() { SpecID = int.Parse(ar2[0]), Value = ValueChoose, CategoryID = int.Parse(ar2[1]) });
                }
                

                db.SaveChanges();
                message.Message = "Thêm mới thành công";
                message.Error = false;


            }
            catch (Exception ex)
            {
                message.Message = "Có lỗi xảy ra : " + ex.Message;
                message.Error = true;
            }
            return message;

        }


        private void getFromData(SpecValue spec)
        {

            if (!string.IsNullOrEmpty(Request["id"]))
            {
                ArrID = Request["id"].ToString();
            }

            if (!string.IsNullOrEmpty(Request["ValueChoose"]))
            {
                ValueChoose = Request["ValueChoose"].ToString();
            }
            if (!string.IsNullOrEmpty(Request["ddlTSKT"]))
            {
                spec.SpecID = int.Parse(Request["ddlTSKT"].ToString());
            }
            lstItemID = Request["itemID"].ToString();


        }

        private void GetParameters()
        {
            PathPage = "#";

            if (!string.IsNullOrEmpty(Request["page"]))
            {
                CurrentPage = Convert.ToInt32(Request["page"]);
            }
            else
                CurrentPage = 1;
            if (!string.IsNullOrEmpty(Request["RowPerPage"]))
            {
                RowPerPage = Convert.ToInt32(Request["RowPerPage"]);
            }
            RowPerPage = (RowPerPage <= 0 || RowPerPage > 200) ? 15 : RowPerPage;

           
                ddlTSKT = Request["ddlTSKTSearch"];
        }

        private void getParams()
        {
            if (!string.IsNullOrEmpty(Request["id"]))
            {
                specValueRepository = new SpecValueRepository(new EFRepository<SpecValue>(), unitOfWork);

                specificationRepository = new SpecificationRepository(new EFRepository<Specification>(), unitOfWork);
                DoAction = Request["do"].ToString();
                ViewBag.ArrID = Request["id"];
                ViewBag.DoAction = DoAction;
                ViewBag.strSubmit = "Cập nhật";
                int arrID = int.Parse(Request["id"]);
                objSpecValue = specValueRepository.All().SingleOrDefault(x => x.ID == arrID);
                var lst = new List<String>();
                lst = objSpecValue.Value.Split(';').ToList();

                ViewBag.lstValue = lst;   //list string
                ViewBag.obj = objSpecValue; //obj SpecValue
                Session["objSpecValue"] = objSpecValue;

            }
            else
            {
                ViewBag.DoAction = "add";
                DoAction = "add";
                ViewBag.strSubmit = "Lưu lại";
            }
        }



    }
}