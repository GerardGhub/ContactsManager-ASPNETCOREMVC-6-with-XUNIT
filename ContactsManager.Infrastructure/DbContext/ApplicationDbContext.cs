using ContactsManager.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    // DbContext for the application, extending IdentityDbContext for Identity functionality
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
 {
        // Constructor accepting DbContextOptions to configure the context
   public ApplicationDbContext(DbContextOptions options) : base(options)
  {
  }

        // DbSets representing tables in the database
  public virtual DbSet<Country> Countries { get; set; }
  public virtual DbSet<Person> Persons { get; set; }

        // Method to configure the model (tables, relationships, constraints, etc.
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
   base.OnModelCreating(modelBuilder);
    
    // Configure the Country and Person entities to use specific table names
   modelBuilder.Entity<Country>().ToTable("Countries");
   modelBuilder.Entity<Person>().ToTable("Persons");

    // Seed the Countries table with data from a JSON file
   string countriesJson = System.IO.File.ReadAllText("countries.json");
   List<Country> countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

   foreach (Country country in countries)
    modelBuilder.Entity<Country>().HasData(country);


            // Seed the Persons table with data from a JSON file
   string personsJson = System.IO.File.ReadAllText("persons.json");
   List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

   foreach (Person person in persons)
    modelBuilder.Entity<Person>().HasData(person);


            // Fluent API configurations for the Person entity
      modelBuilder.Entity<Person>().Property(temp => temp.TIN)
     .HasColumnName("TaxIdentificationNumber")
     .HasColumnType("varchar(8)")
     .HasDefaultValue("ABC12345");

            //modelBuilder.Entity<Person>()
            //  .HasIndex(temp => temp.TIN).IsUnique();

            // Adding a check constraint to the TIN column to ensure it has exactly 8 characters
      modelBuilder.Entity<Person>()
     .HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8");

            //Table Relations
            // Configure the relationship between Person and Country entities
    modelBuilder.Entity<Person>(entity =>
   {
    entity.HasOne<Country>(c => c.Country)
       .WithMany(p => p.Persons)
       .HasForeignKey(p => p.CountryID);
   });
  }

        // Method to call a stored procedure that retrieves all persons

    public List<Person> sp_GetAllPersons()
  {
   return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
  }

        // Method to call a stored procedure that inserts a new person

      public int sp_InsertPerson(Person person)
  {
   SqlParameter[] parameters = new SqlParameter[] {
        new SqlParameter("@PersonID", person.PersonID),
        new SqlParameter("@PersonName", person.PersonName),
        new SqlParameter("@Email", person.Email),
        new SqlParameter("@DateOfBirth", person.DateOfBirth),
        new SqlParameter("@Gender", person.Gender),
        new SqlParameter("@CountryID", person.CountryID),
        new SqlParameter("@Address", person.Address),
        new SqlParameter("@ReceiveNewsLetters", person.ReceiveNewsLetters)
      };

            // Execute the stored procedure with the provided parameters

        return Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetters", parameters);
  }
 }
}
