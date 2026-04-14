using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity;
using Talabat.Core.specifications;

namespace Talabat.Core.Repository.content
{
    public interface IGenaricrepository<T>where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int Id);
        Task<IReadOnlyList<T>> GetAllAsync();
        //Task<IEnumerable<T>> GetAllAsync();//Iam you IReadOnlyList because Iam not operation in controller
        Task<T?> GetByIdwithSpecAsync(Ispecifications<T> spec );


        //Task<IEnumerable<T>> GetAllwithSpecAsync(Ispecifications<T>spec) ;
        Task<IReadOnlyList<T>> GetAllwithSpecAsync(Ispecifications<T> spec);
        Task<int> GetCountAsync(Ispecifications<T> spec);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);  


    }
}
 