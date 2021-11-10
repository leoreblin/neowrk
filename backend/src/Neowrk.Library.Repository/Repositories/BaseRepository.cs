using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using Neowrk.Library.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Neowrk.Library.Repository.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        public IDbConnection _dbConnection => new SqlConnection(_connectionString);

        protected readonly string _connectionString;

        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<TEntity> GetAll()
        {
            using IDbConnection conn = _dbConnection;
            var query = conn.GetAll<TEntity>();
            return query;
        }

        public TEntity GetById(Guid id)
        {
            using IDbConnection conn = _dbConnection;
            return _dbConnection.Get<TEntity>(id);
        }

        public async void Save(TEntity entity)
        {
            using IDbConnection conn = _dbConnection;
            await conn.UpdateAsync(entity);
        }

        public async Task<bool> Delete(TEntity entity)
        {
            using IDbConnection conn = _dbConnection;
            return await conn.DeleteAsync(entity);
        }
    }
}
