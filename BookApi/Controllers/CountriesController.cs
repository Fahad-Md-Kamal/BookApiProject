using System.Collections.Generic;
using System.Linq;
using BookApi.Dtos;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{

    // This will be always diduct controller word from the full controller name 
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IAuthorRepository _AuthorRepository;
        public CountriesController(ICountryRepository countryRepository, IAuthorRepository AuthorRepository)
        {
            _AuthorRepository = AuthorRepository;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
        public IActionResult GetCountries()
        {
            // Will first check if the request is valid
            var countries = _countryRepository.GetCountries();

            // If there is not country it will return null result 
            // and the execution will stop showing the result.
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // If there are countries it will map the list with Countries View data
            var countriesDto = new List<CountryDto>();
            // Maps all the items of the list with the ViewModel as Data Transferring Object or DTO
            foreach (var country in countries)
            {
                countriesDto.Add(new CountryDto
                {
                    Id = country.Id,
                    Name = country.Name
                });
            }

            return Ok(countriesDto);
        }


        // api/countries/countryId
        [HttpGet("{CountryId}")]
        // It will return only the following request. Except these it will be regarded as and error.
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountry(int countryId)
        {
            // Will first check if a country does exists ?
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            // If a country exists it will assign it to the variable for further processes.
            var country = _countryRepository.GetCountry(countryId);

            // If a requst is invalid this will be executed.
            // Suppose instade of intager id string is inserted
            // This will show a bad request notification
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // If there is a country it will map the object with the ViewModel
            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };

            // After successful execution it will show an Ok messaage with the result.
            return Ok(countryDto);
        }

//TODO - Test This
        // api/countries/authors/authorId
        [HttpGet("authors/{authorId}")]
        // It will return only the following request. Except these it will be regarded as and error.
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountryOfAnAuthor(int authorId)
        {
            if(!_AuthorRepository.AuthorExists(authorId))
                return NotFound();

            // Will check if any author has a country using the authorId
            var country = _countryRepository.GetCountryOfAnAuthor(authorId);

            // If the URL is not valid will generate a bad request message.
            if (!ModelState.IsValid)
                return BadRequest(authorId);

            // If there is a country it will map the result with the view model
            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };

            return Ok(countryDto);

        }



        // api/countries/countryId/Authors
        [HttpGet("{countryId}/authors")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetAuthorsFromACountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            var authors = _countryRepository.GetAuthorsFromACountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorsDto = new List<AuthorDto>();

            foreach (var author in authors)
            {
                authorsDto.Add(new AuthorDto
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });
            }

            return Ok(authorsDto);

        }




    }
}