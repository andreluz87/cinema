using System.Collections.Generic;
using System.Linq;
using Cinema.Context;
using Cinema.Models;

namespace Cinema.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _context;

        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Movie> Movies => _context.Movies;

        public Movie GetMovieById(int MovieId)
        {
            return _context.Movies.FirstOrDefault(m => m.MovieId == MovieId);
        }
    }
}
