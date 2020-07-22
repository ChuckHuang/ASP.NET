using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MovieStore.Core.Entities
{
    //genre class reprsents our Domain Model, we are gonna have all the properties
    //Data Annoation
    [Table(name:"Genre")]
    public class Genre
    {
        public int Id { get; set; }
        [MaxLength(64)]
        [Required]
        public string Name { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
