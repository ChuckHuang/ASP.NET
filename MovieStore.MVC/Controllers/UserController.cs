using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MovieStore.MVC.Controllers
{
    //1. purchase a movie, HttpPost, store that info in the Purchase table
    //http:localhost:fsggrw/User/Purchase--HttpPost
    //2. get all the Movies Purchased by user, logged in User, take user id from HttpContext and 
    //get all the movies
    //and give them to Movie Card Partial View
    //http.Localhost:vsdfv/User/Purchases--HttpGet
    //3. Create a Review for a Movie for logged in user, take the userId from the HtpContext and save 
    //info in Review Table
    //http:localhost:fdaa/User/review--httppost
    //Review button will open a popup and ask user to enter a small review in text area and have him 
    //enter movie rating between 1 and 10 and then save
    //4.Get all the movie reviews done by looged in User,
    //http:localhost:fdaa/User/reviews--httpget
    //5.add a favorite movie for logged in user
    //http:localhost:fdaa/User/Favotite--httpPost
    //6.Check if a particular Movie has been added as Favorite by looged in user
    //http:localhost:12112/User/(123)/movie/movieId/favorite HttpGet
    //7. Remove favorite
    //http:localhost/12112/User/Favorite --Httpdelete
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
