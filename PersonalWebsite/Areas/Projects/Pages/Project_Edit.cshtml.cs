using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
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

        // File limitations
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

            // Retrieve project from database and set the bound property accordingly
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

            // Update model
            AdditionalRepositoryData dataToChange = await _context.AdditionalData.FirstOrDefaultAsync(a => a.RepositoryId == id);
            if (dataToChange.CustomExperienceImages == null)
            {
                dataToChange.CustomExperienceImages = new List<CustomExperienceImage>();
            }

            if (Upload.PostImages != null)
            {
                foreach (var image in Upload.PostImages)
                {
                    // Process uploaded file (check for limitations, validate header bytes, etc)
                    var fileContent = await FileHelpers.ProcessFormFile<CustomExperienceUpload>(image, ModelState, _permittedExtensions, _fileSizeLimit);

                    if (!ModelState.IsValid)
                    {
                        return Page();
                    }

                    // Maybe perform virus scan

                    dataToChange.CustomExperienceImages.Add(new CustomExperienceImage { Content = fileContent });
                }
            }

            await _context.SaveChangesAsync();

            Regex findImages = new Regex("<img.*?/>", RegexOptions.Singleline | RegexOptions.Compiled);
            var match = findImages.Match(Upload.AdditionalRepositoryData.CustomExperience);

            while (match.Success)
            {
                int index = match.Value.IndexOf("src=\"");

                match = match.NextMatch();
            }

            
            dataToChange.CustomExperience = Upload.AdditionalRepositoryData.CustomExperience;
            dataToChange.TextFormat = Upload.AdditionalRepositoryData.TextFormat;
            dataToChange.LastEdit = DateTime.UtcNow;
            
            // Set custom experience to null if input is blank
            if (string.IsNullOrWhiteSpace(dataToChange.CustomExperience))
            {
                dataToChange.CustomExperience = null;
            }

            await _context.SaveChangesAsync();

            return LocalRedirect("~/Projects/AdminDashboard");
        }
    }


    // Model class for the form 
    public class CustomExperienceUpload
    {
        [Required]
        public AdditionalRepositoryData AdditionalRepositoryData { get; set; }

        public List<IFormFile> PostImages { get; set; }
    }
}
