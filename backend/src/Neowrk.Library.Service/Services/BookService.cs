using Neowrk.Library.Core.DTO;
using Neowrk.Library.Core.Models;
using Neowrk.Library.Core.ViewModels;
using Neowrk.Library.Repository.Interfaces;
using Neowrk.Library.Repository.Repositories;
using Neowrk.Library.Rest;
using Neowrk.Library.Service.Interfaces;
using Neowrk.Library.Service.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neowrk.Library.Service.Services
{
    public class BookService : BaseService<Book>, IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly BorrowBookDtoValidator _borrowBookDtoValidator;

        public BookService(IBookRepository bookRepository,
            IStudentRepository studentRepository,
            BorrowBookDtoValidator borrowBookDtoValidator)
        {
            _bookRepository = bookRepository;
            _studentRepository = studentRepository;
            _borrowBookDtoValidator = borrowBookDtoValidator;
        }

        public ApiResponse BorrowBook(Guid id, string studentEmail, string action)
        {
            var validate = _borrowBookDtoValidator.Validate(new BorrowBookDto { BookId = id, StudentEmail = studentEmail, Action = action });
            var exc = new ApiException();
            exc.WithFluentValidation(validate);
            exc.ThrowIfHasError();

            var book = _bookRepository.GetById(id);
            var students = _studentRepository.GetAll();

            if (students.Count() > 0)
            {
                var student = students.Where(x => x.Email.Equals(studentEmail, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                book.LentToStudentId = student.Id;
            }

            _bookRepository.Save(book);
            var response = new ApiResponse();
            response.SuccessResponse("Happy studying!");

            return response;
        }

        public async Task<List<BookViewModel>> GetAllAvailableBooks()
        {
            var query = _bookRepository.GetAll();
            if (query.ToList().Count > 0)
            {
                query = query.Where(x => x.LentToStudentId == null || x.LentToStudentId == Guid.Empty);
            }

            var models = query.Select(book => new BookViewModel()
            {
                Id = book.Id,
                Title = book.Title,
                Pages = book.Pages,
                Author = book.Author,
                Publisher = book.Publisher
            });

            return models.ToList();
        }
    }
}
