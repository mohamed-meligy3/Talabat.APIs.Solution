using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Repositories;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext context;

        private Hashtable reposatories;

        public UnitOfWork(StoreContext context)
        {
            this.context = context;
            reposatories = new Hashtable(); 
        }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
           var type  = typeof(TEntity).Name;//Product
            if(!reposatories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity>(context);

                reposatories.Add(type, repository);
            }
            return reposatories[type] as IGenericRepository<TEntity>;
        }

        public async Task<int> Complete()
         => await context.SaveChangesAsync();

        public async ValueTask DisposeAsync()
          => await context.DisposeAsync();

      
    }
}
