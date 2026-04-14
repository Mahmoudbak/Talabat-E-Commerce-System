using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entity;
using Talabat.Core.Entity.Order_Aggreate;
using Talabat.Core.Entity.Product;
using Talabat.Core.Repository.content;
using Talabat.Repsotiory.Data;

namespace Talabat.Repsotiory
{
    public class UnitOfWork : IUnitOfWork
    {
       // private Dictionary<string, GenericRepository<BaseEntity>> _repositories; //سوا دي او التحت(Hashtable) احنا عملنها علشان لو مثلا product هيعمل اوبجكت جديد وانا مش عايز كدا
        private readonly StoreContext _dbContext;
        private Hashtable _repositories;
        //public IGenaricrepository<Product> productsRepo{get; set;}
        //public IGenaricrepository<ProductBrand> BrandsRepo{get; set;}
        //public IGenaricrepository<ProductCategory> categoriesRepo{get; set;}
        //public IGenaricrepository<DeliveryMethod> deliveryMethodsRepo{get; set;}
        //public IGenaricrepository<OrderItem> ordersItemsRepo{get; set;}
        //public IGenaricrepository<Order> ordersRepo{get; set;}

        public UnitOfWork(StoreContext dbContext)// ASK CLR for Creating Object from DBContext Implicitly
        {
            _dbContext = dbContext;

            _repositories = new Hashtable();

            //عملنا كدا علشان احنا مش محتاجين الكلام دا كلو او لما ننده عليها هيرجع كلو ودا غلط نشوف طريقة تانيه
            //productsRepo = new GenericRepository<Product>(_dbContext);
            //BrandsRepo=new GenericRepository<ProductBrand>(_dbContext);
            //categoriesRepo=new GenericRepository<ProductCategory>(_dbContext);
            //deliveryMethodsRepo=new GenericRepository<DeliveryMethod>(_dbContext);
            //ordersItemsRepo=new GenericRepository<OrderItem>(_dbContext);
            //ordersRepo = new GenericRepository<Order>(_dbContext);
        }
        public IGenaricrepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var Key=typeof(TEntity).Name;
            if (!_repositories.ContainsKey(Key))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);// as GenericRepository<BaseEntity>;

                _repositories.Add(Key, repository );
                
            } 
            return _repositories[Key] as IGenaricrepository<TEntity>;
        }   
        public async Task<int> CompleteAsync()
            =>await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();
    }
}
