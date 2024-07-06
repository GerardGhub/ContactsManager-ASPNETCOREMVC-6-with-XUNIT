using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using ServiceContracts.DTO;

namespace Services
{
    // PersonsGetterServiceChild class inherits from PersonsGetterService
    public class PersonsGetterServiceChild : PersonsGetterService
 {

        // Constructor to initialize the service with dependencies
        // It calls the base class constructor with the provided parameters

    public PersonsGetterServiceChild(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext) : base(personsRepository, logger, diagnosticContext)
  {
  }


        // Overriding the GetPersonsExcel method to customize the Excel export functionality
   public async override Task<MemoryStream> GetPersonsExcel()
  {
            // MemoryStream to hold the Excel data
   MemoryStream memoryStream = new MemoryStream();
   using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
   {
                // Adding a worksheet to the Excel package and setting header values
    ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
    workSheet.Cells["A1"].Value = "Person Name";
    workSheet.Cells["B1"].Value = "Age";
    workSheet.Cells["C1"].Value = "Gender";

                // Styling the header
     using (ExcelRange headerCells = workSheet.Cells["A1:C1"])
    {
     headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
     headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
     headerCells.Style.Font.Bold = true;
    }

    int row = 2;
                // Retrieve all persons and write each person to the worksheet
                List<PersonResponse> persons = await GetAllPersons();

                // Throw an exception if no persons data is found
                if (persons.Count == 0)
    {
     throw new InvalidOperationException("No persons data");
    }

                // Loop through each person and write their details to the worksheet
     foreach (PersonResponse person in persons)
    {
     workSheet.Cells[row, 1].Value = person.PersonName;
     workSheet.Cells[row, 2].Value = person.Age;
     workSheet.Cells[row, 3].Value = person.Gender;

     row++;
    }

                // Autofit the columns to the content
  workSheet.Cells[$"A1:C{row}"].AutoFitColumns();

                // Save the Excel package
                await excelPackage.SaveAsync();
   }

            // Reset the memory stream position to the beginning
            memoryStream.Position = 0;
   return memoryStream;
  }
 }
}
