
using EFDataGateway;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFDataGateway
{

    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> All();
        Task<T> GetById(int id);
        Task<bool> Add(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Upsert(T entity);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);

    }

    public class GenericRepository<T,Tcontext> : IGenericRepository<T> where T : class 
        where Tcontext : DbContext
    {

        public readonly IUnitOfWork<Tcontext> _uow;

        public GenericRepository(IUnitOfWork<Tcontext> uow)
        {
            _uow = uow;
        }

        public virtual async Task<bool> Add(T entity)
        {
            await _uow.Context.AddAsync(entity);
            return true;
        }

        public virtual async Task<bool> AddAndGetId(T entity)
        {
            await _uow.Context.AddAsync(entity);
            return true;
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<bool> Delete(T entity)
        {
            _uow.Context.Remove(entity);
            return true;
        }

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _uow.Context.Set<T>().Where(predicate).ToListAsync();

        }

        public virtual async Task<T> GetById(int id)
        {
            return await _uow.Context.Set<T>().FindAsync(id);
        }

        public virtual async Task<bool> Upsert(T entity)
        {
            _uow.Context.Set<T>().Update(entity);
            return true;
        }
    }

}
