using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.Book
{
    // Data transfer object
    public class BookViewDto
    {
        public string? Title1 { get; set; }
        public string? Price { get; set; }
        public string? Content { get; set; }
        public string? PrivateCode { get; set; }

    }

    public class Book
    {
        public string? Title { get; set; }
        public string? Price { get; set; }
        public string? Content { get; set; }
        public string? PrivateCode { get; set; }

    }

    public class User
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }

    public class RegisterDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }

    public class UserViewDto
    {
        public string? Usernam1 { get; set; }
        public string? Email { get; set; }
    }
}
