using System;
using System.Collections.Generic;
using System.Text;

namespace MovieStore.Core.Entities
{
    public class Trailer
    {
        //One trailer belongs to single movie, but one movie can have have multiple trailers.
        public int Id { get; set; }
        public string TrailerUrl { get; set; }

        public string Name { get; set; }

        public int MovieId { get; set; }
        //if someone gave you Trailer Id and you wanna find that Movie details,
        //then this property will be useful
        //Navigation property
        public Movie Movie { get; set; }
    }
}
