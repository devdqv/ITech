
using ITech.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITech.Controllers.Admin
{

    public class DMThongSoKyThuatController : BaseAdminController
    {

        public List<Specification> lstLoaiTS { get; set; }
        public List<Category> lstDM { get; set; }


        public objMessage message;
        public Specification specification;

        public string DoAction { get; set; }
        public int RowPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string PathPage { get; set; }

        EFUnitOfWork unitOfWork = new EFUnitOfWork();
        SpecificationRepository specificationRepository;

        public string ArrID { get; set; }
        public string TenTS { get; set; }
        public string lstCategoryID { get; private set; }




        // [Route("{danh-muc-thong-so-ky-thuat}")]
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult pList()
        {
            GetParameters();
            int iTotalrecods = 0;
            EFUnitOfWork unitOfWork = new EFUnitOfWork();
            SpecificationRepository objDM_Category = new SpecificationRepository(new EFRepository<Specification>(), unitOfWork);
            lstLoaiTS = objDM_Category.GetPaged(TenTS, RowPerPage, CurrentPage, ref iTotalrecods, "");
            //get html phân trang
            PathPage = string.Format("#ten={0}&RowPerPage={1}&Page=", TenTS, RowPerPage);
            ViewBag.ltlPage = Pager.getPage(PathPage, CurrentPage, RowPerPage, iTotalrecods);
            return PartialView(lstLoaiTS);
        }


        public ActionResult pForm()
        {
            specificationRepository = new SpecificationRepository(new EFRepository<Specification>(), unitOfWork);
            CategoryRepository categoryRepository = new CategoryRepository(new EFRepository<Category>(), unitOfWork);
            lstDM = categoryRepository.All().Where(x => x.isDeleted == 1 && x.ParentCode != 0).ToList();
            ViewBag.lstDM = lstDM;
            getParams();
            return PartialView();
        }

        [HttpPost]
        public ActionResult pAction()
        {
            unitOfWork = new EFUnitOfWork();
            message = new objMessage();
            specification = new Specification();
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
                getFromData(specification);
                specificationRepository = new SpecificationRepository(new EFRepository<Specification>(), unitOfWork);
                int arrID = int.Parse(ArrID);
                Specification c = specificationRepository.All().SingleOrDefault(x => x.ID == arrID);
                c.Name = specification.Name;

                specificationRepository.Save();
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
                getFromData(specification);
                int arrID = int.Parse(ArrID);
                var obj = db.Specifications.SingleOrDefault(m => m.ID == arrID);
                var objSpecValue = db.SpecValues.SingleOrDefault(m => m.SpecID == arrID);
                db.Specifications.Remove(obj);
                if (objSpecValue != null)
                    db.SpecValues.Remove(objSpecValue);
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
                getFromData(specification);
                ITechEntities db = new ITechEntities();
                var ArrID = lstCategoryID.Split(';').ToList();
                ArrID.Remove("");
                foreach (string x in ArrID)
                {
                    var tmp = new Specification()
                    {
                        Name = specification.Name,
                        CategoryID = int.Parse(x)
                    };
                    db.Specifications.Add(tmp);
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


        private void getFromData(Specification spec)
        {

            if (!string.IsNullOrEmpty(Request["id"]))
            {
                ArrID = Request["id"].ToString();
            }

            if (!string.IsNullOrEmpty(Request["txtTS"]))
            {
                spec.Name = Request["txtTS"].ToString();
            }
            if (!string.IsNullOrEmpty(Request["idDanhMucArr"]))
            {
                lstCategoryID = Request["idDanhMucArr"].ToString();
            }


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

            if (!string.IsNullOrEmpty(Request["txtTenTSSearch"]))
                TenTS = Request["txtTenTSSearch"];
        }

        private void getParams()
        {
            if (!string.IsNullOrEmpty(Request["id"]))
            {

                specificationRepository = new SpecificationRepository(new EFRepository<Specification>(), unitOfWork);
                DoAction = Request["do"].ToString();
                ViewBag.ArrID = Request["id"];
                ViewBag.DoAction = DoAction;
                ViewBag.strSubmit = "Cập nhật";
                int arrID = int.Parse(Request["id"]);
                ViewBag.obj = specificationRepository.All().SingleOrDefault(x => x.ID == arrID);

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