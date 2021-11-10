using Dapper;
using Microsoft.Data.Sqlite;
using Neowrk.Library.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neowrk.Library.Repository
{
    public class DatabaseBootstrap
    {
        private readonly string _connectionString;

        public DatabaseBootstrap(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public async Task Setup()
        {            
            using IDbConnection dbConnection = new SqlConnection(_connectionString);

            await CreateTables(dbConnection);
            await SeedData(dbConnection);    
        }

        private async Task SeedData(IDbConnection dbConnection)
        {
            var categories = new List<BookCategory>()
            {
                new BookCategory(){ Id = Guid.NewGuid(), Name = "TI"},
                new BookCategory(){ Id = Guid.NewGuid(), Name = "Engenharia Civil"},
            };

            var books = new List<Book>() {
                new Book() { Id = Guid.NewGuid(), Author = "Uncle Bob", Title = "Clean Code", Publisher = "Atlas", Pages = 429, BookCategoryId = categories.First(c => c.Name == "TI").Id },
                new Book() { Id = Guid.NewGuid(), Author = "Andrew Hunt", Title = "Pragmatic Programmer", Publisher = "Addison-Wesley Professional", Pages = 352, BookCategoryId = categories.First(c => c.Name == "TI").Id },
                new Book() { Id = Guid.NewGuid(), Author = "Hélio Alves de Azeredo", Title = "O edifício até sua cobertura", Publisher = "Blucher", Pages = 193, BookCategoryId = categories.First(c => c.Name == "Engenharia Civil").Id },
                new Book() { Id = Guid.NewGuid(), Author = "Manoel Henrique Campos", Title = "Concreto armado: eu te amo", Publisher = "Blucher", Pages = 652, BookCategoryId = categories.First(c => c.Name == "Engenharia Civil").Id },
            };

            var courses = new List<Course>()
            {
                new Course() { Id = Guid.NewGuid(), Name = "Sistemas de Infomração", CategoriesOfBooksIds = new List<Guid>() {  categories.First(c => c.Name == "TI").Id } },
                new Course() { Id = Guid.NewGuid(), Name = "Engenharia Civil", CategoriesOfBooksIds = new List<Guid>() {  categories.First(c => c.Name == "Engenharia Civil").Id } }
            };

            var students = new List<Student>()
            {
                new Student() { Id = Guid.NewGuid(), Name = "John Doe", Email="hdinizribeiro@gmail.com", CourseId = courses.First(c => c.Name == "Sistemas de Infomração").Id },
                new Student() { Id = Guid.NewGuid(), Name = "Huguinho", Email="huguinho@domain.com", CourseId = courses.First(c => c.Name == "Sistemas de Infomração").Id },
                new Student() { Id = Guid.NewGuid(), Name = "Zezinho", Email="zezinho@domain.com", CourseId = courses.First(c => c.Name == "Engenharia Civil").Id },
                new Student() { Id = Guid.NewGuid(), Name = "Luizinho", Email="luizinho@domain.com", CourseId = courses.First(c => c.Name == "Engenharia Civil").Id },
                new Student() { Id = Guid.NewGuid(), Name = "Leo", Email="leoreblin@gmail.com", CourseId = courses.First(c => c.Name == "Sistemas de Infomração").Id }
            };

            await dbConnection.ExecuteAsync("USE [NeowrkLibrary]; delete from Book; delete from Student; delete from CourseBooksCategories; delete from Course; delete from BookCategory;");

            await dbConnection.ExecuteAsync("USE [NeowrkLibrary]; INSERT INTO BookCategory (Id, Name) VALUES (@Id, @Name)", categories);

            await dbConnection.ExecuteAsync("USE [NeowrkLibrary]; INSERT INTO Book (Id, Title, Author, Publisher, Pages, BookCategoryId) VALUES (@Id, @Title, @Author, @Publisher, @Pages, @BookCategoryId)", books);

            await dbConnection.ExecuteAsync("USE [NeowrkLibrary]; INSERT INTO Course (Id, Name) VALUES (@Id, @Name)", courses);

            foreach (var course in courses)
            {
                await dbConnection.ExecuteAsync("USE [NeowrkLibrary]; INSERT INTO CourseBooksCategories (CourseId, CategoryId) VALUES (@CourseId, @CategoryId)", course.CategoriesOfBooksIds.Select(catId => new { CourseId = course.Id, CategoryId = catId }));
            }

            await dbConnection.ExecuteAsync("USE [NeowrkLibrary]; INSERT INTO Student (Id, Name, Email, CourseId) VALUES (@Id, @Name, @Email, @CourseId)", students);
        }

        private async Task CreateTables(IDbConnection dbConnection)
        {
            string createDatabseQuery = @"
            IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'NeowrkLibrary')
            BEGIN
                CREATE DATABASE [NeowrkLibrary]
            END";
            
            string createTablesQuery = 
            @"USE [NeowrkLibrary]
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='BookCategory' and xtype='U')
            BEGIN
                CREATE TABLE [dbo].[BookCategory]
                (
                    [Id] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL PRIMARY KEY,
                    [Name] NVARCHAR (150) NOT NULL,
                );
            END

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Course' and xtype='U')
            BEGIN        
                CREATE TABLE [dbo].[Course]
                (
                    [Id] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL PRIMARY KEY,
                    [Name] NVARCHAR (150) NOT NULL,
                );
            END;

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Student' and xtype='U')
            BEGIN
                CREATE TABLE [dbo].[Student]
                (	
                    [Id] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL PRIMARY KEY,
                    [Name] NVARCHAR (150) NOT NULL,
                    [Email] NVARCHAR (150) NOT NULL,

                    [CourseId] UNIQUEIDENTIFIER NOT NULL,
                    CONSTRAINT [FK_Student_Couse] FOREIGN KEY ([CourseId]) REFERENCES [Course]([Id]),
                );
            END;

            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='CourseBooksCategories' and xtype='U')
            BEGIN
                CREATE TABLE [dbo].[CourseBooksCategories]
                (
                    [CourseId] UNIQUEIDENTIFIER NOT NULL,
                    [CategoryId] UNIQUEIDENTIFIER NOT NULL,

                    CONSTRAINT [FK_CourseBooksCategories_Couse] FOREIGN KEY ([CourseId]) REFERENCES [Course]([Id]),
                    CONSTRAINT [FK_CourseBooksCategories_Category] FOREIGN KEY ([CategoryId]) REFERENCES [BookCategory]([Id]),
                    CONSTRAINT [PK_CourseBooksCategories] PRIMARY KEY ([CourseId], [CategoryId])
                );
            END;
            
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Book' and xtype='U')
            BEGIN
                CREATE TABLE [dbo].[Book]
                (
                    [Id] UNIQUEIDENTIFIER DEFAULT (newsequentialid()) NOT NULL PRIMARY KEY,
                    [Title] NVARCHAR (150) NOT NULL,
                    [Author] NVARCHAR (100) NOT NULL,
                    [Pages] INT NULL,
                    [Publisher] NVARCHAR (300) NOT NULL,
                    [BookCategoryId] UNIQUEIDENTIFIER NOT NULL,
                    [LentToStudentId] UNIQUEIDENTIFIER NULL

                    CONSTRAINT [FK_Book_Category] FOREIGN KEY ([BookCategoryId]) REFERENCES [BookCategory]([Id]),
                    CONSTRAINT [FK_Book_Student] FOREIGN KEY ([LentToStudentId]) REFERENCES [Student]([Id]),
                );
            END;";

            await dbConnection.ExecuteAsync(createDatabseQuery);
            await dbConnection.ExecuteAsync(createTablesQuery);
        }
    }
}
