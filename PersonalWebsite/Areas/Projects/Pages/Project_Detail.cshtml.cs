using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using PersonalWebsite.GithubService;

namespace PersonalWebsite.Areas.Projects.Pages
{
    public class ProjectDetail : PageModel
    {

        private GithubRepositoryContext _context;

        public GithubRepository SelectedProject { get; set; }

        public ProjectDetail(GithubRepositoryContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(int id = -1)
        {
            if (id == -1)
            {
                return NotFound();
            }

            SelectedProject = _context.Repos.SingleOrDefault(r => r.Id == id);

            if (SelectedProject == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
