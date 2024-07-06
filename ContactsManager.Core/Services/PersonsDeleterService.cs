using Entities;
using ServiceContracts;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Services
{
    public class PersonsDeleterService : IPersonsDeleterService
 {
        // Private fields to hold references to the persons repository, logger, and diagnostic context
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        // Constructor to initialize the service with dependencies
        public PersonsDeleterService(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext)
        {
         _personsRepository = personsRepository;
         _logger = logger;
         _diagnosticContext = diagnosticContext;
        }


        // Method to delete a person by their ID
        // Takes a nullable Guid representing the person's ID and returns a boolean indicating success
        public async Task<bool> DeletePerson(Guid? personID)
       {
            // Check if the personID is null
            // If it is null, throw an ArgumentNullException
            if (personID == null)
           {
           throw new ArgumentNullException(nameof(personID));
           }

            // Retrieve the person entity from the repository by their ID
            Person? person = await _personsRepository.GetPersonByPersonID(personID.Value);

            // If the person does not exist, return false indicating failure
            if (person == null)
            return false;

            // Delete the person entity from the repository by their ID
            await _personsRepository.DeletePersonByPersonID(personID.Value);

            // Return true indicating the person was successfully deleted
            return true;
  }

 }
}
