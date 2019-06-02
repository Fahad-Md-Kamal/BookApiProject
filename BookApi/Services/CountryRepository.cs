using System.Collections.Generic;
using System.Linq;
using BookApi.Models;

namespace BookApi.Services
{
    public class CountryRepository : ICountryRepository
    {
        private readonly BookDbContext _Context;
        public CountryRepository(BookDbContext Context)
        {
            _Context = Context;

        }


        public bool CountryExists(int CountryId)
        {
            return _Context.Countries.Any(c => c.Id == CountryId);
        }
        
        public Country GetCountry(int Id)
        {
            return _Context.Countries.Where(c => c.Id == Id).FirstOrDefault();
        }

        public ICollection<Country> GetCountries()
        {
            return _Context.Countries.OrderBy(c => c.Name).ToList();
        }


        public Country GetCountryOfAnAuthor(int AuthorId)
        {
            return _Context.Authors.Where(a => a.Id == AuthorId).Select(c => c.Country).FirstOrDefault();
        }

        
        public ICollection<Author> GetAuthorsFromACountry(int CountryId)
        {
            return _Context.Authors.Where(c => c.Country.Id == CountryId).ToList();
        }

    }
}