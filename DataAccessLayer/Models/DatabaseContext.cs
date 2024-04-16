using DataAccessLayer.Models.Implementations;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<AbstractMessenger> Messengers { get; set; }
    public DbSet<AbstractMessage> Messages { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AbstractEmployee> Employees { get; set; }
    public DbSet<EmployeeTask> EmployeeTasks { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<MessengerStatistic> MessengerStatistics { get; set; }
    public DbSet<EmployeeStatistic> EmployeeStatistics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EmployeeStatistic>().HasKey(x => x.EmployeeId);
        modelBuilder.Entity<MessengerStatistic>().HasKey(x => x.MessengerId);
        
        modelBuilder.Entity<AbstractMessenger>().HasDiscriminator<int>("Messenger")
            .HasValue<Email>(1)
            .HasValue<MobilePhone>(2);

        modelBuilder.Entity<AbstractEmployee>().HasDiscriminator<int>("Employee")
            .HasValue<Leader>(1)
            .HasValue<OrdinaryEmployee>(2);

        modelBuilder.Entity<AbstractMessage>().HasDiscriminator<int>("Message")
            .HasValue<EmailMessage>(1)
            .HasValue<PhoneMessage>(2);
    }
}