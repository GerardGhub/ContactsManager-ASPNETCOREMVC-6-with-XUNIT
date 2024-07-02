using Microsoft.AspNetCore.Identity;

namespace ContactsManager.Core.Domain.IdentityEntities
{
    /// <summary>
    /// Represents an application-specific user that extends the IdentityUser class with a GUID as the primary key.
    /// </summary>

    public class ApplicationUser : IdentityUser<Guid>
 {
        /// <summary>
        /// Gets or sets the name of the person associated with the user.
        /// </summary>

        public string? PersonName { get; set; }
 }
}
