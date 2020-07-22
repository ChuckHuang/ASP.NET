using System.Linq;
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

            //var movies = new List<Movie>
            //{
            //    new Movie {Id = 1, Title = "Avengers: Infinity War", Budget = 1200000},
            //    new Movie {Id = 2, Title = "Avatar", Budget = 1200000},
            //    new Movie {Id = 3, Title = "Star Wars: The Force Awakens", Budget = 1200000},
            //    new Movie {Id = 4, Title = "Titanic", Budget = 1200000},
            //    new Movie {Id = 5, Title = "Inception", Budget = 1200000},
            //    new Movie {Id = 6, Title = "Avengers: Age of Ultron", Budget = 1200000},
            //    new Movie {Id = 7, Title = "Interstellar", Budget = 1200000},
            //    new Movie {Id = 8, Title = "Fight Club", Budget = 1200000},
            //};
            //ViewBag.MoviesCount = movies.Count;
            //ViewData["myname"] = "John Doe";
            //using data strongly type to send data from controller to view
            //compile time check vs run time check;
            //go to the databse and get some list of movies and give it to the view
            //we need to pass data from controller action method to the view
            //usually its preferred to send a strongly typed model or object
            //3 ways to send data from controller to view:
            //1.strongly typed models(preferred way)
            //2.ViewBag-dynamic
            //3.ViewData-key/value
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
            }

            return View(movie);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Purchases()
        {
            var user = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            int userId = Int32.Parse(user.Value);
            var purchasedMovieIdList = await _dbContext.Purchases.Where(p => p.UserId == userId).Select(p => p.MovieId).ToListAsync();
            var purchasedMovies = await _dbContext.Movies.Where(m => purchasedMovieIdList.Contains(m.Id)).ToListAsync();
            return View(purchasedMovies);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Purchase([Bind("Id, Title, Price")]Movie movie)
        {
            if (ModelState.IsValid)
            {
                //getting logged in user's ID attaching it to the Order
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

        [HttpGet]
        public ActionResult Review(int id)
        {
            var review = new Review()
            {
                MovieId = id
            };


            return PartialView("~/Views/Shared/_Review.cshtml", review);
        }
        [HttpPost]
        public async Task<IActionResult> Review([Bind("MovieId, ReviewText, Rating")]Review review)
        {
            if (ModelState.IsValid)
            {
                //getting logged in user's ID attaching it to the Order
                int userId = Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
                review.UserId = userId;
                var existingReview = _dbContext.Reviews.FirstOrDefault(r => r.MovieId == review.MovieId && r.UserId == review.UserId);
                if (existingReview == null)
                {
                    _dbContext.Add(review);
                }
                else
                {
                    existingReview.ReviewText = review.ReviewText;
                    existingReview.Rating = review.Rating;
                }
                await _dbContext.SaveChangesAsync();

                TempData["message"] = $"Review completed successfully.";
                return RedirectToAction("Details", "Movies", new { movieId = review.MovieId });

            }
            else
            {
                return View();

            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Reviews()
        {
            var user = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            int userId = Int32.Parse(user.Value);
            var reviews = await _dbContext.Reviews.Where(p => p.UserId == userId).Include(r => r.Movie).ToListAsync();
            return View(reviews);
        }
    }
}
