using Neowrk.Library.Core.Models;
using Neowrk.Library.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neowrk.Library.Repository.Interfaces
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        bool CheckBookCategory(Guid bookId, string studentEmail); 
    }
}
