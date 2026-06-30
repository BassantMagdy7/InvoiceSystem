using Business.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace DataAccess.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem>  InvoiceItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }

        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Store>().HasData(
                new Store { Id = 1, Name = "Alexandria" },
                new Store { Id = 2, Name = "Cairo" },
                new Store { Id = 3, Name = "Mansoura" }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Bassant Magdy" },
                new Customer { Id = 2, Name = "Sara Mohamed" },
                new Customer { Id = 3, Name = "Mohamed Hassan" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Price = 25000 },
                new Product { Id = 2, Name = "Mouse", Price = 350 },
                new Product { Id = 3, Name = "Keyboard", Price = 750 }
            );


            modelBuilder.Entity<Invoice>()
                .HasOne(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.CustomerId);

            modelBuilder.Entity<Invoice>()
                .HasMany(x => x.InvoiceItems)
                .WithOne(x => x.Invoice)
                .HasForeignKey(x => x.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);  // If an Invoice is deleted, delete its line items too

            modelBuilder.Entity<Invoice>()
                .HasOne(x => x.Store)
                .WithMany()
                .HasForeignKey(x => x.StoreId);

            modelBuilder.Entity<InvoiceItem>()
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId);


        }
    }


}



