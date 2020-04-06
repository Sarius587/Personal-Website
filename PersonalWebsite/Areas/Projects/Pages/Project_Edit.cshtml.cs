using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonalWebsite.GithubService;

namespace PersonalWebsite.Areas.Projects.Pages
{
    [Authorize(Policy = "RequireAdminRole")]
    public class ProjectEditModel : PageModel
    {

        public GithubRepository SelectedProject;

        [BindProperty]
        public AdditionalRepositoryData Data { get; set; }

        private GithubRepositoryContext _context;

        public ProjectEditModel(GithubRepositoryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id = -1)
        {
            if (id == -1)
            {
                return NotFound();
            }

            SelectedProject = await _context.Repositories.Include(r => r.AdditionalRepositoryData).FirstOrDefaultAsync(r => r.RepositoryId == id);
            Data = SelectedProject.AdditionalRepositoryData;

            if (SelectedProject == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id = -1)
        {
            AdditionalRepositoryData dataToChange = await _context.AdditionalData.FirstOrDefaultAsync(a => a.RepositoryId == id);

            if (await TryUpdateModelAsync<AdditionalRepositoryData>(dataToChange, "data", a => a.CustomExperience, a => a.TextFormat))
            {
                dataToChange.LastEdit = DateTime.UtcNow;

                if (String.IsNullOrWhiteSpace(dataToChange.CustomExperience))
                {
                    dataToChange.CustomExperience = null;
                }

                await _context.SaveChangesAsync();
                return LocalRedirect("~/Projects/AdminDashboard");
            }

            return Page();
        }
    }
}
