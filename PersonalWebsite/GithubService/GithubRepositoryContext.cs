using Microsoft.AspNetCore.Html;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.GithubService
{
    public class GithubRepositoryContext : DbContext
    {

        public GithubRepositoryContext(DbContextOptions<GithubRepositoryContext> options)
            : base(options)
        {

        }

        public DbSet<GithubRepository> Repos { get; set; }
    }

    public class GithubRepository
    {
        public int Id { get; set; }
        public int GithubRepoId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Readme { get; set; }

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
                   GithubRepoId == repository.GithubRepoId &&
                   Name == repository.Name &&
                   Description == repository.Description &&
                   Url == repository.Url &&
                   Readme == repository.Readme;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GithubRepoId, Name, Description, Url, Readme);
        }
    }
}
