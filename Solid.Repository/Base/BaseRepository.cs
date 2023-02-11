using Microsoft.EntityFrameworkCore;
using Solid.Repository.Interfaces;

namespace Solid.Repository.Base
{
    public class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected DbContext Context;

        protected DbSet<T> Entities => Context.Set<T>();

        public BaseRepository(DbContext context)
        {
            this.Context = context;
        }

        public T Create(T entity)
        {
            Entities.Add(entity);
            return entity;
        }

        public async ValueTask<T> CreateAsync(T entity)
        {
            await Entities.AddAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Entities.Attach(entity);
            }
            Context.Entry(entity).State = EntityState.Deleted;
        }

        public Task DeleteAsync(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Entities.Attach(entity);
            }
            Context.Entry(entity).State = EntityState.Deleted;
            return Task.CompletedTask;
        }

        public void Delete(params object[] ids)
        {
            var entity = Entities.Find(ids);

            Delete(entity);
        }

        public async ValueTask DeleteAsync(params object[] ids)
        {
            var entity = Entities.Find(ids);

            await DeleteAsync(entity);
        }

        public T Find(params object[] ids)
        {
            return Entities.Find(ids);
        }

        public async ValueTask<T> FindAsync(params object[] ids)
        {
            return await Entities.FindAsync(ids);
        }

        public IReadOnlyList<T> Get()
        {
            return Entities.ToList();
        }

        public async ValueTask<IReadOnlyList<T>> GetAsync()
        {
            return await Entities.ToListAsync();
        }

        public T Update(T entity)
        {
            UpdateEntityInContext(entity);
            return entity;
        }

        public ValueTask<T> UpdateAsync(T entity)
        {
            UpdateEntityInContext(entity);
            return ValueTask.CompletedTask(entity);
        }

        private void UpdateEntityInContext(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Context.Attach(entity);
            }
            Context.Entry(entity).State = EntityState.Modified;
        }
    }
}