using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.BookEntry
{
    public interface IBookEntryRepository : IGenericRepository<Domain.Entities.BookEntry>
    {

    }
}
