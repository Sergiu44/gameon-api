using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly wbookdbContext Context;

        public BaseRepository(wbookdbContext context)
        {
            Context = context;
        }

        public IQueryable<TEntity> Get()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        public TEntity Insert(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            return entity;
        }

        public TEntity Update(TEntity entitty)
        {
            Context.Set<TEntity>().Update(entitty);

            return entitty;
        }

        public void Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }
    }
}
