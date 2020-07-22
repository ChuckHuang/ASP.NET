using MovieStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieStore.Core.RepositoryInterfaces
{
    public interface IMovieRepository: IAsyncRepository<Movie>
    {
        Task<IEnumerable<Movie>> GetHighestRevenueMovies();
        Task<IEnumerable<Movie>> GetTop25RatedMovies();

    }
    //Iasyncrepo has 8 methods
    //public class MovieRepo: ImovieRepository
    //{
         //need to implement 9 methods
    //}
}
