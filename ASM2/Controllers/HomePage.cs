using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM2.Data;
using ASM2.Models;


namespace ASM2.Controllers
{
    public class HomePage : Controller
    {
		private readonly ApplicationDbContext _context;

		public HomePage(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Books
		public async Task<IActionResult> Index()
		{
			return _context.Books != null ?
						View(await _context.Books.ToListAsync()) :
						Problem("Entity set 'ApplicationDbContext.Books'  is null.");
		}
		public async Task<IActionResult> View(int? id)
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
	}
}
