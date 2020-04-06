using System;
using System.Collections.Generic;
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
    public class AdminDashboardModel : PageModel
    {

        public IList<GithubRepository> Repositories;

        private GithubRepositoryContext _context;

        public AdminDashboardModel(GithubRepositoryContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Repositories = await _context.Repositories
                .Include(r => r.AdditionalRepositoryData)
                .ToListAsync();
        }
    }
}
