using System.Threading;
using System.Threading.Tasks;
using CapaDatos.Repositorio.Infrastructure;
using CapaDatos.Repositorio.Repositories;

namespace CapaDatos.Repositorio.UnitOfWork
{
    public interface IUnitOfWorkAsync : IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class, IObjectState;
    }
}