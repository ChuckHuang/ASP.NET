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
        
    }
    public class MovieServiceTest : IMovieService
    {
        public IEnumerable<Movie> GetTop25HighestRevenueMovies()
        {
            var movies = new List<Movie>()
                        {
                            new Movie {Id = 1, Title = "Avengers: Infinity War", Budget = 1200000},
                            new Movie {Id = 2, Title = "Avatar", Budget = 1200000},
                            new Movie {Id = 3, Title = "Star Wars: The Force Awakens", Budget = 1200000},
                            new Movie {Id = 4, Title = "Titanic", Budget = 1200000},
                        };
            return  movies;
        }

        Task<Movie> IMovieService.CreateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        Task<Movie> IMovieService.GetMovieById(int Id)
        {
            throw new NotImplementedException();
        }

        Task<int> IMovieService.GetMovieCount(string title)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Movie>> IMovieService.GetTop25HiestRevenueMovies()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Movie>> IMovieService.GetTop25RatedMovies()
        {
            throw new NotImplementedException();
        }

        Task<Movie> IMovieService.UpdateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}
