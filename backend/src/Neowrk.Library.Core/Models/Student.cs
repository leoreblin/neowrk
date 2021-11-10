using Neowrk.Library.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neowrk.Library.Core.Models
{
    [Table("Student")]
    public class Student : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Guid CourseId { get; set; }
    }
}
