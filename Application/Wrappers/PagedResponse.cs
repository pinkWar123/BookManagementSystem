using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Identity;

namespace BookManagementSystem.Application.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }

        public PagedResponse(T data,int pageNumber, int pageSize, string? message, string[]? errors, bool? succeeded)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Message = message ?? string.Empty;
            Errors = errors ?? [];
            Succeeded = succeeded ?? true;
        }
    }
}
