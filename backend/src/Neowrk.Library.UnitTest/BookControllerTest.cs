using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Neowrk.Library.Api.Controllers;
using Neowrk.Library.Rest;
using Neowrk.Library.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Neowrk.Library.UnitTest
{
    [TestClass]
    [TestCategory("UnitTest > Controllers > Book")]
    public class BookControllerTest
    {
        [TestMethod]
        public void BorrowBookWithWrongParameters()
        {
            var bookServiceMock = new Mock<IBookService>();
            var bookController = new BookController(bookServiceMock.Object);
            var actionResult = bookController.Post(Guid.Parse("846CD659-B282-41CD-923E-267199B1EEDF"), "leoreblin@gmail.com", "borrow");
            var result = actionResult.Result;
            Assert.IsNotNull(result);
            Assert.AreNotEqual(200, result.Code);            
        }

        [TestMethod]
        public void Get()
        {
            var bookServiceMock = new Mock<IBookService>();
            var bookController = new BookController(bookServiceMock.Object);
            var actionResult = bookController.Get();

            var result = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            
        }
    }
}
