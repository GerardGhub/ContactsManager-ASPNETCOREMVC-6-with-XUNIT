using OfficeOpenXml;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    /// <summary>
    /// Service class extending PersonsGetterService to provide Excel export functionality
    /// with limited fields (Person Name, Age, Gender).
    /// </summary>

    public class PersonsGetterServiceWithFewExcelFields : IPersonsGetterService
 {
  private readonly PersonsGetterService _personGetterService; // Instance of PersonsGetterService for delegated operations

        /// <summary>
        /// Constructor to initialize PersonsGetterServiceWithFewExcelFields with PersonsGetterService dependency.
        /// </summary>
        /// <param name="personsGetterService">Instance of PersonsGetterService for delegated operations.</param>

    public PersonsGetterServiceWithFewExcelFields(PersonsGetterService personsGetterService)
  {
   _personGetterService = personsGetterService;
  }

    /// <inheritdoc/>
  public async Task<List<PersonResponse>> GetAllPersons()
  {
   return await _personGetterService.GetAllPersons();
  }

    /// <inheritdoc/>
  public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
  {
   return await _personGetterService.GetFilteredPersons(searchBy, searchString);
  }

        /// <inheritdoc/>
        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
  {
   return await _personGetterService.GetPersonByPersonID(personID);
  }

        /// <inheritdoc/>
        public async Task<MemoryStream> GetPersonsCSV()
  {
   return await _personGetterService.GetPersonsCSV();
  }

        /// <summary>
        /// Generates an Excel file containing Person Name, Age, and Gender fields.
        /// </summary>
        /// <returns>MemoryStream containing the Excel file data.</returns>

  public async Task<MemoryStream> GetPersonsExcel()
  {
   MemoryStream memoryStream = new MemoryStream();
            // Create Excel package and worksheet
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
   {
    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
                // Set headers for Person Name, Age, and Gender
                workSheet.Cells["A1"].Value = "Person Name";
    workSheet.Cells["B1"].Value = "Age";
    workSheet.Cells["C1"].Value = "Gender";

                // Style headers
                using (ExcelRange headerCells = workSheet.Cells["A1:C1"])
    {
     headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
     headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
     headerCells.Style.Font.Bold = true;
    }

    int row = 2;

                //Fetch all Persons
    List<PersonResponse> persons = await GetAllPersons();
                // Populate Excel sheet with Person Name, Age, and Gender data
                foreach (PersonResponse person in persons)
    {
     workSheet.Cells[row, 1].Value = person.PersonName;
     workSheet.Cells[row, 2].Value = person.Age;
     workSheet.Cells[row, 3].Value = person.Gender;

     row++;
    }
                // Autofit columns for better display
                workSheet.Cells[$"A1:C{row}"].AutoFitColumns();

                // Save Excel package to MemoryStream and return it
                await excelPackage.SaveAsync();
   }
            // Reset MemoryStream position to start
            memoryStream.Position = 0;
   return memoryStream;
  }
 }
}
