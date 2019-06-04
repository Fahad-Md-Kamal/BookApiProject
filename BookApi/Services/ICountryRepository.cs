using System.Collections.Generic;
using BookApi.Models;

namespace BookApi.Services
{
    public interface ICountryRepository
    {
         bool CountryExists(int CountryId);
         ICollection<Country> GetCountries();
         Country GetCountry(int Id);
         Country GetCountryOfAnAuthor(int AuthorId);
         ICollection<Author> GetAuthorsFromACountry(int CountryId);
         bool IsDuplicateCountry(int CountryId, string CountryName);

         bool CreateCountry(Country county);
         bool UpdateCountry(Country county);
         bool DeleteCountry(Country county);
         bool Save();
    }
}