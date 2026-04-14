using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Wprawka1.Data;
using Wprawka1.Models;
using Microsoft.AspNetCore.Authorization;

namespace Wprawka1.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookstoreContext _context;

        public BooksController(BookstoreContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string searchString)
        {
            var booksQuery = _context.Books.Include(b => b.Publisher).Include(b => b.Authors).AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                booksQuery = booksQuery.Where(b => b.Title!.Contains(searchString));
            }
            ViewData["CurrentFilter"] = searchString;

            return View(await booksQuery.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Publisher)
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(m => m.Id == id
                );
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
            ViewData["AuthorIds"] = new MultiSelectList(_context.Authors, "Id", "LastName");
            return View();
        }

        // POST: Books/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,PublisherId")] Book book, int[] selectedAuthors)
        {
            if (ModelState.IsValid)
            {
                if (selectedAuthors != null && selectedAuthors.Length > 0)
                {
                    book.Authors = new List<Author>();
                    foreach (var authorId in selectedAuthors)
                    {
                        var author = await _context.Authors.FindAsync(authorId);
                        if (author != null)
                        {
                            book.Authors.Add(author);
                        }
                    }
                }

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
            ViewData["AuthorIds"] = new MultiSelectList(_context.Authors, "Id", "LastName", selectedAuthors);
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.Include(b => b.Authors).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);

            var authorIds = book.Authors!.Select(a => a.Id).ToList();
            ViewData["AuthorIds"] = new MultiSelectList(_context.Authors, "Id", "LastName", authorIds);

            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,PublisherId")] Book book, int[] selectedAuthors)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var bookToUpdate = await _context.Books
                        .Include(b => b.Authors)
                        .FirstOrDefaultAsync(b => b.Id == id);

                    if (bookToUpdate == null) return NotFound();

                    bookToUpdate.Title = book.Title;
                    bookToUpdate.PublisherId = book.PublisherId;

                    bookToUpdate.Authors!.Clear();
                    if (selectedAuthors != null && selectedAuthors.Length > 0)
                    {
                        foreach (var authorId in selectedAuthors)
                        {
                            var author = await _context.Authors.FindAsync(authorId);
                            if (author != null) bookToUpdate.Authors.Add(author);
                        }
                    }

                    _context.Update(bookToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
            ViewData["AuthorIds"] = new MultiSelectList(_context.Authors, "Id", "LastName", selectedAuthors);
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
