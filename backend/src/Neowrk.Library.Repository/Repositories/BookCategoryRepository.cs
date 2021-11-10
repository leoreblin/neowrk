using Microsoft.Extensions.Configuration;
using Neowrk.Library.Core.Models;
using Neowrk.Library.Repository.Interfaces;

namespace Neowrk.Library.Repository.Repositories
{
    public class BookCategoryRepository : BaseRepository<BookCategory>, IBookCategoryRepository
    {
        public BookCategoryRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
