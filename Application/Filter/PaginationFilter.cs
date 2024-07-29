
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Filter
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public static int MaxPageSize = 4;
        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = MaxPageSize;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;
        }
    }
}
