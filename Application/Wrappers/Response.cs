using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Wrappers
{
    public class Response<T>
    {
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
        public Response() { }
        public Response(T data, string? message, string[]? errors, bool? succeeded)
        {
            Succeeded = succeeded ?? true;
            Message = message ?? string.Empty;
            Errors = errors ?? [];
            Data = data;
        }
    }
}
