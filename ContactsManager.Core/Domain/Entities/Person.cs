using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Person domain model class
    /// Represents a person entity with various attributes such as name, email, date of birth, etc.
    /// </summary>
    public class Person
 {

        /// <summary>
        /// Gets or sets the unique identifier for the person.
        /// The [Key] attribute indicates that this property is the primary key in the database.
        /// </summary>

        [Key]
  public Guid PersonID { get; set; }

        /// <summary>
        /// Gets or sets the name of the person.
        /// The [StringLength(40)] attribute limits the length of the name to 40 characters.
        /// This property is nullable, indicating the person's name might not be set.
        /// </summary>

        [StringLength(40)] //nvarchar(40)
                     //[Required]
  public string? PersonName { get; set; }


        /// <summary>
        /// Gets or sets the email of the person.
        /// The [StringLength(40)] attribute limits the length of the email to 40 characters.
        /// This property is nullable, indicating the person's email might not be set.
        /// </summary>

        [StringLength(40)] //nvarchar(40)
  public string? Email { get; set; }


        /// <summary>
        /// Gets or sets the date of birth of the person.
        /// This property is nullable, indicating the person's date of birth might not be set.
        /// </summary>
        public DateTime? DateOfBirth { get; set; }


        /// <summary>
        /// Gets or sets the gender of the person.
        /// The [StringLength(10)] attribute limits the length of the gender to 10 characters.
        /// This property is nullable, indicating the person's gender might not be set.
        /// </summary>

        [StringLength(10)] //nvarchar(100)
  public string? Gender { get; set; }


        /// <summary>
        /// Gets or sets the unique identifier for the country associated with the person.
        /// This property is nullable, indicating the person's country might not be set.
        /// </summary>

        //uniqueidentifier
        public Guid? CountryID { get; set; }


        /// <summary>
        /// Gets or sets the address of the person.
        /// The [StringLength(200)] attribute limits the length of the address to 200 characters.
        /// This property is nullable, indicating the person's address might not be set.
        /// </summary>

        [StringLength(200)] //nvarchar(200)
  public string? Address { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether the person wants to receive newsletters.
        /// The default value is false.
        /// </summary>
        //bit
        public bool ReceiveNewsLetters { get; set; }

        /// <summary>
        /// Gets or sets the tax identification number (TIN) of the person.
        /// This property was previously configured to be stored as a varchar(8) in the database.
        /// This property is nullable, indicating the person's TIN might not be set.
        /// </summary>

        //[Column("TaxIdentificationNumber", TypeName = "varchar(8)")]
        public string? TIN { get; set; }

        /// <summary>
        /// Gets or sets the country associated with the person.
        /// The [ForeignKey("CountryID")] attribute indicates that this property is a foreign key relationship.
        /// This property is nullable, indicating the person might not have an associated country.
        /// </summary>


        [ForeignKey("CountryID")]
  public virtual Country? Country { get; set; }

        /// <summary>
        /// Returns a string representation of the person entity.
        /// </summary>
        /// <returns>A string that represents the person entity.</returns>

        public override string ToString()
  {
   return $"Person ID: {PersonID}, Person Name: {PersonName}, Email: {Email}, Date of Birth: {DateOfBirth?.ToString("MM/dd/yyyy")}, Gender: {Gender}, Country ID: {CountryID}, Country: {Country?.CountryName}, Address: {Address}, Receive News Letters: {ReceiveNewsLetters}";
  }
 }
}
