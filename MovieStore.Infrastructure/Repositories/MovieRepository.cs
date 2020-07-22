using MovieStore.Core.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MovieStore.Infrastructure;
using MovieStore.Core.Entities;
using System.Threading.Tasks;
using MovieStore.Infrastructure.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MovieStore.Infrastructure.Repositories
{
    public class MovieRepository : ERepository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieStoreDbContext dbContext) : base(dbContext)
        {

        }


        public override async Task<Movie> GetByIdAsync(int id)
        {
            var movie = await _dbContext.Movies.Include(m => m.Reviews) //all the child tables going to be populated
                .Include(m => m.MovieCasts).ThenInclude(mc => mc.Cast)//populate cast from MovieCast
                .Include(m=>m.MovieGenres).ThenInclude(mg=>mg.Genre)
                .FirstOrDefaultAsync(m=>m.Id==id);
            return movie;
        } 
        public async Task<IEnumerable<Movie>> GetHighestRevenueMovies()
        {
            var movies = await _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(25).ToListAsync();
            // select top 25 from Movies order by Revenue desc;
            return movies;
        }

        public async Task<IEnumerable<Movie>> GetTop25RatedMovies()
        {
            var movies= await _dbContext.Reviews.Include(m => m.Movie)
                                                 .GroupBy(r => new
                                                 {
                                                     Id = r.MovieId,
                                                     r.Movie.PosterUrl,
                                                     r.Movie.Title,
                                                     r.Movie.ReleaseDate
                                                 })
                                                 .OrderByDescending(g => g.Average(m => m.Rating))
                                                 .Select(m => new Movie
                                                 {
                                                     Id = m.Key.Id,
                                                     PosterUrl = m.Key.PosterUrl,
                                                     Title = m.Key.Title,
                                                     ReleaseDate = m.Key.ReleaseDate,
                                                     Rating = m.Average(x => x.Rating)
                                                 })
                                                 .Take(25)
                                                 .ToListAsync();
            return movies;
        }


    }
}
