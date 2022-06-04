using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restaurante.Data;
using Restaurante.Models;

namespace Restaurante.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoriasController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> VerPlatillos(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platillos = await _context.Platillos
                .Where(p => p.IdCategoria == id).ToListAsync();
            if (platillos == null)
            {
                return NotFound();
            }

            return View(platillos);
        }


        // GET: Categorias
        public async Task<IActionResult> Index()
        {
              return _context.Categorias != null ? 
                          View(await _context.Categorias.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Categorias'  is null.");
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,UrlImagen")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _webHostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if (archivos.Count() > 0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\categorias\");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStream);
                    }
                    categoria.UrlImagen = @"imagenes\categorias\" + nombreArchivo + extension;
                }
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,UrlImagen")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string rutaPrincipal = _webHostEnvironment.WebRootPath;
                    var archivos = HttpContext.Request.Form.Files;
                    if (archivos.Count() > 0)
                    {
                        Platillo? categoriaBD = await _context.Platillos.FindAsync(id);
                        if (categoriaBD != null)
                        {
                            if (categoriaBD.UrlImagen != null)
                            {
                                var rutaImagenActual = Path.Combine(rutaPrincipal, categoriaBD.UrlImagen);
                                if (System.IO.File.Exists(rutaImagenActual))
                                {
                                    System.IO.File.Delete(rutaImagenActual);
                                }
                            }
                            _context.Entry(categoriaBD).State = EntityState.Detached;
                        }
                        string nombreArchivo = Guid.NewGuid().ToString();
                        var subidas = Path.Combine(rutaPrincipal, @"imagenes\categorias\");
                        var extension = Path.GetExtension(archivos[0].FileName);
                        using (var fileStream = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                        {
                            archivos[0].CopyTo(fileStream);
                        }
                        categoria.UrlImagen = @"imagenes\categorias\" + nombreArchivo + extension;
                        _context.Entry(categoria).State = EntityState.Modified;
                    }
                    
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
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
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categorias == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Categorias'  is null.");
            }
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
          return (_context.Categorias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
