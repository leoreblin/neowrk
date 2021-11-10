using Neowrk.Library.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Neowrk.Library.Core.Models
{
    [Table("Book")]
    public class Book : BaseEntity
    {        
        public string Title { get; set; }
        public string Author { get; set; }
        public int Pages { get; set; }
        public string Publisher { get; set; }
        public Guid BookCategoryId { get; set; }
        public Guid? LentToStudentId { get; set; }
    }
}
