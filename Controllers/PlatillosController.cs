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
    public class PlatillosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PlatillosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Platillos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Platillos.Include(p => p.Categoria);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Platillos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Platillos == null)
            {
                return NotFound();
            }

            var platillo = await _context.Platillos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (platillo == null)
            {
                return NotFound();
            }

            return View(platillo);
        }

        // GET: Platillos/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Nombre");
            return View();
        }

        // POST: Platillos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,IdCategoria,Descripcion,Precio,UrlImagen")] Platillo platillo)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _webHostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if (archivos.Count()>0)
                {
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\platillos\");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(subidas, nombreArchivo+extension),FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStream);
                    }
                    platillo.UrlImagen = @"imagenes\platillos\" + nombreArchivo+extension;
                }
                _context.Add(platillo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Nombre", platillo.IdCategoria);
            return View(platillo);
        }

        // GET: Platillos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Platillos == null)
            {
                return NotFound();
            }

            var platillo = await _context.Platillos.FindAsync(id);
            if (platillo == null)
            {
                return NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Nombre", platillo.IdCategoria);
            return View(platillo);
        }

        // POST: Platillos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,IdCategoria,Descripcion,Precio,UrlImagen")] Platillo platillo)
        {
            if (id != platillo.Id)
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
                        Platillo? platilloBD = await _context.Platillos.FindAsync(id);
                        if (platilloBD != null)
                        {
                            if (platilloBD.UrlImagen!=null)
                            {
                                var rutaImagenActual = Path.Combine(rutaPrincipal, platilloBD.UrlImagen);
                                if (System.IO.File.Exists(rutaImagenActual))
                                {
                                    System.IO.File.Delete(rutaImagenActual);
                                }
                            }
                            _context.Entry(platilloBD).State = EntityState.Detached;
                        }
                        string nombreArchivo = Guid.NewGuid().ToString();
                        var subidas = Path.Combine(rutaPrincipal, @"imagenes\platillos\");
                        var extension = Path.GetExtension(archivos[0].FileName);
                        using (var fileStream = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                        {
                            archivos[0].CopyTo(fileStream);
                        }
                        platillo.UrlImagen = @"imagenes\platillos\" + nombreArchivo + extension;
                        _context.Entry(platillo).State = EntityState.Modified;
                    }
                    _context.Update(platillo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlatilloExists(platillo.Id))
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
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "Id", "Nombre", platillo.IdCategoria);
            return View(platillo);
        }

        // GET: Platillos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Platillos == null)
            {
                return NotFound();
            }

            var platillo = await _context.Platillos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (platillo == null)
            {
                return NotFound();
            }

            return View(platillo);
        }

        // POST: Platillos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Platillos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Platillos'  is null.");
            }
            var platillo = await _context.Platillos.FindAsync(id);
            if (platillo != null)
            {
                _context.Platillos.Remove(platillo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlatilloExists(int id)
        {
          return (_context.Platillos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
