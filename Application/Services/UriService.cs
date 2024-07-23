using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Filter;
using BookManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace BookManagementSystem.Application.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri = string.Empty;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        public Uri GetPageUri(PaginationFilter paginationFilter, string route)
        {
            var _endpointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri = QueryHelpers.AddQueryString(_endpointUri.ToString(), "pageNumber", paginationFilter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", paginationFilter.PageSize.ToString());
            return new Uri(modifiedUri);
        }
    }
}
