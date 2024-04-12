using CMS.Globalization;
using CMS.Helpers;

namespace Kentico.Xperience.K13Ecommerce.Countries;

/// <summary>
/// Represents a collection of countries and states.
/// </summary>
internal class CountryService : ICountryService
{
    private readonly ICountryInfoProvider countryInfoProvider;
    private readonly IStateInfoProvider stateInfoProvider;
    private readonly IProgressiveCache cache;


    /// <summary>
    /// Initializes a new instance of the <see cref="CountryService"/> class.
    /// </summary>
    /// <param name="countryInfoProvider">Provider for <see cref="CountryInfo"/> management.</param>
    /// <param name="stateInfoProvider">Provider for <see cref="StateInfo"/> management.</param>
    /// <param name="cache"></param>
    public CountryService(ICountryInfoProvider countryInfoProvider, IStateInfoProvider stateInfoProvider,
        IProgressiveCache cache)
    {
        this.countryInfoProvider = countryInfoProvider;
        this.stateInfoProvider = stateInfoProvider;
        this.cache = cache;
    }


    /// <summary>
    /// Returns all available countries.
    /// </summary>
    /// <returns>Collection of all available countries</returns>
    public async Task<IEnumerable<CountryInfo>> GetAllCountries()
    {
        return await CacheObject(
            async () => (await countryInfoProvider.Get().GetEnumerableTypedResultAsync()).ToList(),
            $"{nameof(CountryService)}|{nameof(GetAllCountries)}");
    }


    /// <summary>
    /// Returns the country with the specified ID.
    /// </summary>
    /// <param name="countryId">The identifier of the country.</param>
    /// <returns>The country with the specified ID, if found; otherwise, null.</returns>
    public async Task<CountryInfo> GetCountry(int countryId)
    {
        return await CacheObject(async () => await countryInfoProvider.GetAsync(countryId),
            $"{nameof(CountryService)}|{nameof(GetCountry)}|{countryId}");
    }


    /// <summary>
    /// Returns the country with the specified code name.
    /// </summary>
    /// <param name="countryName">The code name of the country.</param>
    /// <returns>The country with the specified code name, if found; otherwise, null.</returns>
    public async Task<CountryInfo> GetCountry(string countryName)
    {
        return await CacheObject(async () => await countryInfoProvider.GetAsync(countryName),
            $"{nameof(CountryService)}|{nameof(GetCountry)}|{countryName}");
    }


    /// <summary>
    /// Returns all states in country with given ID.
    /// </summary>
    /// <param name="countryId">Country identifier</param>
    /// <returns>Collection of all states in county.</returns>
    public async Task<IEnumerable<StateInfo>> GetCountryStates(int countryId)
    {
        return await CacheObject(
            async () => (await stateInfoProvider.Get().WhereEquals("CountryID", countryId)
                .GetEnumerableTypedResultAsync()).ToList(),
            $"{nameof(CountryService)}|{nameof(GetCountryStates)}|{countryId}");
    }


    /// <summary>
    /// Returns the state with the specified code name.
    /// </summary>
    /// <param name="stateName">The code name of the state.</param>
    /// <returns>The state with the specified code name, if found; otherwise, null.</returns>
    public async Task<StateInfo> GetState(string stateName)
    {
        return await CacheObject(async () => await stateInfoProvider.GetAsync(stateName),
            $"{nameof(CountryService)}|{nameof(GetState)}|{stateName}");
    }


    /// <summary>
    /// Returns the state with the specified ID.
    /// </summary>
    /// <param name="stateId">The identifier of the state.</param>
    /// <returns>The state with the specified ID, if found; otherwise, null.</returns>
    public async Task<StateInfo> GetState(int stateId)
    {
        return await CacheObject(async () => await stateInfoProvider.GetAsync(stateId),
            $"{nameof(CountryService)}|{nameof(GetState)}|{stateId}");
    }

    private async Task<TData> CacheObject<TData>(Func<Task<TData>> func, string cacheKey)
    {
        var cacheSettings = new CacheSettings(20, cacheKey);
        return await cache.LoadAsync(async _ => await func(), cacheSettings);
    }
}
