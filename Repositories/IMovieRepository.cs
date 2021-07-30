using System.Collections.Generic;
using Cinema.Models;

namespace Cinema.Repositories
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> Movies { get; }
        Movie GetMovieById(int MovieId);
    }
}
