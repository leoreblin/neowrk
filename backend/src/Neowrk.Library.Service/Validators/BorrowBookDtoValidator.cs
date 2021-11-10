using System;
using FluentValidation;
using Neowrk.Library.Core.DTO;
using Neowrk.Library.Repository.Interfaces;
using System.Linq;

namespace Neowrk.Library.Service.Validators
{
    public class BorrowBookDtoValidator : AbstractValidator<BorrowBookDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IStudentRepository _studentRepository;

        public BorrowBookDtoValidator(IBookRepository bookRepository,
            IStudentRepository studentRepository)
        {
            _bookRepository = bookRepository;
            _studentRepository = studentRepository;

            RuleFor(x => x).Must(CheckIfBookExists).WithMessage(x => "Book not found");
            RuleFor(x => x).Must(CheckBookCategory).WithMessage(x => "You can't borrow this book");
            RuleFor(x => x.Action).NotEmpty().NotNull().WithMessage(x => "Invalid action");
            RuleFor(x => x.StudentEmail).NotEmpty().NotNull().WithMessage(x => "User email cannot be empty");
            RuleFor(x => x.StudentEmail).Must(CheckIfUserExists).WithMessage(x => "Invalid user email");
        }

        private bool CheckIfUserExists(string studentEmail)
        {
            var student = _studentRepository.GetAll()
                .Where(x => x.Email.Equals(studentEmail, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

            return student != null;
        }

        private bool CheckBookCategory(BorrowBookDto dto)
        {
            var check = _bookRepository.CheckBookCategory(dto.BookId, dto.StudentEmail);
            return check;
        }

        private bool CheckIfBookExists(BorrowBookDto dto)
        {
            var book = _bookRepository.GetById(dto.BookId);
            return book != null;
        }
    }
}
