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
    public class DbCourseRepository : ICourseRepository
    {
        private readonly IDbConnection _dbConnection;

        public DbCourseRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task CreateAsync(Course entity)
        {
            var sql = "INSERT INTO Courses (Name) OUTPUT INSERTED.[Id] VALUES (@Name)";
            var id = await _dbConnection.QuerySingleAsync<int>(sql, entity);
            entity.Id = id;
        }

        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM Courses WHERE Id=@Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }

        public Course? Get(int id)
        {
            var result = _dbConnection.QueryFirst<Course>("select * from Courses WHERE Id=@Id", new { Id = id });
            return result;
        }

        public IList<Course?> GetAll()
        {
            var result = _dbConnection.Query<Course>("select * from Courses");
            return result.ToList();
        }

        public async Task<IList<Course?>> GetAllAsync()
        {
            var result = await _dbConnection.QueryAsync<Course>("select * from Courses");
            return result.ToList();
        }

        public IList<Course> GetAllWithPredicate(Expression<Func<Course, bool>> predicate)
        {
            var result = _dbConnection.Query<Course>("select * from Courses");
            return result.Where(predicate.Compile()).ToList();
        }

        public async Task<Course?> GetAsync(int id)
        {
            var result = await _dbConnection.QueryFirstAsync<Course>("select * from Courses WHERE Id=@Id", new { Id = id });
            return result;
        }

        public IEnumerable<Course> GetCoursesByCategory(int categoryId)
        {
            var result = _dbConnection.Query<Course>("select * from Courses WHERE CategoryId=@CategoryId", new { CategoryId = categoryId });
            return result;
        }

        public IEnumerable<Course> GetCoursesByName(string name)
        {
            var result = _dbConnection.Query<Course>("select * from Courses WHERE Name=@Name", new { Name = name });
            return result;
        }

        public async Task UpdateAsync(Course entity)
        {
            await _dbConnection.ExecuteAsync("UPDATE Courses SET Name=@Name,CategoryId=@CategoryId WHERE Id=@Id", entity);
        }
    }
}
