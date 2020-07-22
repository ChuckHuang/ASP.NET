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
    public class GenreRepository : ERepository<Genre>, IGenreRepository
    {
        public GenreRepository(MovieStoreDbContext dbContext) : base(dbContext)
        {

        }
    }
}
