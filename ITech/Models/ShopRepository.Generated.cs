using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;

//Đây là file gen tự động và sẽ bị chỉnh sửa khi cập nhật lại model
//do đó không sửa file này
	
namespace ITech.Models
{   
	public partial class ShopRepository
	{
		private IRepository<Shop> _repository {get;set;}
		public IRepository<Shop> Repository
		{
			get { return _repository; }
			set { _repository = value; }
		}
		
		public ShopRepository(IRepository<Shop> repository, IUnitOfWork unitOfWork)
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
		public IQueryable<Shop> All()
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
		public IQueryable<Shop> All(string includes)
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
		public IQueryable<Shop> All(List<string> lstInclude)
		{
			return Repository.All(lstInclude);
		}

		/// <summary>
        /// Lấy đối tượng Shop theo khóa chính
        /// </summary>        
        /// <returns>Shop</returns>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3/2020
        /// </modified>
      public Shop GetById(string Id)
		{
		    try
			{
		         ITechEntities context = (ITechEntities)Repository.UnitOfWork.Context;
                 return context.Shops.FirstOrDefault(dk => dk.ID == Id);			
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
		 public Shop SaveReturnToObject(Shop obj)
		{
		    try
			{
		         ITechEntities context = (ITechEntities)Repository.UnitOfWork.Context;

                 context.Shops.Add(obj);	

				 context.SaveChanges();		
			 }
			 
			catch(Exception ex) 
            {
                throw new Exception(ex.Message + ex.StackTrace + ex.Source);
			 }

			 return obj;
		}
		/// <summary>
        /// Thêm đối tượng Shop vào database
        /// </summary>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3/2020
        /// </modified>
		public void Add(Shop entity)
		{
			Repository.Add(entity);
		}
		/// <summary>
        /// Xóa đối tượng Shop
        /// </summary>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3/2020
        /// </modified>
		public void Delete(Shop entity)
		{
			Repository.Delete(entity);
		}
		/// <summary>
        /// Thực hiện commit phiên làm việc để thay đổi dữ liệu trong database liên quan đến đối tượng Shop
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
        public void AddRange(IEnumerable<Shop> list)
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
        public void RemoveRange(IEnumerable<Shop> list)
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
        public Shop FindByPK(params object[] keyValues)
        {
            return this._repository.FindByPK(keyValues);
        }
	}
}