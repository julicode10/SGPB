using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGPB.Web.Data;
using System.Linq;

namespace SGPB.Web.Controllers.API
{
        [ApiController]
        [Route("api/[controller]")]
        public class BooksController : ControllerBase
        {
                private readonly ApplicationDbContext _context;

                public BooksController(ApplicationDbContext context)
                {
                        _context = context;
                }

                [HttpGet]
                public IActionResult GetBooks()
                {
                        return Ok(_context.Books
                            .Include(p => p.Category)
                            .Include(p => p.Editorial)
                            .Include(p => p.BookImages)
                            .Where(p => p.IsActive)
                       );
                }

        }
}
