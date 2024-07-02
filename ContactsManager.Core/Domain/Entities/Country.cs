using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Domain Model for Country
    /// Represents a country entity with a unique identifier, name, and related persons.
    /// </summary>
    public class Country
  {
        /// <summary>
        /// Gets or sets the unique identifier for the country.
        /// The [Key] attribute indicates that this property is the primary key in the database.
        /// </summary>


        [Key]
    public Guid CountryID { get; set; }

        /// <summary>
        /// Gets or sets the name of the country.
        /// This property is nullable, meaning the country might not have a name assigned.
        /// </summary>

        public string? CountryName { get; set; }

        /// <summary>
        /// Gets or sets the collection of persons related to this country.
        /// The 'virtual' keyword enables lazy loading of this collection if supported by the ORM (e.g., Entity Framework).
        /// This property is nullable, meaning a country might not have any associated persons.
        /// </summary>

        public virtual ICollection<Person>? Persons { get; set; }
  }
}
