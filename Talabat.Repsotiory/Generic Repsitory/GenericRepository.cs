using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity;
using Talabat.Core.Repository.content;
using Talabat.Core.specifications;
using Talabat.Repsotiory.Data;

namespace Talabat.Repsotiory
{
    public class GenericRepository<T> : IGenaricrepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;
        public GenericRepository(StoreContext dbcontext)//ASk CLR For creating object from DBcontext
        {
            _dbContext = dbcontext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if (typeof(T)==typeof(Product))
            //    return (IEnumerable<T>) await _dbContext.Set<Product>().Include(p=>p.productBrand).Skip(20).Take(10).Order(p=>p.Name).Include(p=>p.productCategory).ToListAsync();
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllwithSpecAsync(Ispecifications<T> spec)
        {
             return await applySpecifiaction(spec).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int Id)
        {
            //if (typeof(T) == typeof(Product))
            //    return  await _dbContext.Set<Product>().Where(p => p.Id == Id).Include(p => p.productBrand).Include(p => p.productCategory).FirstOrDefaultAsync() as T;
            return await _dbContext.Set<T>().FindAsync(Id);

        }

        public async Task<T?> GetByIdwithSpecAsync(Ispecifications<T> spec)
        {
            return await applySpecifiaction(spec).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountAsync(Ispecifications<T> spec)
        {
             return await applySpecifiaction(spec).CountAsync();  
        }

        private IQueryable<T> applySpecifiaction(Ispecifications<T> spec)
        {

            return SpecificationEvaluation<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public async Task AddAsync(T entity)
        =>await _dbContext.Set<T>().AddAsync(entity);
       
        public void Update(T entity)
        =>_dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
        =>_dbContext.Set<T>().Remove(entity);
    }
}
