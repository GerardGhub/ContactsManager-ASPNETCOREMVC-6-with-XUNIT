using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using Services.Helpers;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Services
{
    public class PersonsAdderService : IPersonsAdderService
 {
        // Private fields to hold references to the persons repository, logger, and diagnostic context
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonsGetterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        // Constructor to initialize the service with dependencies
        public PersonsAdderService(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext)
       {
         _personsRepository = personsRepository;
         _logger = logger;
         _diagnosticContext = diagnosticContext;
        }

        // Method to add a new person
        // Takes a PersonAddRequest object and returns a PersonResponse object
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            // Check if the personAddRequest is null
            // If it is null, throw an ArgumentNullException
            if (personAddRequest == null)
            {
             throw new ArgumentNullException(nameof(personAddRequest));
            }

            // Validate the personAddRequest object
            // This ensures that the data conforms to the expected model
            ValidationHelper.ModelValidation(personAddRequest);

            // Convert the personAddRequest DTO into a Person entity
            Person person = personAddRequest.ToPerson();

            // Generate a new unique identifier for the person
            person.PersonID = Guid.NewGuid();

            // Add the person entity to the repository
            await _personsRepository.AddPerson(person);

            // Convert the Person entity into a PersonResponse DTO
            return person.ToPersonResponse();
  }

 }
}
