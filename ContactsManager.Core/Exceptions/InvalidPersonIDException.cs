// Define the namespace for custom exceptions

namespace Exceptions
{

    // Define a custom exception class for invalid person IDs that inherits from ArgumentException

    public class InvalidPersonIDException : ArgumentException
 {

        // Default constructor that calls the base class (ArgumentException) constructor
   public InvalidPersonIDException() : base()
  {
  }

        // Constructor that accepts a custom error message and passes it to the base class constructor
  public InvalidPersonIDException(string? message) : base(message)
  {
  }

        // Constructor that accepts a custom error message and an inner exception,
        // then passes these to the base class constructor

        public InvalidPersonIDException(string? message, Exception? innerException)
  {
  }
 }
}