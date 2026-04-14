using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity;
using Talabat.Core.specifications;

namespace Talabat.Repsotiory
{
    internal class SpecificationEvaluation<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity>InputQuery,Ispecifications<TEntity>spec)
        {
            var query = InputQuery; //_dbcontext.Products

            if (spec.criteria is not null)
                query = query.Where(spec.criteria);
            //query=_dbcontext.Products.Where(p=>p.Id==1)
            //Includes
            //1.p=>p.Brand
            //2.p=>p.category


            if(spec.OrderBy is not null)//p=>p.Name
                query = query.OrderBy(spec.OrderBy);
            else  if(spec.OrderByDesc is not null)//p=>p.Price 
            {
                query=query.OrderByDescending(spec.OrderByDesc);
            }



            if(spec.IsPaginationEnabled)
                query=query.Skip(spec.Skip).Take(spec.Take);





            query = spec.Includes.Aggregate(query, (currentQuery, IncludeExpressions) => currentQuery.Include(IncludeExpressions));
            //_dbcontext.Products.Where(p=>p.Id==1).Include(p=>p.Brand)
            //query=_dbcontext.Products.Where(p=>true&&true).orderby(p=>p.Name).Skip(5).Take(5).Include(p=>p.Brand)

            return query;
        }
    }
}
