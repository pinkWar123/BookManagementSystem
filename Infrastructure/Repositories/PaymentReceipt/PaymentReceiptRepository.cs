using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.PaymentReceipt
{
    public class PaymentReceiptRepository : GenericRepository<Domain.Entities.PaymentReceipt>, IPaymentReceiptRepository
    {
        public PaymentReceiptRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
    
}