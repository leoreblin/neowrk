using Neowrk.Library.Core.Models;
using Neowrk.Library.Core.ViewModels;
using Neowrk.Library.Rest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neowrk.Library.Service.Interfaces
{
    public interface IBookService : IBaseService<Book>
    {
        ApiResponse BorrowBook(Guid id, string studentEmail, string action);
        Task<List<BookViewModel>> GetAllAvailableBooks();
    }
}
