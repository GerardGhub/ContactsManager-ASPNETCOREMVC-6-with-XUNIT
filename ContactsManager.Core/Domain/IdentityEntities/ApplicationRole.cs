using Microsoft.AspNetCore.Identity;

namespace ContactsManager.Core.Domain.IdentityEntities
{
    /// <summary>
    /// Represents an application-specific role that extends the IdentityRole class with a GUID as the primary key.
    /// </summary>

    public class ApplicationRole : IdentityRole<Guid>
 {
        // This class inherits all functionality from IdentityRole<Guid>.
        // Additional properties and methods specific to your application's roles can be added here.
    }
}
