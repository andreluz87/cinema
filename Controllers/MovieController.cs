using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Cinema.Context;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly AppDbContext _context;
        private readonly string[] _fileTypes = { "image/jpeg" };

        public MovieController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Movies
         [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            var _search = search;
            if (string.IsNullOrEmpty(_search))
                return View(await _context.Movies.ToListAsync());

            var appDbContext = _context.Movies.Where(m => m.Title.ToLower().Contains(_search.ToLower())).ToListAsync();
            ViewBag.search = _search;        

            return View(await appDbContext);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Create([Bind("MovieId,Title,Description,Image,Duration")] Movie movie)
        {
            //upload de imagem
            var _requestForm = Request.Form;
            var _movie = movie;

            if (_requestForm.Files.Count > 0)
            {
                try
                {
                    var _file = _requestForm.Files[0];
                    if (_fileTypes.Contains(_file.ContentType))
                    {
                        //TODO Criar pasta com o MovieId
                        var folderName = Path.Combine("wwwroot", "imgs/Movies/");
                        var fileName = ContentDispositionHeaderValue.Parse(_file.ContentDisposition).FileName.Trim('"');
                        var dbPath = Path.Combine(folderName, fileName);
                        using (var stream = new FileStream(dbPath, FileMode.Create))
                        {
                            _file.CopyTo(stream);
                            //forma implicita do System.IO que nao estava encontrando o 'File';
                            if(System.IO.File.Exists(dbPath))
                            {
                                ModelState.Clear();
                                _movie.Image = fileName;
                                TryValidateModel(_movie);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Image", "Desculpe, não foi encontrado nenhum tipo de imagem permitida.");
                    }
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("Image", "Ocorreu um erro ao realizar o upload do arquivo, tente novamente mais tarde..");
                }           
            }

            if (ModelState.IsValid)
            {
                if (!MovieExistsByTitle(_movie.Title)){
                    
                    _context.Add(_movie);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }else
                {
                    ModelState.AddModelError("Title", "Desculpe, o Título    escolhido para o filme já existe.");
                }
            }
            return View(_movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,Title,Image,Description,Duration")] Movie movie)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            ViewBag.DeleteMovieError = false;

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int MovieId)
        {
            var movie = await _context.Movies.FindAsync(MovieId);
            if (!MovieAtSession(MovieId))
            {
                DeleteImageFile(movie.Image);
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
            else
            {
                ViewBag.DeleteMovieError = true;
                return View(movie);
            }
            return RedirectToAction(nameof(Index));
        }

        public bool DeleteImageFile(string fileName)
        {
            string _fileName = Path.Combine("wwwroot", "imgs/Movies/" + fileName);
            if (System.IO.File.Exists(_fileName))
            {
                System.IO.File.Delete(_fileName);
                return true;
            }
            return false;
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }

        //Verifica se a existencia do filme pelo Título
        private bool MovieExistsByTitle(string title)
        {
            string _title = title.Trim();
            return _context.Movies.Any(m =>m.Title.Equals(_title));
        }

        //Valida se o filme esta associado a alguma Sessão.
        private bool MovieAtSession(int id)
        {
            return _context.Sessions.Any(s => s.MovieId == id);
        }

    }
}
