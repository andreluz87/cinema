using System.Collections.Generic;
using Cinema.Models;

namespace Cinema.Repositories
{
    public interface ISessionRepository
    {
        IEnumerable<Session> Sessions { get; }
        //Session GetSessionById(int SessionId);
    }
}
