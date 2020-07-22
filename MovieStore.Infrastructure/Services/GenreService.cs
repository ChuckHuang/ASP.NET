using MovieStore.Core.Entities;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
       
        public GenreService(IGenreRepository genreRepository) //when you have an interface in the parameter
                                                              //any class that implements this interface can be passed
        {
            _genreRepository = genreRepository;
        }
        public async Task<IEnumerable<Genre>> GetAllGenres()
        {
            return await  _genreRepository.ListAllAsync();
        }
    }
}
