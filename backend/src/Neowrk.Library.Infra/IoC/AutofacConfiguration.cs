using Autofac;
using Neowrk.Library.Repository.Interfaces;
using Neowrk.Library.Repository.Repositories;
using Neowrk.Library.Service.Interfaces;
using Neowrk.Library.Service.Services;
using Neowrk.Library.Service.Validators;
using System;
using System.Reflection;

namespace Neowrk.Library.Infra.IoC
{
    public static class AutofacConfiguration
    {
        public static void Load(ContainerBuilder builder)
        {

            #region Services
            builder.RegisterType<BookCategoryService>().As<IBookCategoryService>();
            builder.RegisterType<BookService>().As<IBookService>();
            builder.RegisterType<CourseService>().As<ICourseService>();
            builder.RegisterType<StudentService>().As<IStudentService>();
            #endregion

            #region Repositories
            builder.RegisterType<BookCategoryRepository>().As<IBookCategoryRepository>();
            builder.RegisterType<BookRepository>().As<IBookRepository>();
            builder.RegisterType<CourseRepository>().As<ICourseRepository>();
            builder.RegisterType<StudentRepository>().As<IStudentRepository>();
            #endregion

            #region Validators
            builder.RegisterAssemblyTypes(typeof(BorrowBookDtoValidator).Assembly)
                .Where(t => t.Name.EndsWith("Validator"))
                .AsImplementedInterfaces()
                .InstancePerRequest();
            #endregion

            
        }
    }
}
