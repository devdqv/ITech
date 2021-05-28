using System;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace ITech.Models
{
	public class EFUnitOfWork : IUnitOfWork
	{
		public DbContext Context { get; set; }

		public EFUnitOfWork()
		{
			Context = new ITechEntities();
			
		}

		public void Save()
		{
			try
			{
				Context.SaveChanges();
			}
			catch (DbEntityValidationException e)
			{
				string strMsg = string.Empty;
				foreach (var eve in e.EntityValidationErrors)
				{
					 strMsg += string.Format("\nEntity of type \"{0}\" in state \"{1}\" has the following validation errors:",
						eve.Entry.Entity.GetType().Name, eve.Entry.State);
					foreach (var ve in eve.ValidationErrors)
					{
						strMsg += string.Format( "- Property: \"{0}\", Error: \"{1}\"",
							ve.PropertyName, ve.ErrorMessage);
					}
				}
                throw new Exception(strMsg);
			}	
			
		}
		
		public bool LazyLoadingEnabled
		{
			get { return Context.Configuration.LazyLoadingEnabled; }
			set { Context.Configuration.LazyLoadingEnabled = value;}
		}

		public bool ProxyCreationEnabled
		{
			get { return Context.Configuration.ProxyCreationEnabled; }
			set { Context.Configuration.ProxyCreationEnabled = value; }
		}
		
		public string ConnectionString
		{
			get { return Context.Database.Connection.ConnectionString; }
			set { Context.Database.Connection.ConnectionString = value; }
		}
	}
}