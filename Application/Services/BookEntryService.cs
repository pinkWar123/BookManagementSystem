using AutoMapper;
using BookManagementSystem.Application.Dtos.BookEntry;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Infrastructure.Repositories.BookEntry;
using BookManagementSystem.Application.Exceptions;
using BookManagementSystem.Application.Queries;
using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Application.Dtos.Book;
using BookManagementSystem.Application.Dtos.InventoryReportDetail;
namespace BookManagementSystem.Application.Services
{

    public class BookEntryService : IBookEntryService
    {
        private readonly IBookEntryRepository _bookEntryRepository;
        private readonly IMapper _mapper;
        private readonly IRegulationService _regulationService;        
        private readonly IBookService _bookService;
        private readonly IInventoryReportService _inventoryReportService;
        private readonly IInventoryReportDetailService _inventoryReportDetailService;

        public BookEntryService(
            IBookEntryRepository bookEntryRepository, 
            IRegulationService regulationService,
            IBookService bookService,
            IInventoryReportService inventoryReportService,
            IInventoryReportDetailService inventoryReportDetailService,
            IMapper mapper)
        {
            _bookEntryRepository = bookEntryRepository ?? throw new ArgumentNullException(nameof(bookEntryRepository));
            _regulationService = regulationService ?? throw new ArgumentNullException(nameof(regulationService));
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
            _inventoryReportService = inventoryReportService ?? throw new ArgumentNullException(nameof(inventoryReportService));
            _inventoryReportDetailService = inventoryReportDetailService ?? throw new ArgumentNullException(nameof(inventoryReportDetailService));
            _mapper = mapper;
        
        }

        public async Task<BookEntryDto> CreateNewBookEntry(CreateBookEntryDto createBookEntryDto)
        {
            var bookEntry = _mapper.Map<BookEntry>(createBookEntryDto);
            var bookEntryDetails  = createBookEntryDto.BookEntryDetails;
            var regulation = await _regulationService.GetMinimumBookEntry();
            foreach (var bookEntryDetail in bookEntryDetails)
            {
                if (regulation?.Value > bookEntryDetail.Quantity)
                {
                    throw new ExceedMinimumBookEntry();
                }
            }
            // Add stock to book
            bookEntry = _mapper.Map<BookEntry>(createBookEntryDto);
            var inventoryReportID = await _inventoryReportService.GetReportIdByMonthYear(bookEntry.Date.Month, bookEntry.Date.Year);
            // check inventory report null
            foreach (var bookEntryDetail in bookEntryDetails)
            {
                var book = await _bookService.GetBookById(bookEntryDetail.BookID);
                var inventoryReportDetail = await _inventoryReportDetailService.GetInventoryReportDetailById(inventoryReportID, bookEntryDetail.BookID);
                if (inventoryReportDetail == null)
                {
                    throw new InventoryReportDetailNotFound(inventoryReportID, bookEntryDetail.BookID);
                }
            }
            // update stock quantity
            foreach (var bookEntryDetail in bookEntryDetails)
            {
                // update book quantity
                var book = await _bookService.GetBookById(bookEntryDetail.BookID);
                book.StockQuantity += bookEntryDetail.Quantity;
                await _bookService.UpdateBook(book.BookId, _mapper.Map<UpdateBookDto>(book));             
                // update inventory report detail final stock
                var inventoryReportDetail = await _inventoryReportDetailService.GetInventoryReportDetailById(inventoryReportID, bookEntryDetail.BookID);
                inventoryReportDetail.FinalStock += bookEntryDetail.Quantity;
                await _inventoryReportDetailService.UpdateInventoryReportDetail(inventoryReportID, bookEntryDetail.BookID, _mapper.Map<UpdateInventoryReportDetailDto>(inventoryReportDetail)); 
            }


            await _bookEntryRepository.AddAsync(bookEntry);
            await _bookEntryRepository.SaveChangesAsync();
            return _mapper.Map<BookEntryDto>(bookEntry);
        }

        public async Task<BookEntryDto> UpdateBookEntry(int EntryID, UpdateBookEntryDto updateBookEntryDto)
        {   
            
            var existingEntry = await _bookEntryRepository.GetByIdAsync(EntryID);
            if (existingEntry == null)
            {
                throw new BookEntryException($"Không tìm thấy BookEntry với EntryID {EntryID}.");
            }

            _mapper.Map(updateBookEntryDto, existingEntry);
            var updatedEntry = await _bookEntryRepository.UpdateAsync(EntryID, existingEntry);
            await _bookEntryRepository.SaveChangesAsync();
            return _mapper.Map<BookEntryDto>(updatedEntry);
        }

        public async Task<BookEntryDto> GetBookEntryById(int EntryID)
        {
            var bookEntry = await _bookEntryRepository.GetByIdAsync(EntryID);
            if (bookEntry == null)
            {
                throw new BookEntryException($"Không tìm thấy BookEntry với EntryID {EntryID}.");
            }
            return _mapper.Map<BookEntryDto>(bookEntry);
        }

        public async Task<IEnumerable<BookEntryDto>> GetAllBookEntries(BookEntryQuery bookEntryQuery)
        {
            var query = _bookEntryRepository.GetValuesByQuery(bookEntryQuery);
            if (query == null)
            {
                return Enumerable.Empty<BookEntryDto>();
            }

            var bookEntries = await query.ToListAsync();

            return _mapper.Map<IEnumerable<BookEntryDto>>(bookEntries);
        }
       

        public async Task<bool> DeleteBookEntry(int EntryID)
        {
            var bookEntry = await _bookEntryRepository.GetByIdAsync(EntryID);

            if (bookEntry == null)
            {
                throw new BookEntryException($"Không tìm thấy BookEntry với EntryID {EntryID}");
            }
            
            _bookEntryRepository.Remove(bookEntry);
            await _bookEntryRepository.SaveChangesAsync();
            
            return true;
        }
    }
}