using InvestorFlow.ContactManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace InvestorFlow.ContactManagement.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Fund> Funds { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Schema definition
        modelBuilder.Entity<Contact>()
            .HasOne(contact => contact.Fund)
            .WithMany(fund => fund.Contacts)
            .HasForeignKey(contact => contact.FundId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of Fund if Contacts are assigned

        modelBuilder.Entity<Contact>()
            .HasIndex(contact => contact.ExternalId)
            .IsUnique(); // Ensure ExternalId is unique across all contacts
        
        modelBuilder.Entity<Contact>()
            .Property(contact => contact.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        modelBuilder.Entity<Contact>()
            .Property(contact => contact.Email)
            .HasMaxLength(150);
        
        modelBuilder.Entity<Contact>()
            .Property(contact => contact.PhoneNumber)
            .HasMaxLength(20);
         
        // This should ideally sit in Fund Management API
        modelBuilder.Entity<Fund>()
            .HasIndex(contact => contact.ExternalId);

        // Seeding data for testing
        modelBuilder.Entity<Fund>()
            .HasData(
                new Fund
                {
                    Id = 1,
                    ExternalId = Guid.Parse("8c315b74-f063-48b0-a479-35763535075c"),
                    Name = "Global Growth Fund"
                },
                new Fund
                {
                    Id = 2,
                    ExternalId = Guid.Parse("9ecb2c37-9a59-44f7-9dc4-fcafa2d79389"),
                    Name = "Emerging Markets Fund"
                },
                new Fund
                {
                    Id =3,
                    ExternalId = Guid.Parse("6d4e7bf2-dd14-4032-9c72-7eceb0d663a4"),
                    Name = "Small Business Fund"
                });
    }
}
