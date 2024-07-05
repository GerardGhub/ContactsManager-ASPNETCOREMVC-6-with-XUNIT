﻿using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic (insert) for manipulating Person entity
    /// </summary>
    public interface IPersonsAdderService
 {
  /// <summary>
  /// Addds a new person into the list of persons
  /// </summary>
  /// <param name="personAddRequest">Person to add</param>
  /// <returns>Returns the same person details, along with newly generated PersonID</returns>
  Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

 }
}
