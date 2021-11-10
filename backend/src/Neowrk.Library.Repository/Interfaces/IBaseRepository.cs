using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neowrk.Library.Repository.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(Guid id);
        void Save(TEntity entity);
        Task<bool> Delete(TEntity entity);
    }
}
