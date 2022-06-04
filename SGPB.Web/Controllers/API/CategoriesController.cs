using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGPB.Web.Data;

namespace SGPB.Web.Controllers.API
{
        [ApiController]
        [Route("api/[controller]")]

        public class CategoriesController : ControllerBase
        {
                private readonly ApplicationDbContext _context;

                public CategoriesController(ApplicationDbContext context)
                {
                        _context = context;
                }

                [HttpGet]
                public IActionResult GetCategories()
                {
                        return Ok(_context.Categories);
                }
        }
}
