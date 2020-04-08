using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PersonalWebsite.GithubService;
using PersonalWebsite.Util;

namespace PersonalWebsite.Areas.Projects.Pages
{
    [Authorize(Policy = "RequireAdminRole")]
    public class ProjectEditModel : PageModel
    {

        public GithubRepository SelectedProject;

        [BindProperty]
        public CustomExperienceUpload Upload { get; set; }

        private GithubRepositoryContext _context;
        private readonly string[] _permittedExtensions = { ".png", ".jpg", ".jpeg", ".svg" };
        private readonly long _fileSizeLimit = 2097152;

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
            Upload = new CustomExperienceUpload { AdditionalRepositoryData = SelectedProject.AdditionalRepositoryData };

            if (SelectedProject == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUploadAsync(int id = -1)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            AdditionalRepositoryData dataToChange = await _context.AdditionalData.FirstOrDefaultAsync(a => a.RepositoryId == id);
            dataToChange.CustomExperience = Upload.AdditionalRepositoryData.CustomExperience;
            dataToChange.TextFormat = Upload.AdditionalRepositoryData.TextFormat;
            dataToChange.LastEdit = DateTime.UtcNow;
            
            if (string.IsNullOrWhiteSpace(dataToChange.CustomExperience))
            {
                dataToChange.CustomExperience = null;
            }

            await _context.SaveChangesAsync();

            foreach (var image in Upload.PostImages)
            {
                var fileContent = await FileHelpers.ProcessFormFile<CustomExperienceUpload>(image, ModelState, _permittedExtensions, _fileSizeLimit);

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                // Maybe perform virus scan

                
            }

            return LocalRedirect("~/Projects/AdminDashboard");
        }
    }

    public class CustomExperienceUpload
    {
        [Required]
        public AdditionalRepositoryData AdditionalRepositoryData { get; set; }

        public List<IFormFile> PostImages { get; set; }
    }
}
