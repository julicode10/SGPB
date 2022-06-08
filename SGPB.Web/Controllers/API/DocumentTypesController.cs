using Microsoft.AspNetCore.Mvc;
using SGPB.Web.Data;

namespace SGPB.Web.Controllers.API
{
        [ApiController]
        [Route("api/[controller]")]
        public class DocumentTypesController : ControllerBase
        {
                private readonly ApplicationDbContext _context;

                public DocumentTypesController(ApplicationDbContext context)
                {
                        _context = context;
                }

                [HttpGet]
                public IActionResult GetDocumentTypes()
                {
                        return Ok(_context.DocumentTypes);
                }
        }
}
