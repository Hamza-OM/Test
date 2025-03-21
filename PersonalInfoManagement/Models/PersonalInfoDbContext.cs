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
            // Map to the "Person" table explicitly
            modelBuilder.Entity<Person>().ToTable("Person");
            
            // Configure PersonID property to not be database generated
            modelBuilder.Entity<Person>()
                .Property(p => p.PersonID)
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
        }
    }
} 