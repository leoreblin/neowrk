using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neowrk.Library.Rest;
using Neowrk.Library.Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace Neowrk.Library.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        [Route("/api/books")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var books = await _bookService.GetAllAvailableBooks();
                return Ok(books);
            }
            catch (ApiException ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpPost]
        [Route("/api/books/{bookId:guid}/student/{studentEmail}")]
        [Authorize]
        public async Task<ApiResponse> Post([FromRoute] Guid bookId, [FromRoute] string studentEmail, [FromQuery] string action)
        {
            var response = new ApiResponse();
            try
            {
                switch (action)
                {
                    case "borrow":
                        response = _bookService.BorrowBook(bookId, studentEmail, action);
                        break;
                    default:
                        response.ErrorResponse("Invalid action");
                        return response;
                }

                return response;
            }
            catch (ApiException ex)
            {
                response = ex.GetResponse();
                response.Fields.ForEach(x => response.Messages.Add(x.Value));
                response.BadRequestResponse();
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorResponse(ex.Message);
                return response;
            }
            
        }
    }
}
