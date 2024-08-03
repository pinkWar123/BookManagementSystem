using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Helpers;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookEntry> BookEntrys { get; set; }
        public DbSet<InventoryReport> InventoryReports { get; set; }
        public DbSet<InventoryReportDetail> InventoryReportDetails { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<PaymentReceipt> PaymentReceipts { get; set; }

        public DbSet<DebtReport> DebtReports { get; set; }

        public DbSet<DebtReportDetail> DebtReportDetails { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        // protected override void ConfigureConventions
        // (ModelConfigurationBuilder builder)        
        // {
        //     builder.Properties<DateOnly>()                
        //         .HaveConversion<DateOnlyConverter>()                
        //         .HaveColumnType("date");
        //     builder.Properties<DateOnly?>()                
        //         .HaveConversion<NullableDateOnlyConverter>()                
        //         .HaveColumnType("date");        
        // }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Primary key
            // Book: 
            builder.Entity<Book>()
                .HasKey(p => p.Id);

            // Book Entry: 
            builder.Entity<BookEntry>()
                .HasKey(p => p.Id);

            //Book Entry Detail: 
            builder.Entity<BookEntryDetail>()
                .HasKey(b => new { b.EntryID, b.BookID });

            // Inventory Report : 
            builder.Entity<InventoryReport>()
                .HasKey(p => p.Id);

            // Inventory Report Details : 
            builder.Entity<InventoryReportDetail>()
                .HasKey(b => new { b.ReportID, b.BookID });

            //customer :
            builder.Entity<Customer>()
                .HasKey(p => p.Id);

            builder.Entity<Customer>()
                .Property(c => c.Id);
                

            //payment Receive : 
            builder.Entity<PaymentReceipt>()
                .HasKey(p => p.Id);

            //Debt Report : 
            builder.Entity<DebtReport>()
                .HasKey(p => p.Id);

            // Debt Report Details : 
            builder.Entity<DebtReportDetail>()
                .HasKey(b => new { b.ReportID, b.CustomerID });

            //Invoice : 
            builder.Entity<Invoice>()
                .HasKey(p => p.Id);

            //Invoice Detail :
            builder.Entity<InvoiceDetail>()
               .HasKey(b => new { b.InvoiceID, b.BookID });

            //User : 
            // builder.Entity<User>()
            //     .HasKey(g => g.Id);

            builder.Entity<Regulation>()
                .HasKey(p => p.Id);
            /////////////////////////////////////////////

            // other
            // builder.Entity<User>()
            // .Property(e => e.Role)
            // .HasConversion<string>();
            //////////////////////////////// 

            //Foreign Key         
            builder.Entity<BookEntryDetail>()
            .HasOne(bed => bed.BookEntry)
            .WithMany(be => be.BookEntryDetails)
            .HasForeignKey(bed => bed.EntryID)
            .HasConstraintName("FK_BookEntryDetail_BookEntry");

            builder.Entity<BookEntryDetail>()
            .HasOne(bed => bed.Book)
            .WithMany(be => be.BookEntryDetails)
            .HasForeignKey(bed => bed.BookID)
            .HasConstraintName("FK_BookEntryDetail_Book");

            builder.Entity<InventoryReportDetail>()
            .HasOne(bed => bed.Book)
            .WithMany(be => be.InventoryReportDetails)
            .HasForeignKey(bed => bed.BookID)
            .HasConstraintName("FK_InventoryReportDetail_Book");

            builder.Entity<InventoryReportDetail>()
            .HasOne(bed => bed.InventoryReport)
            .WithMany(be => be.InventoryReportDetails)
            .HasForeignKey(bed => bed.ReportID)
            .HasConstraintName("FK_InventoryReportDetail_InventoryReport");

            builder.Entity<PaymentReceipt>()
            .HasOne(bed => bed.Customer)
            .WithMany(be => be.PaymentReceipts)
            .HasForeignKey(bed => bed.CustomerID)
            .HasConstraintName("FK_PaymentReceipt_Customer");

            builder.Entity<DebtReportDetail>()
            .HasOne(bed => bed.DebtReport)
            .WithMany(be => be.DebtReportDetails)
            .HasForeignKey(bed => bed.ReportID)
            .HasConstraintName("FK_DebtReportDetail_DebtReport");

            builder.Entity<DebtReportDetail>()
            .HasOne(bed => bed.Customer)
            .WithMany(be => be.DebtReportDetails)
            .HasForeignKey(bed => bed.CustomerID)
            .HasConstraintName("FK_DebtReportDetail_Customer");

            builder.Entity<Invoice>()
            .HasOne(bed => bed.Customer)
            .WithMany(be => be.Invoices)
            .HasForeignKey(bed => bed.CustomerID)
            .HasConstraintName("FK_Invoice_Customer");

            builder.Entity<InvoiceDetail>()
            .HasOne(bed => bed.Book)
            .WithMany(be => be.invoiceDetails)
            .HasForeignKey(bed => bed.BookID)
            .HasConstraintName("FK_InvoiceDetail_Book");

            builder.Entity<InvoiceDetail>()
            .HasOne(bed => bed.Invoice)
            .WithMany(be => be.InvoiceDetails)
            .HasForeignKey(bed => bed.InvoiceID)
            .HasConstraintName("FK_InvoiceDetail_Invoice");

            ///////////////////////////////////////
        }
    }
}
