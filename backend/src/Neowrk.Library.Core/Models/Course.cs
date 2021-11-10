using Neowrk.Library.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neowrk.Library.Core.Models
{
    [Table("Course")]
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public List<Guid> CategoriesOfBooksIds { get; set; }
    }
}
