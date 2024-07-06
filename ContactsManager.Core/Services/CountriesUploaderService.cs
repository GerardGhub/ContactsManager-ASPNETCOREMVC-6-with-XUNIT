using Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using RepositoryContracts;
using ServiceContracts;

namespace Services
{
    public class CountriesUploaderService : ICountriesUploaderService
 {
        // Private field to hold the reference to the countries repository
        private readonly ICountriesRepository _countriesRepository;

      // Constructor that initializes the service with the countries repository dependency
   public CountriesUploaderService(ICountriesRepository countriesRepository)
  {
   _countriesRepository = countriesRepository;
  }

        // Method to upload countries from an Excel file
        // Returns the number of countries inserted into the repository
    public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
  {
            // Create a memory stream to read the file content
           MemoryStream memoryStream = new MemoryStream();
            // Copy the content of the uploaded file to the memory stream
            await formFile.CopyToAsync(memoryStream);
            // Initialize the counter for the number of countries inserted
            int countriesInserted = 0;

            // Use ExcelPackage to work with the Excel file in the memory stream
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
   {
                // Access the worksheet named "Countries"
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets["Countries"];
                // Get the total number of rows in the worksheet
                int rowCount = workSheet.Dimension.Rows;
                // Iterate through the rows, starting from the second row (excluding header)
                for (int row = 2; row <= rowCount; row++)
    {
                    // Read the value of the first cell in the current row
                    string? cellValue = Convert.ToString(workSheet.Cells[row, 1].Value);

                    // If the cell value is not null or empty
                    if (!string.IsNullOrEmpty(cellValue))
     {
                        // Assign the cell value to the country name
                        string? countryName = cellValue;

                        // Check if the country already exists in the repository
                        if (await _countriesRepository.GetCountryByCountryName(countryName) == null)
      {
                            // Create a new Country object with the extracted country name
                            Country country = new Country() { CountryName = countryName };
                            // Add the new country to the repository
                            await _countriesRepository.AddCountry(country);
                            // Increment the counter for the number of countries inserted
                            countriesInserted++;
      }
     }
    }
   }
            // Return the total number of countries inserted
            return countriesInserted;
  }
 }
}

