﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Core.ServiceInterfaces;
using MovieStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MovieStore.Core.RepositoryInterfaces;
using MovieStore.Core.Entities;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MovieStore.MVC.Controllers
{

    public class MoviesController : Controller
    {
        //interview question
        //IOC, ASP.NET Core has built-in IOC/DI
        //In .NET Framework we need to reply on third-party IOC to do the Dependency Injection
        private readonly IMovieService _movieService;
        private readonly MovieStoreDbContext _dbContext;
        private readonly IGenreRepository _genreRepository;
        public MoviesController(IMovieService movieService, MovieStoreDbContext dbContext, IGenreRepository genreRepository)
        {
            _movieService = movieService;
            _dbContext = dbContext;
            _genreRepository = genreRepository;
        }
        //GET localhost/Movies/index
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            //call our Movie Service method, highest grossing method
            var movies = await _movieService.GetTop25HiestRevenueMovies();
            return View(movies);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int movieId)
        {
            var movie = await _movieService.GetMovieById(movieId);

            var user = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (user != null)
            {
                int userId = Int32.Parse(user.Value);
                ViewBag.AlreadyBought = await _movieService.IsBought(userId, movieId);
                ViewBag.IsFavorite = await _movieService.IsFavorite(userId, movieId);
            }
            if (!movie.Reviews.Any())
            {
                var review = new Review()
                {
                    MovieId = movieId
                };
                movie.Reviews.Add(review);
            }
            return View(movie);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Purchase([Bind("Id, Title, Price")]Movie movie)
        {
            int userId = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var purchase = new Purchase()
            {
                UserId = userId,
                MovieId = movie.Id,
                PurchaseNumber = Guid.NewGuid(),
                TotalPrice = movie.Price == null ? 0 : movie.Price.Value,
                PurchaseDateTime = DateTime.Now
            };
            _dbContext.Add(purchase);
            await _dbContext.SaveChangesAsync();

            TempData["message"] = $"Purhase of {movie.Title} completed successfully.";
            return RedirectToAction("Details", "Movies", new { movieId = movie.Id });
        }
       
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Favorite([Bind("Id, Title")]Movie movie)
        {
            if (ModelState.IsValid)
            {
                //getting logged in user's ID attaching it to the Order
                int userId = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                var favorite = new Favorite()
                {
                    UserId = userId,
                    MovieId = movie.Id,                 
                };
                _dbContext.Add(favorite);
                await _dbContext.SaveChangesAsync();

                TempData["message"] = $"{movie.Title} has been added to favorites successfully.";
                return RedirectToAction("Details", "Movies", new { movieId = movie.Id });

            }
            else
            {
                return View();

            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Unfavorite([Bind("Id, Title")]Movie movie)
        {
            if (ModelState.IsValid)
            {
                //getting logged in user's ID attaching it to the Order
                int userId = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                var favorite = _dbContext.Favorites.FirstOrDefault(f => f.MovieId == movie.Id && f.UserId == userId);
                _dbContext.Remove(favorite);
                await _dbContext.SaveChangesAsync();

                TempData["message"] = $"{movie.Title} has been removed from favorites successfully.";
                return RedirectToAction("Details", "Movies", new { movieId = movie.Id });
            }
            else
            {
                return View();

            }
        }
        [HttpGet]
        public async Task<IActionResult> ByGenre(int genreId)
        {

            var movies = await _dbContext.Movies
                .Include(m => m.MovieGenres)
                .Where(m => m.MovieGenres
                      .Any(mg => mg.GenreId == genreId))
                .ToListAsync();
            var genre = await _genreRepository.GetByIdAsync(genreId);
            if (genre != null)
            {
                ViewData["Title"] = genre.Name;
            }
            else
            {
                ViewData["Title"] = "N/A";
            }
            return View(movies);
        }

        [HttpPost]
        public IActionResult Create(string title, decimal budget, string TITLE, string tite)
        {
            //POST//http:localhost/Movies/create
            //interview question: model binding
            //whatever is post to the server, as long as the parameter name is the same,
            //then it will take the value to the attribute, they are case-insensitive
            //look at the incoming request and maps the input fields name with the paramter 
            //names of the action method, then the paramter will have the value automatically
            //it will also do casting/converting
            //we need to get the data from view and save it in database
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            //http:localhost/Movies/create
            //we need to have this method so that we can show the empty page for user to enter movie 
            //information that need to be created
            return View();
        }

        
    }
}
