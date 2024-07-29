using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Filter;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter paginationFilter, string route);
    }
}
