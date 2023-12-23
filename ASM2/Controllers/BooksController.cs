using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM2.Data;
using ASM2.Models;
using Microsoft.AspNetCore.Authorization;

namespace ASM2.Controllers
{
    public class BooksController : Controller
    {

        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "admin")]
        // GET: Books
        public async Task<IActionResult> Index()
        {
              return _context.Books != null ? 
                          View(await _context.Books.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Books'  is null.");
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

		[Authorize(Roles = "admin")]
		public IActionResult Create()
        {
            List<Category> categories = _context.Category.ToList();
            ViewBag.CategoryList = new SelectList(categories, "Id", "KindOfBooks");
            return View();
        }

		[Authorize(Roles = "admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameOfBook,Details,NameAuthor,PublicDay,Price,Picture,Category")] Books books)
        {
            if (ModelState.IsValid)
            {
                _context.Add(books);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(books);
        }

		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Edit(int? id)
        {
            List<Category> categories = _context.Category.ToList();
            ViewBag.CategoryList = new SelectList(categories, "Id", "KindOfBooks");
            
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }
            return View(books);
        }

		[Authorize(Roles = "admin")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameOfBook,Details,NameAuthor,PublicDay,Price,Picture,Category")] Books books)
        {
            if (id != books.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(books);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BooksExists(books.Id))
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
            return View(books);
        }

		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }

		[Authorize(Roles = "admin")]
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Books'  is null.");
            }
            var books = await _context.Books.FindAsync(id);
            if (books != null)
            {
                _context.Books.Remove(books);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> AddToCart(int? Id)
        {
            Books b;
            CartItem cartItem = new CartItem();
            if (Id != null)
            {
                b = _context.Books.FirstOrDefault(b => b.Id ==Id);
                cartItem.Books = b;
                cartItem.ProductName = b.NameOfBook;
				cartItem.Total = (float)b.Price;
			}
			cartItem.Quantity = 1;

			_context.Add(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "HomePage");
        }

        public async Task<IActionResult> EditCard(int? id)
        {
            if (id == null || _context.CartItem == null)
            {
                return NotFound();
            }

            var cartid = await _context.CartItem.FindAsync(id);
            if (cartid == null)
            {
                return NotFound();
            }
            return View(cartid);
        }
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditCart(int id, int quantity, [Bind("Id , ToTal, Quantity, Books")] CartItem cartItem)
		{
			if (id != cartItem.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
                    Books b;
                    b = _context.Books.FirstOrDefault(b => b.Id == id);
                    cartItem.Total = cartItem.Quantity*(float)b.Price;
					_context.Update(cartItem);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!BooksExists(cartItem.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction("Index","Home");
			}
			return View(cartItem);
		}
		private bool BooksExists(int id)
        {
          return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
