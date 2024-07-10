using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Filters.ActionFilters
{
    // Action filter to handle logic before and after executing actions for listing persons
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> _logger; // Logger to log information

        // Constructor to inject the logger dependency
        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            _logger = logger; // Assign the injected logger to a private field
        }

        // Method that gets executed after the action method
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Log the execution of the OnActionExecuted method
            _logger.LogInformation("{FilterName}.{MethodName} method", nameof(PersonsListActionFilter), nameof(OnActionExecuted));

            // Cast the context's controller to PersonsController
            PersonsController personsController = (PersonsController)context.Controller;

            // Retrieve action arguments stored in HttpContext.Items
            IDictionary<string, object?>? parameters = (IDictionary<string, object?>?)context.HttpContext.Items["arguments"];

            if (parameters != null)
            {
                // Set the ViewData with the current search criteria
                if (parameters.ContainsKey("searchBy"))
                {
                    personsController.ViewData["CurrentSearchBy"] = Convert.ToString(parameters["searchBy"]);
                }

                if (parameters.ContainsKey("searchString"))
                {
                    personsController.ViewData["CurrentSearchString"] = Convert.ToString(parameters["searchString"]);
                }

                // Set the sorting criteria, defaulting to PersonName if not provided
                if (parameters.ContainsKey("sortBy"))
                {
                    personsController.ViewData["CurrentSortBy"] = Convert.ToString(parameters["sortBy"]);
                }
                else
                {
                    personsController.ViewData["CurrentSortBy"] = nameof(PersonResponse.PersonName);
                }

                // Set the sorting order, defaulting to ascending if not provided
                if (parameters.ContainsKey("sortOrder"))
                {
                    personsController.ViewData["CurrentSortOrder"] = Convert.ToString(parameters["sortOrder"]);
                }
                else
                {
                    personsController.ViewData["CurrentSortOrder"] = nameof(SortOrderOptions.ASC);
                }
            }

            // Populate the ViewBag with search fields for the view
            personsController.ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.CountryID), "Country" },
                { nameof(PersonResponse.Address), "Address" }
            };
        }





        // Method that gets executed before the action method
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Store the action arguments in HttpContext.Items
            context.HttpContext.Items["arguments"] = context.ActionArguments;

            // Log the execution of the OnActionExecuting method
            _logger.LogInformation("{FilterName}.{MethodName} method", nameof(PersonsListActionFilter), nameof(OnActionExecuting));

            // Validate the searchBy parameter if it exists
            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);

                // Validate the searchBy parameter value
                if (!string.IsNullOrEmpty(searchBy))
                {
                    var searchByOptions = new List<string>()
                    {
                        nameof(PersonResponse.PersonName),
                        nameof(PersonResponse.Email),
                        nameof(PersonResponse.DateOfBirth),
                        nameof(PersonResponse.Gender),
                        nameof(PersonResponse.CountryID),
                        nameof(PersonResponse.Address)
                    };

                    // Reset the searchBy parameter value if it's not a valid option
                    if (searchByOptions.Any(temp => temp == searchBy) == false)
                    {
                        _logger.LogInformation("searchBy actual value {searchBy}", searchBy);
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                        _logger.LogInformation("searchBy updated value {searchBy}", context.ActionArguments["searchBy"]);
                    }
                }
            }
        }
    }
}
