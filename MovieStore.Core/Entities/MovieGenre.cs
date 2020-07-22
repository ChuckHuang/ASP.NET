using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Core.Entities
{
    public class MovieGenre
    {
        public int MovieId { get; set; }
        public int GenreId { get; set; }
        //navigation properties
        public virtual Movie Movie { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
