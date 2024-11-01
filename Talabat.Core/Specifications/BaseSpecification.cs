using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get ; set ; }//WHERE
        public List<Expression<Func<T, object>>> Includes { get; set ; } = new List<Expression<Func<T, object>>>();//Inialize 
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get  ; set ; }
        public int Take { get ; set ; }
        public int Skip { get; set ; }
        public bool IsPagenationEnabled { get ; set; }

        public BaseSpecification()
        {
            
        }

        public BaseSpecification(Expression<Func<T, bool>> Criteria)//P=>P.ID = id;
        {
            this.Criteria = Criteria;   
          
        }
        public void AddOrderBy(Expression<Func<T, object>> OrderBy)
        {
            this.OrderBy = OrderBy;
        }

        public void AddOrderByDesc(Expression<Func<T, object>> OrderByDesc)
        {
            this.OrderByDesc = OrderByDesc;
        }

        public void ApplyPagenation(int take,int skip)
        {
            IsPagenationEnabled = true;
            Take = take;
            Skip = skip;
        }
     
    }
}
