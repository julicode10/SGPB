﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGPB.Web.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGPB.Web.Helpers
{
        public class CombosHelper : ICombosHelper
        {
                private readonly ApplicationDbContext _context;

                public CombosHelper(ApplicationDbContext context)
                {
                        _context = context;
                }

                public IEnumerable<SelectListItem> GetComboCategories()
                {
                        List<SelectListItem> list = _context.Categories.Select(t => new SelectListItem
                        {
                                Text = t.Name,
                                Value = $"{t.Id}"
                        })
                            .OrderBy(t => t.Text)
                            .ToList();

                        list.Insert(0, new SelectListItem
                        {
                                Text = "[Seleccione una categoría...]",
                                Value = "0"
                        });

                        return list;
                }


                public IEnumerable<SelectListItem> GetComboEditoriales()
                {
                        List<SelectListItem> list = _context.Editoriales.Select(e => new SelectListItem
                        {
                                Text = e.Name,
                                Value = $"{e.Id}"
                        })
                            .OrderBy(e => e.Text)
                            .ToList();

                        list.Insert(0, new SelectListItem
                        {
                                Text = "[Seleccione una editorial...]",
                                Value = "0"
                        });

                        return list;
                }

                public async Task<IEnumerable<SelectListItem>> GetComboDocumentTypesAsync()
                {
                        List<SelectListItem> list = await _context.DocumentTypes.Select(x => new SelectListItem
                        {
                                Text = x.Name,
                                Value = $"{x.Id}"
                        })
                            .OrderBy(x => x.Text)
                            .ToListAsync();

                        list.Insert(0, new SelectListItem
                        {
                                Text = "[Seleccione un tipo de documento...]",
                                Value = "0"
                        });

                        return list;
                }

        }
}
