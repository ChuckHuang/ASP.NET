using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieStore.Core.Entities;
using System.Collections.Generic;
using System.Linq;
namespace MovieStore.Infrastructure.Data
{
    //install all the EF Core libraries using Nuget package Manager
    //create a class that inherits from dbcontext class
    //dbcontext kinda represnts your databse
    //create a connectionstring which EF is gonna use to create/access the database
    //should include your server name, database name and any credentials.
    //create an entity class, genre table
    //in EF we have Migration we are gonna use Migration to create our Database
    //we need to add Migration commands to create the tables, database etc
    //when running Migration commands, make sure the project selected is the one project which has DbContext
    //make sure you add reference for library that has DbContext to your starup project
    //tell MVC project that we are using Entity framework in startup files
    //add DbContext options as constructor parameter for out DbContext
    //Add-Migration xxxx, make sure your migration names are meaningful, dont use names like abc, xx
    //make sure you have Migration folder created and check the created migration file
    //after its done, verify it we need to use update-database command
    //whenever changing the model/entity, always make sure you add new migration
    //with CF approach, never change the database directly, always change the entities using DataAnnotaion
    //or fluent API and add new Migration then update the database

    //in EF we have 2 ways to create entities and model our database using code-First Approach
    //1. Data Annotations which is nothing but bunch of attributes that we can use
    //2. Fluent API is more syntax and more powerful and usually use lambdas
    //combine both DataAnnotations and Fluent API
    public class MovieStoreDbContext : DbContext
    {
        //multiple dbsets; all the dbsets you create are gonna reside in your Dbcontext class
        public MovieStoreDbContext(DbContextOptions<MovieStoreDbContext> options): base(options)
        {

        }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Trailer> Trailers { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }

        public DbSet<Cast> Casts { get; set; }

        public DbSet<MovieCast> MovieCasts { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<Review> Reviews { get; set; }
        //regular lists, Dicti..all collection implement IEnumerable,
        //so linq methods will point to IEnumerable extension methods
        //Dbsets, since they implement IQuerable they point to IQuerable methods.
        public void Test()
        {
            var ll = new List<int>();
            //Func<Tsource, bool> predicate
            ll.Where(x => x > 3);
            //Expression<Func<Tsource, bool>> needs to be translated into SQL later
            Genres.Where(g =>g.Id==2);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(ConfigureMovie);
            modelBuilder.Entity<Trailer>(ConfigureTrailer);
            modelBuilder.Entity<MovieGenre>(ConfigureMovieGenre);
            modelBuilder.Entity<Cast>(ConfigureCast);
            modelBuilder.Entity<MovieCast>(ConfigureMovieCast);
            modelBuilder.Entity<User>(ConfigureUser);
            modelBuilder.Entity<Role>(ConfigureRole);
            modelBuilder.Entity<UserRole>(ConfigureUserRole);
            modelBuilder.Entity<Purchase>(ConfigurePurchase);
            modelBuilder.Entity<Favorite>(ConfigureFavorite);
            modelBuilder.Entity<Review>(ConfigureReview);
            modelBuilder.Entity<Genre>(ConfigureGenre);
        }
        private void ConfigureGenre(EntityTypeBuilder<Genre> modelbuilder)
        {
            // ToTable(table) method is used to define the Table name for Entity Class, in this case we are creating Genre table. Equivalent to the Table attribute
            modelbuilder.ToTable("Genre");

            // HasKey(selector) method takes lambda expression that selects the primary key for our Table, in our case we want Id as primary Key. It is similar to [Key] attribute in data annotations.
            modelbuilder.HasKey(g => g.Id);

            // Property(selector) is used to describe more details about property or column in our table, like making it not null or restricting the maximum length etc and many more.
            modelbuilder.Property(g => g.Name).IsRequired().HasMaxLength(64);
        }
        private void ConfigureReview(EntityTypeBuilder<Review> modelBuilder)
        {
            modelBuilder.ToTable("Review");
            modelBuilder.HasKey(r => new { r.MovieId, r.UserId });
            modelBuilder.HasOne(r => r.Movie).WithMany(r => r.Reviews).HasForeignKey(r => r.MovieId);
            modelBuilder.HasOne(r => r.User).WithMany(r => r.Reviews).HasForeignKey(r => r.UserId);
            modelBuilder.Property(r => r.ReviewText).HasMaxLength(20000);
            modelBuilder.Property(r => r.Rating).HasColumnType("nvarchar(max)");
        }
        private void ConfigureMovieCast(EntityTypeBuilder<MovieCast> modelBuilder)
        {
            modelBuilder.ToTable("MovieCast");
            modelBuilder.HasKey(mc => new { mc.MovieId, mc.CastId });
            modelBuilder.HasOne(mc => mc.Movie).WithMany(mc => mc.MovieCasts).HasForeignKey(mc => mc.MovieId);
            modelBuilder.HasOne(mc => mc.Cast).WithMany(mc => mc.MovieCasts).HasForeignKey(mc => mc.CastId);
        }

        private void ConfigureFavorite(EntityTypeBuilder<Favorite> builder)
        {
            builder.ToTable("Favorite");
            builder.HasKey(f => f.Id);
        }
        private void ConfigurePurchase(EntityTypeBuilder<Purchase> modelBuilder)
        {
            modelBuilder.ToTable("Purchase");
            modelBuilder.HasKey(p => p.Id);
            modelBuilder.Property(p => p.UserId);
            modelBuilder.Property(p => p.PurchaseNumber).ValueGeneratedOnAdd();
            modelBuilder.Property(p => p.TotalPrice).HasColumnType("decimal(5, 2)").HasDefaultValue(9.9m);
            modelBuilder.Property(p => p.PurchaseDateTime).HasColumnType("datetime2");
        }
        private void ConfigureRole(EntityTypeBuilder<Role> modelBuilder)
        {
            modelBuilder.ToTable("Role");
            modelBuilder.HasKey(r => r.Id);
            modelBuilder.Property(r => r.Name).HasMaxLength(20);
        }

        private void ConfigureUserRole(EntityTypeBuilder<UserRole> modelBuilder)
        {
            modelBuilder.ToTable("UserRole");
            modelBuilder.HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.HasOne(ur => ur.Role).WithMany(ur => ur.UserRoles).HasForeignKey(ur => ur.RoleId);
            modelBuilder.HasOne(ur => ur.User).WithMany(ur => ur.UserRoles).HasForeignKey(ur => ur.UserId);
        }
        private void ConfigureUser(EntityTypeBuilder<User> modelBuilder)
        {
            modelBuilder.ToTable("User");
            modelBuilder.HasKey(u => u.Id);
            modelBuilder.Property(u => u.Email).HasMaxLength(256);
            modelBuilder.Property(u => u.FirstName).HasMaxLength(128);
            modelBuilder.Property(u => u.DateOfBirth).HasColumnType("datetime2");
            modelBuilder.Property(u => u.LastName).HasMaxLength(128);
            modelBuilder.Property(u => u.HashedPassword).HasMaxLength(1024);
            modelBuilder.Property(u => u.Salt).HasMaxLength(1024);
            modelBuilder.Property(u => u.PhoneNumber).HasMaxLength(16);
            modelBuilder.Property(u => u.TwoFactorEnabled).HasDefaultValue(false);
            modelBuilder.Property(u => u.LockoutEndDate).HasColumnType("DateTimeOffset");
            modelBuilder.Property(u => u.LastLoginDateTime).HasColumnType("datetime2");
            modelBuilder.Property(u => u.AccessFailedCount);
            modelBuilder.Property(u => u.IsLocked).HasDefaultValue(false);
              
        }
        private void ConfigureCast(EntityTypeBuilder<Cast> modelBuilder)
        {
            modelBuilder.ToTable("Cast");
            modelBuilder.HasKey(c=>c.Id);
            modelBuilder.Property(c => c.Name).HasMaxLength(256);
            modelBuilder.Property(c => c.Gender).HasMaxLength(20);
            modelBuilder.Property(c => c.TmdbUrl).HasMaxLength(2084);
            modelBuilder.Property(c => c.ProfilePath).HasMaxLength(2084); 

        }
      
        private void ConfigureMovieGenre(EntityTypeBuilder<MovieGenre> modelBuilder)
        {
            modelBuilder.ToTable("MovieGenre");
            modelBuilder.HasKey(mg => new { mg.MovieId, mg.GenreId });
            modelBuilder.HasOne(mg => mg.Movie).WithMany(mg => mg.MovieGenres).HasForeignKey(mg => mg.MovieId);
            modelBuilder.HasOne(mg => mg.Genre).WithMany(mg => mg.MovieGenres).HasForeignKey(mg => mg.GenreId);
        }
        private void ConfigureTrailer(EntityTypeBuilder<Trailer> modelBuilder)
        {
            modelBuilder.ToTable("Trailer");
            modelBuilder.HasKey(t => t.Id);
            modelBuilder.Property(t => t.Name).HasMaxLength(2084);
            modelBuilder.Property(t => t.TrailerUrl).HasMaxLength(2084);
        }
        private void ConfigureMovie(EntityTypeBuilder<Movie> modelBuilder)
        {
            //we can use Fluent API Configuration to model our tables
            modelBuilder.ToTable("Movie");
            modelBuilder.HasKey(m => m.Id);
            modelBuilder.Property(m => m.Title).IsRequired().HasMaxLength(256);
            modelBuilder.Property(m => m.Overview).HasMaxLength(4096);
            modelBuilder.Property(m => m.Tagline).HasMaxLength(512);
            modelBuilder.Property(m => m.ImdbUrl).HasMaxLength(2084);
            modelBuilder.Property(m => m.TmdbUrl).HasMaxLength(2084);
            modelBuilder.Property(m => m.PosterUrl).HasMaxLength(2084);
            modelBuilder.Property(m => m.BackdropUrl).HasMaxLength(2084);
            modelBuilder.Property(m => m.OriginalLanguage).HasMaxLength(64);
            modelBuilder.Property(m => m.Price).HasColumnType("decimal(5, 2)").HasDefaultValue(9.9m);
            modelBuilder.Property(m => m.CreatedDate).HasDefaultValueSql("getdate()");
            //we dont want to create Rating Column but we want C# rating property in our entity
            //so that we can show Movie rating in UI
            modelBuilder.Ignore(m => m.Rating);
        }

        
    }
}

 
