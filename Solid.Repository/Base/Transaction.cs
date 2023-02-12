using Microsoft.EntityFrameworkCore.Storage;
using Solid.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.Repository.Base
{
    public class Transaction : ITransaction
    {
        private readonly IDbContextTransaction dbContextTransaction;

        public Transaction(IDbContextTransaction dbContextTransaction)
        {
            this.dbContextTransaction = dbContextTransaction;
        }

        public void Commit() => dbContextTransaction.Commit();

        public Task CommitAsync(CancellationToken cancellationToken) => dbContextTransaction.CommitAsync(cancellationToken);

        public void Dispose()
        {
            dbContextTransaction?.Dispose();
        }

        public void RollBack() => dbContextTransaction.Rollback();

        public Task RollBackAsync(CancellationToken cancellationToken) => dbContextTransaction.RollbackAsync(cancellationToken);
    }
}