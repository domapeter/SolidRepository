using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solid.Repository.Interfaces
{
    public interface ITransaction : IDisposable
    {
        void Commit();

        Task CommitAsync(CancellationToken cancellationToken);

        void RollBack();

        Task RollBackAsync(CancellationToken cancellationToken);
    }
}