using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Exceptions
{
    public class UserException : BaseException
    {
        public UserException(string message, HttpStatusCode statusCode = HttpStatusCode.NotFound) : base(message, statusCode)
        {
        }
    }

    public class InvalidTokenException : BaseException
    {
        public InvalidTokenException(string token) : base($"Token ${token} is not valid", HttpStatusCode.Unauthorized)
        {
        }

    }
}