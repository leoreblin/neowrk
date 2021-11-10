using Microsoft.Extensions.Configuration;
using Neowrk.Library.Core.Models;
using Neowrk.Library.Repository.Interfaces;

namespace Neowrk.Library.Repository.Repositories
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
