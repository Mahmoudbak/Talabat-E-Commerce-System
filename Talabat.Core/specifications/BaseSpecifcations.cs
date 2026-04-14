using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entity;

namespace Talabat.Core.specifications;

public class BaseSpecifcations<T> : Ispecifications<T> where T : BaseEntity
{
    public Expression<Func<T, bool>>? criteria { get; set; }=null;
    public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
    public Expression<Func<T, object>> OrderBy { get; set; } = null;
    public Expression<Func<T, object>> OrderByDesc { get; set; } = null;
    public int Skip { get  ;set; }
    public int Take { get  ;set; }
    public bool IsPaginationEnabled { get; set; } = false;

    public BaseSpecifcations()
    {
        
    }
    public BaseSpecifcations(Expression<Func<T, bool>>criteriaExpression)
    {
        criteria = criteriaExpression; //(p=>p.Id==id=10)
        
    }
    public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    { 
        OrderBy= orderByExpression;
    }
    public void AddOrderByDesc(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDesc = orderByDescExpression;
    }
    public void ApplyPagination(int skip, int take) 
    {
        IsPaginationEnabled=true;
        Skip= skip;
        Take= take; 
    }

}


