using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PersonalWebsite.Areas.Projects.Pages
{
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminDashboardModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
