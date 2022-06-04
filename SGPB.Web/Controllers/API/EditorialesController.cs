using Microsoft.AspNetCore.Mvc;
using SGPB.Web.Data;

namespace SGPB.Web.Controllers.API
{
        [ApiController]
        [Route("api/[controller]")]
        public class EditorialesController : ControllerBase
        {
                private readonly ApplicationDbContext _context;

                public EditorialesController(ApplicationDbContext context)
                {
                        _context = context;
                }

                [HttpGet]
                public IActionResult GetEditoriales()
                {
                        return Ok(_context.Editoriales);
                }
        }
}
