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
        var entities = GetWithIncludes(specification.Includes);
        entities = entities.Where(specification.Filter);
        var orderedentities = ApplyOrdering(specification.OrderByList, entities);

        return orderedentities.ToList();
    }

    public async ValueTask<IReadOnlyList<T>> GetAsync(CancellationToken cancellationToken, ISpecification<T> specification = null)
    {
        if (specification == null)
        {
            return await Entities.ToListAsync(cancellationToken);
        }
        var entities = GetWithIncludes(specification.Includes);
        entities = entities.Where(specification.Filter);
        var orderedentities = ApplyOrdering(specification.OrderByList, entities);

        return await orderedentities.ToListAsync(cancellationToken);
    }

    public T Update(T entity)
    {
        UpdateEntityInContext(entity);
        return entity;
    }

    public ValueTask<T> UpdateAsync(CancellationToken cancellationToken, T entity)
    {
        UpdateEntityInContext(entity);
        return ValueTask.FromResult(entity);
    }

    private void UpdateEntityInContext(T entity)
    {
        if (Context.Entry(entity).State == EntityState.Detached)
        {
            Context.Attach(entity);
        }
        Context.Entry(entity).State = EntityState.Modified;
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

        var entities = GetWithIncludes(specification.Includes);
        entities = entities.Where(specification.Filter);
        var orderedentities = ApplyOrdering(specification.OrderByList, entities);

        var totalRecords = orderedentities.Count();
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        var queryResult = orderedentities.Skip(skip).Take(pageSize);

        return new PageResult<T>()
        {
            FirstPage = 1,
            CurrentPage = pageNumber,
            LastPage = totalPages,
            NextPage = (pageNumber + 1) <= totalPages ? (pageNumber + 1) : -1,
            PreviousPage = (pageNumber - 1) >= 1 ? (pageNumber - 1) : -1,
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

        var entities = GetWithIncludes(specification.Includes);
        entities = entities.Where(specification.Filter);
        var orderedentities = ApplyOrdering(specification.OrderByList, entities);

        var totalRecords = await orderedentities.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

        var queryResult = orderedentities.Skip(skip).Take(pageSize);

        return new PageResult<T>()
        {
            FirstPage = 1,
            CurrentPage = pageNumber,
            LastPage = totalPages,
            NextPage = (pageNumber + 1) <= totalPages ? (pageNumber + 1) : -1,
            PreviousPage = (pageNumber - 1) >= 1 ? (pageNumber - 1) : -1,
            Size = pageSize,
            Records = await queryResult.ToListAsync(cancellationToken)
        };
    }

    protected static IOrderedQueryable<T> ApplyOrdering(List<KeyValuePair<Expression<Func<T, object>>, SortDirection>> orderByList, IQueryable<T> entities)
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