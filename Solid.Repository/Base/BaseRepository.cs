using Microsoft.EntityFrameworkCore;
using Solid.Models;
using Solid.Models.Interfaces;
using Solid.Repository.Interfaces;
using System.Linq.Expressions;

namespace Solid.Repository.Base;

public class BaseRepository<T> : IRepository<T> where T : class, IEntity
{
    protected DbContext Context;

    protected DbSet<T> Entities => Context.Set<T>();

    public BaseRepository(DbContext context)
    {
        this.Context = context;
    }

    public async ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await Context.SaveChangesAsync(cancellationToken);
    }

    public int SaveChanges()
    {
        return Context.SaveChanges();
    }

    public ITransaction BeginTransaction()
    {
        return new Transaction(Context.Database.BeginTransaction());
    }

    public async ValueTask<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return new Transaction(await Context.Database.BeginTransactionAsync(cancellationToken));
    }

    public T Create(T entity)
    {
        Entities.Add(entity);

        return entity;
    }

    public async ValueTask<T> CreateAsync(CancellationToken cancellationToken, T entity)
    {
        await Entities.AddAsync(entity, cancellationToken);
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

    public Task DeleteAsync(CancellationToken cancellationToken, T entity)
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

    public async Task DeleteAsync(CancellationToken cancellationToken, params object[] ids)
    {
        var entity = Entities.FindAsync(ids, cancellationToken);

        await DeleteAsync(cancellationToken, entity);
    }

    public T Find(params object[] ids)
    {
        return Entities.Find(ids);
    }

    public async ValueTask<T> FindAsync(CancellationToken cancellationToken, params object[] ids)
    {
        return await Entities.FindAsync(ids, cancellationToken);
    }

    public IReadOnlyList<T> Get(ISpecification<T> specification = null)
    {
        if (specification == null)
        {
            return Entities.ToList();
        }

        var orderedentities = GetFilteredOrdered(specification);

        return orderedentities.ToList();
    }

    public IReadOnlyList<EndResult> Get<EndResult>(ISpecification<T, EndResult> specification)
    {
        if (specification == null)
        {
            throw new ArgumentNullException(nameof(specification));
        }

        var orderedentities = GetFilteredOrdered(specification);

        return specification.Projection.Invoke(orderedentities).ToList();
    }

    public async ValueTask<IReadOnlyList<EndResult>> GetAsync<EndResult>(CancellationToken cancellationToken, ISpecification<T, EndResult> specification)
    {
        if (specification == null)
        {
            throw new ArgumentNullException(nameof(specification));
        }

        var orderedentities = GetFilteredOrdered(specification);

        return await specification.Projection.Invoke(orderedentities).ToListAsync(cancellationToken);
    }

    public async ValueTask<IReadOnlyList<T>> GetAsync(CancellationToken cancellationToken, ISpecification<T> specification = null)
    {
        if (specification == null)
        {
            return await Entities.ToListAsync(cancellationToken);
        }

        var orderedentities = GetFilteredOrdered(specification);

        return await orderedentities.ToListAsync(cancellationToken);
    }

    public T Update(T entity)
    {
        Entities.Update(entity);
        return entity;
    }

    public ValueTask<T> UpdateAsync(CancellationToken cancellationToken, T entity)
    {
        Entities.Update(entity);
        return ValueTask.FromResult(entity);
    }

    public IPageResult<T> GetPagedResult(ISpecification<T> specification)
    {
        if (specification == null)
        {
            throw new ArgumentNullException(nameof(specification));
        }

        if (specification.Pagination == null)
        {
            throw new ArgumentNullException(nameof(specification.Pagination));
        }

        var pageNumber = specification.Pagination.PageNumber;
        var pageSize = specification.Pagination.PageSize;

        var skip = (pageNumber - 1) * pageSize;

        var orderedentities = GetFilteredOrdered(specification);

        var totalRecords = orderedentities.Count();

        var queryResult = orderedentities.Skip(skip).Take(pageSize);

        return new PageResult<T>()
        {
            CurrentPage = pageNumber,
            TotalCount = totalRecords,
            Size = pageSize,
            Records = queryResult.ToList()
        };
    }

    public async ValueTask<IPageResult<T>> GetPagedResultAsync(CancellationToken cancellationToken, ISpecification<T> specification)
    {
        if (specification == null)
        {
            throw new ArgumentNullException(nameof(specification));
        }

        if (specification.Pagination == null)
        {
            throw new ArgumentNullException(nameof(specification.Pagination));
        }

        var pageNumber = specification.Pagination.PageNumber;
        var pageSize = specification.Pagination.PageSize;

        var skip = (pageNumber - 1) * pageSize;

        var orderedentities = GetFilteredOrdered(specification);

        var totalRecords = await orderedentities.CountAsync(cancellationToken);

        var queryResult = orderedentities.Skip(skip).Take(pageSize);

        return new PageResult<T>()
        {
            TotalCount = totalRecords,
            CurrentPage = pageNumber,
            Size = pageSize,
            Records = await queryResult.ToListAsync(cancellationToken)
        };
    }

    protected virtual IQueryable<T> GetFilteredOrdered(ISpecification<T> specification)
    {
        var entities = GetWithIncludes(specification.Includes);
        if (specification.Filter != null)
        {
            entities = entities.Where(specification.Filter);
        }
        return ApplyOrdering(specification.OrderByList, entities);
    }

    protected virtual IOrderedQueryable<T> ApplyOrdering(List<KeyValuePair<Expression<Func<T, object>>, SortDirection>> orderByList, IQueryable<T> entities)
    {
        if (orderByList == null || orderByList.Count == 0)
        {
            return entities.OrderBy(p => true);
        }
        IOrderedQueryable<T> orderedentities = null;

        foreach ((var expression, SortDirection direction) in orderByList)
        {
            if (orderedentities == null)
            {
                orderedentities = direction == SortDirection.Ascending ? orderedentities.OrderBy(expression) : orderedentities.OrderByDescending(expression);
            }
            else
            {
                orderedentities = direction == SortDirection.Ascending ? orderedentities.ThenBy(expression) : orderedentities.ThenByDescending(expression);
            }
        }
        return orderedentities;
    }

    protected virtual IQueryable<T> GetWithIncludes(List<Expression<Func<T, object>>> includes)
    {
        IQueryable<T> query = Entities;

        if (includes == null || !includes.Any())
        {
            return query;
        }

        return includes
             .Aggregate(Entities.AsQueryable(),
                (current, include) => current.Include(include));
    }
}