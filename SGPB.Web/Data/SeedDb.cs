using Microsoft.EntityFrameworkCore;
using SGPB.Web.Data.Entities;
using SGPB.Web.Enums;
using SGPB.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGPB.Web.Data
{
        public class SeedDb
        {
                private readonly ApplicationDbContext _context;
                private readonly IUserHelper _userHelper;
                private readonly IBlobHelper _blobHelper;

                public SeedDb(ApplicationDbContext context, IUserHelper userHelper, IBlobHelper blobHelper)
                {
                        _context = context;
                        _userHelper = userHelper;
                        _blobHelper = blobHelper;
                }

                public async Task SeedAsync()
                {
                        await _context.Database.EnsureCreatedAsync();
                        await CheckCategoriesAsync();
                        await CheckDocumentTypesAsync();
                        await CheckEditorialesAsync();
                        await CheckBooksAsync();
                        await CheckRolesAsync();
                        await CheckUserAsync("1010", "Julian", "Londoño", "julian@hotmail.com", "3000000000", "Calle Luna Calle Sol", UserType.Admin);
                        await CheckUserAsync("2020", "Juan Fernando", "Perez", "juanf@hotmail.com", "3000000000", "Calle Luna Calle Sol", UserType.Admin);
                        await CheckUserAsync("3030", "Sol", "Garcia", "solga@hotmail.com", "3000000000", "Calle Luna Calle Sol", UserType.User);
                        await CheckUserAsync("4040", "Maria", "Serrano", "mserrano@hotmail.com", "3000000000", "Calle Luna Calle Sol", UserType.User);
                }

                private async Task CheckRolesAsync()
                {
                        await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
                        await _userHelper.CheckRoleAsync(UserType.User.ToString());
                }

                private async Task<User> CheckUserAsync(
                    string document,
                    string firstName,
                    string lastName,
                    string email,
                    string phone,
                    string address,
                    UserType userType)
                {
                        User user = await _userHelper.GetUserAsync(email);
                        if (user == null)
                        {
                                user = new User
                                {
                                        FirstName = firstName,
                                        LastName = lastName,
                                        Email = email,
                                        UserName = email,
                                        PhoneNumber = phone,
                                        Address = address,
                                        Document = document,
                                        DocumentType = _context.DocumentTypes.OrderBy(o => Guid.NewGuid()).First(),
                                        UserType = userType
                                        
                                };

                                await _userHelper.AddUserAsync(user, "123456");
                                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                        }

                        return user;
                }


                //private async Task CheckAuthorAsync()
                //{
                //        if (!_context.Authors.Any())
                //        {
                //                _context.Authors.Add(new Author { Name = "Gabriel García Márquez" });
                //                _context.Authors.Add(new Author { Name = "Edgar Allan Poe" });
                //                _context.Authors.Add(new Author { Name = "William Shakespeare" });
                //               _context.Authors.Add(new Author { Name = "Jane Austen" });
                //                _context.Authors.Add(new Author { Name = "Miguel de Cervantes" });
                //                _context.Authors.Add(new Author { Name = "Mario Vargas Llosa" });
                //               await _context.SaveChangesAsync();
                //        }
                //}

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

                private async Task CheckBooksAsync()
                {
                        if (!_context.Books.Any())
                        {
                                await AddBookAsync("000000001", "La Biblia", 3000, 3);
                                await AddBookAsync("000000002", "El Alquimista", 100, 3);
                                await AddBookAsync("000000003", "El Señor de los Anillos", 3000, 3);
                                await AddBookAsync("000000004", "El Código da Vinci", 920, 2);
                                await AddBookAsync("000000005", "Lo que el viento se llevó", 100, 0);
                                await AddBookAsync("000000006", "Piense y hágase rico", 435, 9);
                                await AddBookAsync("000000007", "El diario de Ana Frank", 92, 3);
                                await AddBookAsync("000000008", "100 años de Soledad", 1003, 2);
                                await AddBookAsync("000000009", "La casa de los espíritus", 954, 1);
                                await AddBookAsync("000000010", "Preludio a la fundación", 543, 0);
                                await AddBookAsync("000000011", "La comedia humana", 534, 2);
                                await AddBookAsync("000000012", "El extranjero", 879, 6);
                                await AddBookAsync("000000013", "Drácula", 743, 5);
                                await AddBookAsync("000000014", "La divina comedia", 452, 3);
                                await AddBookAsync("000000015", "Don Quijote de la mancha", 535, 4);
                                await AddBookAsync("000000016", "Poesías completas", 564, 5);
                                await AddBookAsync("000000017", "La montaña mágica", 644, 6);
                                await AddBookAsync("000000018", "La naranja mecánica", 757, 9);
                                await AddBookAsync("000000019", "La odiseai", 456, 10);
                                await AddBookAsync("000000020", "El Principito", 1000, 12);
                                await AddBookAsync("000000021", "En busca del tiempo perdido", 646, 3);
                                await AddBookAsync("000000022", "Rayuela", 890, 2);
                                await AddBookAsync("000000023", "El laberinto de la soledad", 532, 1);
                                await AddBookAsync("000000024", "La guerra del fin del mundo", 646, 3);
                                await AddBookAsync("000000025", "El túnel", 345, 1);
                                await AddBookAsync("000000026", "El tambor de hojalata", 345, 5);
                                await AddBookAsync("000000027", "Tres tristes tigres", 234, 6);
                                await AddBookAsync("000000028", "A sangre fría", 234, 1);
                                await AddBookAsync("000000029", "Tala", 123, 2);
                                await AddBookAsync("000000030", "El nombre de la rosa", 546, 1);
                                await _context.SaveChangesAsync();
                        }
                }

                private async Task AddBookAsync(string serial, string name, int numPages, int numCopies)
                {

                        Book book = new()
                        {
                                Serial = serial,
                                Name = name,
                                Description = name,
                                NumPages = numPages,
                                NumCopies = numCopies,
                                EditionDate = DateTime.Now,
                                IsActive = true,
                                IsStarred = true,
                                Category = _context.Categories.OrderBy(o => Guid.NewGuid()).First(),
                                Editorial = _context.Editoriales.OrderBy(o => Guid.NewGuid()).First()
                        };

                        _context.Books.Add(book);
                }
        }
}
