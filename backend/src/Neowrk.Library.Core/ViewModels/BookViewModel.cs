using System;
using System.Collections.Generic;
using System.Text;

namespace Neowrk.Library.Core.ViewModels
{
    public class BookViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Pages { get; set; }
        public string Publisher { get; set; }
    }
}
