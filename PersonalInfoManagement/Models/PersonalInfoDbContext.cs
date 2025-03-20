using System.Data.Entity;

namespace PersonalInfoManagement.Models
{
    public class PersonalInfoDbContext : DbContext
    {
        public PersonalInfoDbContext() : base("name=PersonalInfoDbContext")
        {
            // Disable database initialization to ensure it uses the existing database
            Database.SetInitializer<PersonalInfoDbContext>(null);
        }

        public DbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("Person");
        }
    }
} 