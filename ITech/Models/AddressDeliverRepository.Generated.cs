using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;

//Đây là file gen tự động và sẽ bị chỉnh sửa khi cập nhật lại model
//do đó không sửa file này
	
namespace ITech.Models
{   
	public partial class AddressDeliverRepository
	{
		private IRepository<AddressDeliver> _repository {get;set;}
		public IRepository<AddressDeliver> Repository
		{
			get { return _repository; }
			set { _repository = value; }
		}
		
		public AddressDeliverRepository(IRepository<AddressDeliver> repository, IUnitOfWork unitOfWork)
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
		public IQueryable<AddressDeliver> All()
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
		public IQueryable<AddressDeliver> All(string includes)
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
		public IQueryable<AddressDeliver> All(List<string> lstInclude)
		{
			return Repository.All(lstInclude);
		}

		/// <summary>
        /// Lấy đối tượng AddressDeliver theo khóa chính
        /// </summary>        
        /// <returns>AddressDeliver</returns>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3/2020
        /// </modified>
      public AddressDeliver GetById(int Id)
		{
		    try
			{
		         ITechEntities context = (ITechEntities)Repository.UnitOfWork.Context;
                 return context.AddressDelivers.FirstOrDefault(dk => dk.ID == Id);			
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
		 public AddressDeliver SaveReturnToObject(AddressDeliver obj)
		{
		    try
			{
		         ITechEntities context = (ITechEntities)Repository.UnitOfWork.Context;

                 context.AddressDelivers.Add(obj);	

				 context.SaveChanges();		
			 }
			 
			catch(Exception ex) 
            {
                throw new Exception(ex.Message + ex.StackTrace + ex.Source);
			 }

			 return obj;
		}
		/// <summary>
        /// Thêm đối tượng AddressDeliver vào database
        /// </summary>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3/2020
        /// </modified>
		public void Add(AddressDeliver entity)
		{
			Repository.Add(entity);
		}
		/// <summary>
        /// Xóa đối tượng AddressDeliver
        /// </summary>
        /// <modified>
        /// Author              Date                comments
        /// VINHQD 10/3/2020
        /// </modified>
		public void Delete(AddressDeliver entity)
		{
			Repository.Delete(entity);
		}
		/// <summary>
        /// Thực hiện commit phiên làm việc để thay đổi dữ liệu trong database liên quan đến đối tượng AddressDeliver
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
        public void AddRange(IEnumerable<AddressDeliver> list)
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
        public void RemoveRange(IEnumerable<AddressDeliver> list)
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
        public AddressDeliver FindByPK(params object[] keyValues)
        {
            return this._repository.FindByPK(keyValues);
        }
	}
}