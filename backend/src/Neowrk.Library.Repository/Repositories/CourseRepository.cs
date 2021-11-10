using Microsoft.Extensions.Configuration;
using Neowrk.Library.Core.Models;
using Neowrk.Library.Repository.Interfaces;

namespace Neowrk.Library.Repository.Repositories
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
