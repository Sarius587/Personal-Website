using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalWebsite.GithubService;

namespace PersonalWebsite.Pages
{
    public class ProjectsModel : PageModel
    {
        private readonly ILogger<ProjectsModel> _logger;
        private readonly GithubRepositoryContext _context;

        public IList<GithubRepository> Repositories { get; set; }

        public ProjectsModel(ILogger<ProjectsModel> logger, GithubRepositoryContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Repositories = await _context.Repos.ToListAsync();
        }
    }
}