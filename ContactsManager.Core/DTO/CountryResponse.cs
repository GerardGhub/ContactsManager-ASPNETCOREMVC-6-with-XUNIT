using Entities; // Assuming Entities namespace contains the Country class

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Data Transfer Object (DTO) representing country information returned from CountryService methods.
    /// </summary>
    public class CountryResponse
  {
        /// <summary>
        /// Unique identifier for the country.
        /// </summary>

        public Guid CountryID { get; set; }

        /// <summary>
        /// Name of the country.
        /// </summary>
        public string? CountryName { get; set; }


        /// <summary>
        /// Compares the current CountryResponse object with another object to determine equality based on CountryID and CountryName.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>True if both objects are equal; otherwise, false.</returns>

        public override bool Equals(object? obj)
    {
                   // Check if the object is null or of a different type
      if (obj == null)
      {
        return false;
      }

      if (obj.GetType() != typeof(CountryResponse))
      {
        return false;
      }
      CountryResponse country_to_compare = (CountryResponse)obj;

            // Compare CountryID and CountryName for equality
            return CountryID == country_to_compare.CountryID && CountryName == country_to_compare.CountryName;
    }

        /// <summary>
        /// Computes a hash code for the current CountryResponse object.
        /// </summary>
        /// <returns>A hash code based on CountryID and CountryName.</returns>
        public override int GetHashCode()
    {
      return base.GetHashCode();
    }
  }

    /// <summary>
    /// Static class containing extension methods for converting between Entity.Country and DTO.CountryResponse objects.
    /// </summary>

    public static class CountryExtensions
  {
        /// <summary>
        /// Converts a Country object to a corresponding CountryResponse object.
        /// </summary>
        /// <param name="country">The Country object to convert.</param>
        /// <returns>A new instance of CountryResponse initialized with the properties of the input Country object.</returns>
        public static CountryResponse ToCountryResponse(this Country country)
    {
      return new CountryResponse() {  CountryID = country.CountryID, CountryName = country.CountryName };
    }
  }
}
