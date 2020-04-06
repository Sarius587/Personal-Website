using Microsoft.AspNetCore.Html;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using PersonalWebsite.Util;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PersonalWebsite.GithubService
{
    public class GithubRepositoryContext : DbContext
    {

        public GithubRepositoryContext(DbContextOptions<GithubRepositoryContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdditionalRepositoryData>()
                .Property(e => e.TextFormat)
                .HasConversion<int>();
        }

        public DbSet<GithubRepository> Repositories { get; set; }

        public DbSet<AdditionalRepositoryData> AdditionalData { get; set; }
    }

    public class GithubRepository
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RepositoryId { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(300)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Url { get; set; }

        public string Readme { get; set; }

        public int AdditionalRepositoryDataId { get; set; }

        public AdditionalRepositoryData AdditionalRepositoryData { get; set; }


        public HtmlString ReadmeHtml
        {
            get
            {
                return new HtmlString(Readme);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is GithubRepository repository &&
                   RepositoryId == repository.RepositoryId &&
                   Name == repository.Name &&
                   Description == repository.Description &&
                   Url == repository.Url &&
                   Readme == repository.Readme;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RepositoryId, Name, Description, Url, Readme);
        }
    }

    public class AdditionalRepositoryData
    {
        public int Id { get; set; }

        public int RepositoryId { get; set; }

        public string CustomExperience { get; set; }

        public ExperienceTextFormat TextFormat { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime LastEdit { get; set; }

        public HtmlString CustomExperienceHtml
        {
            get
            {
                if (TextFormat == ExperienceTextFormat.Markdown)
                {
                    return MarkdownParser.Parse(CustomExperience);
                }
                else
                {
                    return new HtmlString(CustomExperience);
                }
            }
        }
    }

    public enum ExperienceTextFormat
    {
        Markdown,
        Html
    }
}
