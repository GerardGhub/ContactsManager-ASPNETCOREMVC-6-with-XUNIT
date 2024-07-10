using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;

namespace CRUDExample.Filters.ActionFilters
{
    // Action filter to handle logic before and after executing actions for creating and editing persons
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesGetterService _countriesService; // Service to fetch country data
        private readonly ILogger<PersonCreateAndEditPostActionFilter> _logger; // Logger to log information

        // Constructor to inject dependencies
        public PersonCreateAndEditPostActionFilter(ICountriesGetterService countriesService, ILogger<PersonCreateAndEditPostActionFilter> logger)
        {
            _countriesService = countriesService; // Assign the injected countries service to a private field
            _logger = logger; // Assign the injected logger to a private field
        }

        // Method that gets executed before and after the action method
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // TODO: Add any logic that needs to run before the action method execution

            // Check if the controller is of type PersonsController
            if (context.Controller is PersonsController personsController)
            {
                // Check if the model state is invalid
                if (!personsController.ModelState.IsValid)
                {
                    // Fetch the list of all countries
                    List<CountryResponse> countries = await _countriesService.GetAllCountries();

                    // Populate the ViewBag with countries data for dropdown selection
                    personsController.ViewBag.Countries = countries.Select(temp =>
                        new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

                    // Populate the ViewBag with error messages from the model state
                    personsController.ViewBag.Errors = personsController.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    // Retrieve the person request object from the action arguments
                    var personRequest = context.ActionArguments["personRequest"];

                    // Set the result to the view with the person request to display validation errors and skip further action filters
                    context.Result = personsController.View(personRequest);
                }
                else
                {
                    // If the model state is valid, invoke the next filter or action method
                    await next();
                }
            }
            else
            {
                // If the controller is not PersonsController, invoke the next filter or action method
                await next();
            }

            // TODO: Add any logic that needs to run after the action method execution
            _logger.LogInformation("In after logic of PersonsCreateAndEdit Action filter");
        }
    }
}
