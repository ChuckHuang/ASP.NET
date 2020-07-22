using Microsoft.AspNetCore.Mvc;
using MovieStore.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieStore.MVC.Views.Shared.Components.Genres
{
    public class GenresViewComponent:ViewComponent
    {
        private readonly IGenreService _genreService;
        public GenresViewComponent(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genres = await _genreService.GetAllGenres();
            return View(genres);
        }
    }

}
