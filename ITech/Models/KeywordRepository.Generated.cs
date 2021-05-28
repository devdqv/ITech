using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;

//Đây là file gen tự động và sẽ bị chỉnh sửa khi cập nhật lại model
//do đó không sửa file này
	
namespace ITech.Models
{   
	public partial class KeywordRepository
	{
		private IRepository<Keyword> _repository {get;set;}
		public IRepository<Keyword> Repository
		{
			get { return _repository; }
			set { _repository = value; }
		}
		
		public KeywordRepository(IRepository<Keyword> repository, IUnitOfWork unitOfWork)
    	{
    		Repository = repository;
			Repository.UnitOfWork = unitOfWork; 
    	}
		/// <summary>
        /// Lấy tất cả dữ liệu của bảng
        /// </summary>        
        /// <returns>Query đã lấy dữ liệu toàn bộ bảng</returns>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3/2020
        /// </modified>
		public IQueryable<Keyword> All()
		{
			return Repository.All();
		}

		/// <summary>
        /// Include để gọi dữ liệu các bảng có quan hệ
        /// </summary>
        /// <param name="includes">Danh sách chứa tên các bảng quan hệ cần lấy, cách nhau bằng dấu ","</param>
        /// <returns>Query đã lấy những bảng quan hệ</returns>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3
        /// </modified>
		public IQueryable<Keyword> All(string includes)
		{
			return Repository.All(includes);
		}
		
		/// <summary>
        /// Include để gọi dữ liệu các bảng có quan hệ
        /// </summary>
        /// <param name="lstInclude">Danh sach chua ten cac bang quan he can lay</param>
        /// <returns>Query đã lấy những bảng quan hệ</returns>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3
        /// </modified>
		public IQueryable<Keyword> All(List<string> lstInclude)
		{
			return Repository.All(lstInclude);
		}

		/// <summary>
        /// Lấy đối tượng Keyword theo khóa chính
        /// </summary>        
        /// <returns>Keyword</returns>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3/2020
        /// </modified>
      public Keyword GetById(int Id)
		{
		    try
			{
		         ITechEntities context = (ITechEntities)Repository.UnitOfWork.Context;
                 return context.Keywords.FirstOrDefault(dk => dk.ID == Id);			
			 }
			 catch(Exception ex) 
            {
                throw new Exception(ex.Message + ex.StackTrace + ex.Source);
			 }

		}

		/// <summary>
        /// Hàm thêm đối tượng vào database rồi trả về cả đối tượng (chỉ hữu ích khi dùng lazy load)
        /// </summary>        
        /// <returns>Đối tượng đã thêm vào database</returns>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3/2020
        /// </modified>
		 public Keyword SaveReturnToObject(Keyword obj)
		{
		    try
			{
		         ITechEntities context = (ITechEntities)Repository.UnitOfWork.Context;

                 context.Keywords.Add(obj);	

				 context.SaveChanges();		
			 }
			 
			catch(Exception ex) 
            {
                throw new Exception(ex.Message + ex.StackTrace + ex.Source);
			 }

			 return obj;
		}
		/// <summary>
        /// Thêm đối tượng Keyword vào database
        /// </summary>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3/2020
        /// </modified>
		public void Add(Keyword entity)
		{
			Repository.Add(entity);
		}
		/// <summary>
        /// Xóa đối tượng Keyword
        /// </summary>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3/2020
        /// </modified>
		public void Delete(Keyword entity)
		{
			Repository.Delete(entity);
		}
		/// <summary>
        /// Thực hiện commit phiên làm việc để thay đổi dữ liệu trong database liên quan đến đối tượng Keyword
        /// </summary>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3/2020
        /// </modified>
		public void Save()
		{
			Repository.Save();
		}
		/// <summary>
        /// Thêm một list đối tượng vào cơ sở dữ liệu
        /// </summary>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3/2020
        /// </modified>
        public void AddRange(IEnumerable<Keyword> list)
        {
            this._repository.AddRange(list);
        }
		/// <summary>
        /// Xóa một list đối tượng trong cơ sở dữ liệu
        /// </summary>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3/2020
        /// </modified>
        public void RemoveRange(IEnumerable<Keyword> list)
        {
            this._repository.RemoveRange(list);
        }
		/// <summary>
        /// Lấy đối tượng có nhiều khóa chính
        /// </summary>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3/2020
        /// </modified>
        public Keyword FindByPK(params object[] keyValues)
        {
            return this._repository.FindByPK(keyValues);
        }
	}
}