using System;
using System.Collections.Generic;
using System.Text;

namespace Neowrk.Library.Core.DTO
{
    public class BorrowBookDto
    {
        public Guid BookId { get; set; }
        public string StudentEmail { get; set; }
        public string Action { get; set; }
    }
}
