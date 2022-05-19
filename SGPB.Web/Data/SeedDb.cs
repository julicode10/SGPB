using SGPB.Web.Data.Entities;
using SGPB.Web.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace SGPB.Web.Data
{
        public class SeedDb
        {
                private readonly ApplicationDbContext _context;
               
                private readonly IBlobHelper _blobHelper;

                public SeedDb(ApplicationDbContext context,  IBlobHelper blobHelper)
                {
                        _context = context;
                        _blobHelper = blobHelper;
                }

                public async Task SeedAsync()
                {
                        await _context.Database.EnsureCreatedAsync();
                        await CheckCategoriesAsync();
                        await CheckDocumentTypesAsync();
                        await CheckEditorialesAsync();

                }

                private async Task CheckCategoriesAsync()
                {
                        if (!_context.Categories.Any())
                        {
                                _context.Categories.Add(new Category { Name = "Científicos" });
                                _context.Categories.Add(new Category { Name = "Literatura y lingüísticos" });
                                _context.Categories.Add(new Category { Name = "De viaje" });
                                _context.Categories.Add(new Category { Name = "Biografías" });
                                _context.Categories.Add(new Category { Name = "Nutrición" });
                                _context.Categories.Add(new Category { Name = "Libros de gran formato" });
                                _context.Categories.Add(new Category { Name = "Ciencia ficción" });
                                _context.Categories.Add(new Category { Name = "De referencia o consulta" });
                                _context.Categories.Add(new Category { Name = "Recreativos" });
                                _context.Categories.Add(new Category { Name = "Poéticos" });
                                _context.Categories.Add(new Category { Name = "Juveniles" });
                                await _context.SaveChangesAsync();
                        }
                }

                private async Task CheckDocumentTypesAsync()
                {
                        if (!_context.DocumentTypes.Any())
                        {
                                _context.DocumentTypes.Add(new DocumentType { Abbreviation = "CC", Name = "Cédula" });
                                _context.DocumentTypes.Add(new DocumentType { Abbreviation = "CE", Name = "Cédula extranjería" });
                                _context.DocumentTypes.Add(new DocumentType { Abbreviation = "DIP", Name = "Diplomatico" });
                                _context.DocumentTypes.Add(new DocumentType { Abbreviation = "PAS", Name = "Pasaporte" });
                                _context.DocumentTypes.Add(new DocumentType { Abbreviation = "REC", Name = "Registro civil" });
                                _context.DocumentTypes.Add(new DocumentType { Abbreviation = "TI", Name = "Tarjeta de identidad" });
                                await _context.SaveChangesAsync();
                        }
                }

                private async Task CheckEditorialesAsync()
                {
                        if (!_context.Editoriales.Any())
                        {
                                _context.Editoriales.Add(new Editorial { Name = "Alianza Distribuidora de Colombia" });
                                _context.Editoriales.Add(new Editorial { Name = "Babel Libros" });
                                _context.Editoriales.Add(new Editorial { Name = "Cangrejo Editores" });
                                _context.Editoriales.Add(new Editorial { Name = "Carvajal Ediciones" });
                                _context.Editoriales.Add(new Editorial { Name = "Ediciones Urano" });
                                _context.Editoriales.Add(new Editorial { Name = "Editorial Gato Malo" });
                                _context.Editoriales.Add(new Editorial { Name = "Editorial Planeta" });
                                _context.Editoriales.Add(new Editorial { Name = "Grupo Penta" });
                                _context.Editoriales.Add(new Editorial { Name = "Tragaluz Editores" });
                                _context.Editoriales.Add(new Editorial { Name = "Villegas Editores" });
                                _context.Editoriales.Add(new Editorial { Name = "Editorial Sexto Piso" });
                                await _context.SaveChangesAsync();
                        }
                }
        }
}
