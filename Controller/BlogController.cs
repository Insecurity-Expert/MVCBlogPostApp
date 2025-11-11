using Microsoft.AspNetCore.Mvc;
using SimpleBlogApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SimpleBlogApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public BlogController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        private Post LoadPostMetadata(int id)
        {
            var post = new Post { Id = id };
            var filePath = Path.Combine(_hostEnvironment.ContentRootPath, $"Views/Blog/Posts/Post{id}.cshtml");

            if (System.IO.File.Exists(filePath))
            {
                var content = System.IO.File.ReadAllText(filePath);
                
                // Extract metadata from the Razor file
                post.Title = ExtractValue(content, "postTitle");
                post.Author = ExtractValue(content, "postAuthor");
                post.ImageUrl = ExtractValue(content, "postImageUrl");
                post.Summary = ExtractSummary(content);
            }

            post.PublishedDate = DateTime.UtcNow.AddHours(8);
            return post;
        }

        private string ExtractValue(string content, string variable)
        {
            var pattern = $@"var\s+{variable}\s*=\s*""([^""]*)"";";
            var match = Regex.Match(content, pattern);
            return match.Success ? match.Groups[1].Value : "";
        }

        private string ExtractSummary(string content)
        {
            var pattern = @"<h3>Summary</h3>\s*<p>([^<]*)</p>";
            var match = Regex.Match(content, pattern);
            return match.Success ? match.Groups[1].Value.Trim() : "";
        }

        public IActionResult Index()
        {
            var posts = new List<Post>
            {
                LoadPostMetadata(1),
                LoadPostMetadata(2)
            };
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var details = LoadPostMetadata(id);

            if (string.IsNullOrEmpty(details.Title))
            {
                return NotFound();
            }

            return View(details);
        }
    }
}
