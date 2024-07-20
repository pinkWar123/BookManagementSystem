using BookManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookEntry> BookEntrys { get; set; }
        public DbSet<InventoryReport> InventoryReports { get; set; }
        public DbSet<InventoryReportDetail> InventoryReportDetails { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<PaymentReceive> PaymentReceives { get; set; }

        public DbSet<DeptReport> DeptReports { get; set; }

        public DbSet<DeptReportDetail> DeptReportDetails { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
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

            //payment Receive : 
            builder.Entity<PaymentReceive>()
                .HasKey(p => p.Id);

            //Dept Report : 
            builder.Entity<DeptReport>()
                .HasKey(p => p.Id);

            // Dept Report Details : 
            builder.Entity<DeptReportDetail>()
                .HasKey(b => new { b.ReportID, b.CustomerID });

            //Invoice : 
            builder.Entity<Invoice>()
                .HasKey(p => p.Id);

            //Invoice Detail :
            builder.Entity<InvoiceDetail>()
               .HasKey(b => new { b.InvoiceID, b.BookID });

            //Users : 
            builder.Entity<Users>()
                .HasKey(g => g.Id);  

            builder.Entity<Regulation>()
                .HasKey(p => p.Id);
            /////////////////////////////////////////////

            // other
            builder.Entity<Users>()
            .Property(e => e.Role)
            .HasConversion<string>();
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

            builder.Entity<PaymentReceive>()
            .HasOne(bed => bed.Customer)
            .WithMany(be => be.PaymentReceives)
            .HasForeignKey(bed => bed.CustomerID)
            .HasConstraintName("FK_PaymentReceive_Customer");

            builder.Entity<DeptReportDetail>()
            .HasOne(bed => bed.DeptReport)
            .WithMany(be => be.DeptReportDetails)
            .HasForeignKey(bed => bed.ReportID)
            .HasConstraintName("FK_DeptReportDetail_DeptReport");

            builder.Entity<DeptReportDetail>()
            .HasOne(bed => bed.Customer)
            .WithMany(be => be.DeptReportDetails)
            .HasForeignKey(bed => bed.CustomerID)
            .HasConstraintName("FK_DeptReportDetail_Customer");

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
