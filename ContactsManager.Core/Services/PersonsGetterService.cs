// Using statements to import necessary namespaces and libraries
// Entities: Contains the Person entity and related classes
using Entities;
// ServiceContracts: Contains the interfaces for services
using ServiceContracts.DTO;
using ServiceContracts;
// CsvHelper: Library to handle CSV file operations
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
// OfficeOpenXml: Library to handle Excel file operations
using OfficeOpenXml;
// RepositoryContracts: Contains the interfaces for repositories
using RepositoryContracts;
// Microsoft.Extensions.Logging: Provides logging functionalities
using Microsoft.Extensions.Logging;
// Serilog: Logging library for structured logging
using Serilog;
// SerilogTimings: Provides timing functionalities for logging
using SerilogTimings;

namespace Services
{
    public class PersonsGetterService : IPersonsGetterService
 {
        // Private fields to hold references to the persons repository, logger, and diagnostic context
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        // Constructor to initialize the service with dependencies
        public PersonsGetterService(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext)
       {
        _personsRepository = personsRepository;
        _logger = logger;
        _diagnosticContext = diagnosticContext;
        }

        // Method to get all persons
        // Returns a list of PersonResponse objects
        public virtual async Task<List<PersonResponse>> GetAllPersons()
       {
        _logger.LogInformation("GetAllPersons of PersonsService");

            // Retrieve all persons from the repository
            var persons = await _personsRepository.GetAllPersons();

            // Convert each Person entity to a PersonResponse DTO and return as a list
            return persons.Select(temp => temp.ToPersonResponse()).ToList();
  }


        // Method to get a person by their ID
        // Takes a nullable Guid representing the person's ID and returns a PersonResponse object

        public virtual async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
       {
        if (personID == null)
           return null;

            // Retrieve the person entity from the repository by their ID
            Person? person = await _personsRepository.GetPersonByPersonID(personID.Value);

       if (person == null)
           return null;

            // Convert the Person entity to a PersonResponse DTO
            return person.ToPersonResponse();
  }


        // Method to get filtered persons based on search criteria
        // Takes the search field and search string, returns a list of PersonResponse objects
   public virtual async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
  {
   _logger.LogInformation("GetFilteredPersons of PersonsService");

   List<Person> persons;

    // Measure the time taken for the filtering operation using SerilogTimings
    using (Operation.Time("Time for Filtered Persons from Database"))
   {
                // Use switch expression to filter persons based on the search criteria
     persons = searchBy switch
    {
     nameof(PersonResponse.PersonName) =>
      await _personsRepository.GetFilteredPersons(temp =>
      temp.PersonName.Contains(searchString)),

     nameof(PersonResponse.Email) =>
      await _personsRepository.GetFilteredPersons(temp =>
      temp.Email.Contains(searchString)),

     nameof(PersonResponse.DateOfBirth) =>
      await _personsRepository.GetFilteredPersons(temp =>
      temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString)),


     nameof(PersonResponse.Gender) =>
      await _personsRepository.GetFilteredPersons(temp =>
      temp.Gender.Contains(searchString)),

     nameof(PersonResponse.CountryID) =>
      await _personsRepository.GetFilteredPersons(temp =>
      temp.Country.CountryName.Contains(searchString)),

     nameof(PersonResponse.Address) =>
     await _personsRepository.GetFilteredPersons(temp =>
     temp.Address.Contains(searchString)),

     _ => await _personsRepository.GetAllPersons()
    };
   } //end of "using block" of serilog timings

            // Set diagnostic context with the persons data
            _diagnosticContext.Set("Persons", persons);

            // Convert each Person entity to a PersonResponse DTO and return as a list
            return persons.Select(temp => temp.ToPersonResponse()).ToList();
  }


        // Method to get all persons in CSV format
        // Returns a MemoryStream containing the CSV data
   public virtual async Task<MemoryStream> GetPersonsCSV()
  {
   MemoryStream memoryStream = new MemoryStream();
   StreamWriter streamWriter = new StreamWriter(memoryStream);

            // Configure the CSV writer with the appropriate culture information
   CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
   CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);

            // Write the CSV header fields
            //PersonName,Email,DateOfBirth,Age,Gender,Country,Address,ReceiveNewsLetters
   csvWriter.WriteField(nameof(PersonResponse.PersonName));
   csvWriter.WriteField(nameof(PersonResponse.Email));
   csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
   csvWriter.WriteField(nameof(PersonResponse.Age));
   csvWriter.WriteField(nameof(PersonResponse.Country));
   csvWriter.WriteField(nameof(PersonResponse.Address));
   csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));
   csvWriter.NextRecord();

            // Retrieve all persons and write each person to the CSV
   List<PersonResponse> persons = await GetAllPersons();

   foreach (PersonResponse person in persons)
   {
    csvWriter.WriteField(person.PersonName);
    csvWriter.WriteField(person.Email);
    if (person.DateOfBirth.HasValue)
     csvWriter.WriteField(person.DateOfBirth.Value.ToString("yyyy-MM-dd"));
    else
     csvWriter.WriteField("");
    csvWriter.WriteField(person.Age);
    csvWriter.WriteField(person.Country);
    csvWriter.WriteField(person.Address);
    csvWriter.WriteField(person.ReceiveNewsLetters);
    csvWriter.NextRecord();
    csvWriter.Flush();
   }

            // Reset the memory stream position to the beginning
            memoryStream.Position = 0;
   return memoryStream;
  }


        // Method to get all persons in Excel format
        // Returns a MemoryStream containing the Excel data
   public virtual async Task<MemoryStream> GetPersonsExcel()
  {
   MemoryStream memoryStream = new MemoryStream();
   using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
   {
                // Create a worksheet and set the header values
    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
    workSheet.Cells["A1"].Value = "Person Name";
    workSheet.Cells["B1"].Value = "Email";
    workSheet.Cells["C1"].Value = "Date of Birth";
    workSheet.Cells["D1"].Value = "Age";
    workSheet.Cells["E1"].Value = "Gender";
    workSheet.Cells["F1"].Value = "Country";
    workSheet.Cells["G1"].Value = "Address";
    workSheet.Cells["H1"].Value = "Receive News Letters";

                // Style the header
     using (ExcelRange headerCells = workSheet.Cells["A1:H1"])
    {
     headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
     headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
     headerCells.Style.Font.Bold = true;
    }

                // Retrieve all persons and write each person to the worksheet
    int row = 2;
    List<PersonResponse> persons = await GetAllPersons();

    foreach (PersonResponse person in persons)
    {
     workSheet.Cells[row, 1].Value = person.PersonName;
     workSheet.Cells[row, 2].Value = person.Email;
     if (person.DateOfBirth.HasValue)
      workSheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString("yyyy-MM-dd");
     workSheet.Cells[row, 4].Value = person.Age;
     workSheet.Cells[row, 5].Value = person.Gender;
     workSheet.Cells[row, 6].Value = person.Country;
     workSheet.Cells[row, 7].Value = person.Address;
     workSheet.Cells[row, 8].Value = person.ReceiveNewsLetters;

     row++;
    }

                // Autofit the columns to the content
                workSheet.Cells[$"A1:H{row}"].AutoFitColumns();

                // Save the Excel package
                await excelPackage.SaveAsync();
   }

            // Reset the memory stream position to the beginning
            memoryStream.Position = 0;
   return memoryStream;
  }
 }
}
