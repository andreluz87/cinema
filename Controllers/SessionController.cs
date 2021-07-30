using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cinema.Context;
using Cinema.Models;
using Cinema.ViewModels;

namespace Cinema.Controllers
{
    public class SessionController : Controller
    {
        private readonly AppDbContext _context;

        public SessionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Session
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Sessions.Include(s => s.Movie).Include(s => s.Room);

            ViewBag.AnimationTypes = SessionViewModel.AnimationTypes;
            ViewBag.Audios = SessionViewModel.Audios;

            return View(await appDbContext.ToListAsync());
        }

        // GET: Session/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.Movie)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.SessionId == id);
            if (session == null)
            {
                return NotFound();
            }

            ViewBag.AnimationTypes = SessionViewModel.AnimationTypes;
            ViewBag.Audios = SessionViewModel.Audios;

            return View(session);
        }

        // GET: Session/Create
        public IActionResult Create()
        {
            var movies = _context.Movies;
            var rooms = _context.Rooms;
            //Prepend para simular um 'placeholder'
            ViewData["MovieId"] = new SelectList(movies, "MovieId", "Title", 0).Prepend(new SelectListItem { Text = "Selecione um filme", Value = "0" });
            ViewData["RoomId"] = new SelectList(rooms, "RoomId", "Name");
            
            ViewData["Rooms"] = rooms;
            ViewData["Movies"] = movies;
            
            ViewBag.AnimationType = new SessionViewModel().AnimationTypeSelectList;
            ViewBag.Audio = new SessionViewModel().AudioSelectList;


            return View();
        }

        // POST: Session/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SessionId,Date,Start,End,TicketValue,AnimationType,Audio,MovieId,RoomId")] Session session)
        {
            if (ModelState.IsValid)
            {
                //Encontra o filme para utilizar do atributo 'Duration'
                var movie = await _context.Movies.FindAsync(session.MovieId);
                //retorna o valor para o final da sessão com base hora de início + duração do filme.
                session.End = GetSessionEnd((DateTime)session.Date, (TimeSpan)session.Start, (TimeSpan)movie.Duration);

                _context.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "MovieId", "Title", session.MovieId).Prepend(new SelectListItem { Text = "Selecione um filme", Value = "0" });
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "Name", session.RoomId);
            ViewData["Rooms"] = _context.Rooms; 

            ViewBag.AnimationType = new SessionViewModel().AnimationTypeSelectList;
            ViewBag.Audio = new SessionViewModel().AudioSelectList;

            return View(session);
        }

        // GET: Session/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.Movie)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.SessionId == id);

            if (session == null)
            {
                return NotFound();
            }

            //Verifica se pode ser deletada a sessão
            ViewBag.isDeletable = IsDeletable(session.Date);
            ViewBag.AnimationTypes = SessionViewModel.AnimationTypes;
            ViewBag.Audios = SessionViewModel.Audios;

            return View(session);
        }

        // POST: Session/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var session = await _context.Sessions.FindAsync(id);

            if (IsDeletable(session.Date))
            {
                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        private bool SessionExists(int id)
        {
            return _context.Sessions.Any(e => e.SessionId == id);
        }

        //A Sessão só pode ser deletada se faltar 10 dias para iniciar
        private static Boolean IsDeletable(DateTime sessionDate)
        {
            DateTime today = DateTime.Now;
            var countDays = sessionDate - today;
            if(countDays.Days < 0)
            {
                return true;
            }
            return countDays.Days >= Session._daysForDelete;                
        }

        //Método para retornar a hora do fim da sessão.
        private static TimeSpan GetSessionEnd(DateTime date, TimeSpan start, TimeSpan duration)
        {
            DateTime sessionDuration = date;
            sessionDuration = sessionDuration.Add(start + duration);            

            var concatDuration = sessionDuration.Hour + ":" + sessionDuration.Minute + ":" + sessionDuration.Second; 
            return TimeSpan.Parse(concatDuration);
        }

        //Método para retornar salas que estão indisponíveis para selecionar na sessão.
        [HttpGet]
        public async Task<IActionResult> GetUnavailableRooms(DateTime date, TimeSpan start, TimeSpan duration)
        {
            string _date = date.ToString("yyyy-MM-dd");

            TimeSpan end = GetSessionEnd(date, start, duration);

            var sessions = await _context.Sessions.Where(
                s => s.Date == DateTime.Parse(_date) 
                && s.Start >= start 
                && s.Start < end 
                || start >= s.Start 
                && start < s.End
                && s.Date == DateTime.Parse(_date)) // _date novamente para conseguir interpolar a condição 'OR'
                .ToListAsync(); 

            return Json(sessions);
        }

    }
}
