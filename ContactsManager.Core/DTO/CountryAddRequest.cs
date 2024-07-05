using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class for adding a new country
    /// </summary>
    public class CountryAddRequest
  {
        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>

        public string? CountryName { get; set; }

        /// <summary>
        /// Converts the current DTO to a Country domain model.
        /// </summary>
        /// <returns>A new Country object with the properties from the DTO.</returns>

        public Country ToCountry()
    {
      return new Country() { CountryName = CountryName };
    }
  }
}

