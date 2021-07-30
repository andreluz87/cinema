using System.Collections.Generic;
using Cinema.Context;
using Cinema.Models;

namespace Cinema.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;

        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Room> Rooms => _context.Rooms;
    }
}
