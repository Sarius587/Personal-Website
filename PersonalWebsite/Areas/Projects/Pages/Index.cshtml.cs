using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalWebsite.GithubService;

namespace PersonalWebsite.Areas.Projects.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly GithubRepositoryContext _context;

        public IList<GithubRepository> Repositories { get; set; }

        public IndexModel(ILogger<IndexModel> logger, GithubRepositoryContext context)
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