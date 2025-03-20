using System.Data.Entity;

namespace PersonalInfoManagement.Models
{
    public class PersonalInfoDbContext : DbContext
    {
        public PersonalInfoDbContext() : base("name=PersonalInfoDbContext")
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
} 