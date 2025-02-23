﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SGPB.Web.Data;
using SGPB.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SGPB.Web.Controllers
{
        public class HomeController : Controller
        {
                private readonly ILogger<HomeController> _logger;
                private readonly ApplicationDbContext _context;

                public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
                {
                        _logger = logger;
                        _context = context;
                }

                public async Task<IActionResult> Index()
                {
                                            
                        return View(await _context.Books
                                    .Include(b => b.Category)
                                    .Include(b => b.Editorial)
                                    .Include(b => b.BookImages)
                                    .Where(b => b.IsActive)
                                    .OrderByDescending(b => b.Id)
                                    .ToListAsync());
                }

                public IActionResult Privacy()
                {
                        return View();
                }

                [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
                public IActionResult Error()
                {
                        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                [Route("error/404")]
                public IActionResult Error404()
                {
                        return View();
                }

        }
}
