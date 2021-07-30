using System.Collections.Generic;
using Cinema.Models;

namespace Cinema.Repositories
{
    public interface IRoomRepository
    {
        IEnumerable<Room> Rooms { get; }
    }
}
