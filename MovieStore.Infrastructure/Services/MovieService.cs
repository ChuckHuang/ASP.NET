using MovieStore.Core.Entities;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        //constructor Injection, inject MovieRepository class instance
         public MovieService(IMovieRepository movieRepository) //when you have an interface in the parameter
            //any class that implements this interface can be passed
        {
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<Movie>> GetTop25HiestRevenueMovies()
        {
            return await _movieRepository.GetHighestRevenueMovies();
        }

        public async Task<IEnumerable<Movie>> GetTop25RatedMovies()
        {
            return await _movieRepository.GetTop25RatedMovies();
        }

        public virtual async Task<Movie> GetMovieById(int Id)
        {
            return await _movieRepository.GetByIdAsync(Id);
        }

        public async Task<Movie> CreateMovie(Movie movie)
        {
            return await _movieRepository.AddAsync(movie);
        }
        public async Task<Movie> UpdateMovie(Movie movie)
        {
            return await _movieRepository.UpdateAsync(movie);
        }

        public Task<int> GetMovieCount(string title=" ")
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsBought(int userId, int movieId)
        {
            return await _movieRepository.IsBought(userId, movieId);
        }
    }
  
}
