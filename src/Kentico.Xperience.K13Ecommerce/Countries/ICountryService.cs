using CMS.Globalization;

namespace Kentico.Xperience.K13Ecommerce.Countries;

/// <summary>
/// Represents a service for managing countries and states.
/// </summary>
public interface ICountryService
{
    /// <summary>
    /// Returns all available countries.
    /// </summary>
    /// <returns>Collection of all available countries</returns>
    Task<IEnumerable<CountryInfo>> GetAllCountries();


    /// <summary>
    /// Returns the country with the specified ID.
    /// </summary>
    /// <param name="countryId">The identifier of the country.</param>
    /// <returns>The country with the specified ID, if found; otherwise, null.</returns>
    Task<CountryInfo> GetCountry(int countryId);


    /// <summary>
    /// Returns the country with the specified code name.
    /// </summary>
    /// <param name="countryName">The code name of the country.</param>
    /// <returns>The country with the specified code name, if found; otherwise, null.</returns>
    Task<CountryInfo> GetCountry(string countryName);


    /// <summary>
    /// Returns all states in country with given ID.
    /// </summary>
    /// <param name="countryId">Country identifier</param>
    /// <returns>Collection of all states in county.</returns>
    Task<IEnumerable<StateInfo>> GetCountryStates(int countryId);


    /// <summary>
    /// Returns the state with the specified code name.
    /// </summary>
    /// <param name="stateName">The code name of the state.</param>
    /// <returns>The state with the specified code name, if found; otherwise, null.</returns>
    Task<StateInfo> GetState(string stateName);


    /// <summary>
    /// Returns the state with the specified ID.
    /// </summary>
    /// <param name="stateId">The identifier of the state.</param>
    /// <returns>The state with the specified ID, if found; otherwise, null.</returns>
    Task<StateInfo> GetState(int stateId);
}
