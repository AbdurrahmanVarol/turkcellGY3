using CourseApp.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CourseApp.Infrastructure.Repositories.Dapper
{
    public class DpCaregoryRepository : ICategoryRepository
    {
         private readonly IDbConnection _dbConnection;

        public DpCaregoryRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task CreateAsync(Category entity)
        {
            var sql = "INSERT INTO Categories (Name) OUTPUT INSERTED.[Id] VALUES (@Name)";
            var id = await _dbConnection.QuerySingleAsync<int>(sql, entity);
            entity.Id = id;
        }

        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM Categories WHERE Id=@Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }

        public Category? Get(int id)
        {
            var result = _dbConnection.QueryFirst<Category>("select * from Categories WHERE Id=@Id", new { Id = id });
            return result;
        }

        public IList<Category?> GetAll()
        {
            var result = _dbConnection.Query<Category>("select * from Categories");
            return result.ToList();
        }

        public async Task<IList<Category?>> GetAllAsync()
        {
            var result = await _dbConnection.QueryAsync<Category>("select * from Categories");
            return result.ToList();
        }

        public IList<Category> GetAllWithPredicate(Expression<Func<Category, bool>> predicate)
        {
            var result = _dbConnection.Query<Category>("select * from Categories");
            return result.Where(predicate.Compile()).ToList();
        }

        public async Task<Category?> GetAsync(int id)
        {
            var result = await _dbConnection.QueryFirstAsync<Category>("select * from Categories WHERE Id=@Id", new { Id = id });
            return result;
        }

        public async Task UpdateAsync(Category entity)
        {
            await _dbConnection.ExecuteAsync("UPDATE Categories SET Name=@Name WHERE Id=@Id", entity);
        }
    }
}
