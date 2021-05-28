using ITech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITech
{
    public class GetObj
    {
        

        public static Category GetCategory(int id)
        {
            EFUnitOfWork unitOfWork = new EFUnitOfWork();
            CategoryRepository categoryRepository = new CategoryRepository(new EFRepository<Category>(), unitOfWork);
            return categoryRepository.GetById(id);
        }
        public static Specification GetSpecification(int id)
        {
            EFUnitOfWork unitOfWork = new EFUnitOfWork();
            SpecificationRepository categoryRepository = new SpecificationRepository(new EFRepository<Specification>(), unitOfWork);
            return categoryRepository.GetById(id);
        }

        public static Shop GetSaleman(string id)
        {
            EFUnitOfWork unitOfWork = new EFUnitOfWork();
            ShopRepository SalemanRepository = new ShopRepository(new EFRepository<Shop>(), unitOfWork);
            return SalemanRepository.GetById(id);
        }

        public static Product GetProduct(int id)
        {
            EFUnitOfWork unitOfWork = new EFUnitOfWork();
            ProductRepository ProductRepository = new ProductRepository(new EFRepository<Product>(), unitOfWork);
            return ProductRepository.GetById(id);
        }

        public static AddressDeliver GetAddressDeliver(int id)
        {
            EFUnitOfWork unitOfWork = new EFUnitOfWork();
            AddressDeliverRepository addressDeliverRepository = new AddressDeliverRepository(new EFRepository<AddressDeliver>(), unitOfWork);
            return addressDeliverRepository.GetById(id);
        }

    }
}