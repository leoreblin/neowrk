using Dapper;
using Microsoft.Extensions.Configuration;
using Neowrk.Library.Core.Models;
using Neowrk.Library.Repository.Interfaces;
using System;
using System.Data;
using System.Linq;

namespace Neowrk.Library.Repository.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public bool CheckBookCategory(Guid bookId, string studentEmail)
        {
            using IDbConnection conn = _dbConnection;
            string query = string.Format(
            @"SELECT *
                    FROM Book
                   WHERE BookCategoryId = (
                     SELECT CategoryId
                       FROM CourseBooksCategories
                      WHERE CourseId = (SELECT TOP 1 CourseId FROM Student WHERE Email = '{0}'))", studentEmail);

            var bookIds = _dbConnection.Query<Book>(query).Select(x => x.Id);
            if (bookIds.ToList().Contains(bookId))
            {
                return true;
            }
            return false;
        }
    }
}
