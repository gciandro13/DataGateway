using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace EFDataGateway
{
    public interface IUnitOfWork<T> : IDisposable where T: DbContext
    {
        Task<int> CompleteAsync();
        Task BeginTransaction();
        void Dispose();
        void Commit();
        void Rollback();
        T Context { get; }

    }

    public class UnitOfWork<T>: IUnitOfWork<T>, IDisposable where T: DbContext
    {
        public T Context { get; }
        private IDbContextTransaction Transaction { get; set; }
      

        public UnitOfWork(T context)
        {
            Context = context;
        }
        public async Task BeginTransaction()
        {
            var transaction = await Context.Database.BeginTransactionAsync();
            Transaction = transaction;
        }

        public void Commit()
        {
            Transaction.Commit();
        }

        public async Task<int> CompleteAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }
    }
}
