using System.ComponentModel.DataAnnotations.Schema;
using Neowrk.Library.Data;

namespace Neowrk.Library.Core.Models
{
    [Table("BookCategory")]
    public class BookCategory : BaseEntity
    {
        public string Name { get; set; }
    }
}
