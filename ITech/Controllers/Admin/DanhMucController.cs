using ITech.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ITech.Controllers.Admin
{

    public class DanhMucController : BaseAdminController
    {

        public List<Category> lstDanhMuc { get; set; }
        public List<Category> lstDMCha { get; set; }
        public objMessage message;
        public Category category;
        
        public string DoAction { get; set; }
        public int RowPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string PathPage { get; set; }

        EFUnitOfWork unitOfWork = new EFUnitOfWork();
        CategoryRepository categoryRepository;
        public string file = "";
        public int STTMax { get; set; }
        public string ArrID { get; set; }
        public string TenDM { get;  set; }
        

       // [Route("{danh-muc}")] // GET: DanhMuc
        public ActionResult Index()
        {
             
            return View();
        }

        public ActionResult pList()
        {
            GetParameters();
            int iTotalrecods = 0;
            EFUnitOfWork unitOfWork = new EFUnitOfWork();
            CategoryRepository objDM_Category = new CategoryRepository(new EFRepository<Category>(), unitOfWork);
            lstDanhMuc = objDM_Category.GetPaged(TenDM, RowPerPage, CurrentPage, ref iTotalrecods, "");
            //get html phân trang
            PathPage = string.Format("#ten={0}&RowPerPage={1}&Page=", TenDM, RowPerPage);
            ViewBag.ltlPage = Pager.getPage(PathPage, CurrentPage, RowPerPage, iTotalrecods);
            return PartialView(lstDanhMuc);
        }
        

        public ActionResult pForm()
        {
            categoryRepository = new CategoryRepository(new EFRepository<Category>(), unitOfWork);
            lstDMCha = categoryRepository.All().Where(x => x.isDeleted == 1 && x.ParentCode == 0).ToList();
            ViewBag.lstDMCha = lstDMCha;
            ViewBag.STTMax = categoryRepository.All().Max(x => x.STT) + 1==null?1: categoryRepository.All().Max(x => x.STT)+1;
            getParams();
            return PartialView();
        }

        [HttpPost]
        public ActionResult pAction()
        {
            message = new objMessage();
            category = new Category();
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
                getFromData(category);
                categoryRepository = new CategoryRepository(new EFRepository<Category>(), unitOfWork);
                int arrID = int.Parse(ArrID);
                Category c = categoryRepository.All().SingleOrDefault(x => x.ID == arrID);
                c.Name=category.Name;
                c.ParentCode=category.ParentCode;
                c.STT=category.STT;
                var filenames = UploadImageCommon();
                if (filenames != "-1" && filenames != "0") //khong co anh hoac error
                {
                    List<string> arr = filenames.Split(';').ToList();
                    c.Image = arr[0];
                }
                categoryRepository.Save();
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
                getFromData(category);
                int arrID = int.Parse(ArrID);
                var obj = db.Categories.SingleOrDefault(m => m.ID == arrID);
                obj.isDeleted = 0;
                db.SaveChanges();
                DeleteImage(obj.Image);
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
                getFromData(category);
                var filenames = UploadImageCommon();
                if (filenames != "-1" && filenames != "0")
                {
                    List<string> arr= filenames.Split(';').ToList();
                    category.Image = arr[0];
                    category.isDeleted = 1;
                    ITechEntities db = new ITechEntities();
                    db.Categories.Add(category);
                    db.SaveChanges();
                    message.Message = "Thêm mới thành công";
                    message.Error = false;
                }
                else
                {
                    message.Message = "Thêm mới không thành công";
                    message.Error = true;
                }

            }
            catch (Exception ex)
            {
                message.Message = "Có lỗi xảy ra : " + ex.Message;
                message.Error = true;
            }
            return message;

        }


        private void getFromData(Category category)
        {

            if (!string.IsNullOrEmpty(Request["id"]))
            {
                ArrID = Request["id"].ToString();
            }

            if (!string.IsNullOrEmpty(Request["txtDM"]))
            {
                category.Name = Request["txtDM"].ToString();
            }
            if (!string.IsNullOrEmpty(Request["ddlDMCha"]))
            {
                category.ParentCode = int.Parse(Request["ddlDMCha"]);
            }
            if (!string.IsNullOrEmpty(Request["txtSTT"]))
            {
                category.STT = int.Parse(Request["txtSTT"].ToString());
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
            
            if (!string.IsNullOrEmpty(Request["txtDMSearch"]))
                TenDM = Request["txtDMSearch"];
        }

        private void getParams()
        {
            if (!string.IsNullOrEmpty(Request["id"]))
            {

                categoryRepository = new CategoryRepository(new EFRepository<Category>(), unitOfWork);
                DoAction = Request["do"].ToString();
                ViewBag.DoAction = DoAction;
                ViewBag.strSubmit = "Cập nhật";
                int arrID = int.Parse(Request["id"]);
                ViewBag.ArrID = Request["id"];

                ViewBag.objDM = categoryRepository.All().SingleOrDefault(x => x.ID == arrID);

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