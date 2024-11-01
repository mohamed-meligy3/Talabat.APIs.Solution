using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvalutor<TEntity> where TEntity : BaseEntity
    {
        
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecification<TEntity> spec)
        {
            var query = inputQuery;//context.Orders

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);



            if(spec.OrderBy is not null)
                query =query.OrderBy(spec.OrderBy);//Asc
           
            if (spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);//Desc
           


            if(spec.IsPagenationEnabled)
            query = query.Skip(spec.Skip).Take(spec.Take);
           

            query = spec.Includes.Aggregate(query, (currentQuery, IncludeExpression) => currentQuery.Include(IncludeExpression));
           
            return query;
        }

    }
}
