using Microsoft.AspNetCore.Mvc;
using SimpleBlogApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace SimpleBlogApp.Controllers
{
    /// <summary>
    /// BlogController handles all blog post operations including listing and displaying individual posts.
    /// </summary>
    public class BlogController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private const int PostsPerPage = 10;
        private const string PostFileTemplate = "Views/Blog/Posts/Post{0}.cshtml";

        /// <summary>
        /// Initializes a new instance of the BlogController.
        /// </summary>
        /// <param name="hostEnvironment">The web host environment</param>
        public BlogController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
        }

        /// <summary>
        /// Loads post metadata from the post Razor file.
        /// </summary>
        /// <param name="id">The post ID</param>
        /// <returns>A Post object with loaded metadata</returns>
        private Post LoadPostMetadata(int id)
        {
            var post = new Post { Id = id };
            var filePath = Path.Combine(_hostEnvironment.ContentRootPath, string.Format(PostFileTemplate, id));

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    var fileContent = System.IO.File.ReadAllText(filePath);
                    post.Title = ExtractMetadataValue(fileContent, "postTitle");
                    post.Author = ExtractMetadataValue(fileContent, "postAuthor");
                    post.ImageUrl = ExtractMetadataValue(fileContent, "postImageUrl");
                    post.Summary = ExtractSummary(fileContent);
                }
                catch (Exception ex)
                {
                    // Log error and continue with empty metadata
                    System.Diagnostics.Debug.WriteLine($"Error loading post metadata: {ex.Message}");
                }
            }

            post.PublishedDate = DateTime.UtcNow.AddHours(8);
            return post;
        }

        /// <summary>
        /// Extracts metadata value from Razor file using regex pattern matching.
        /// </summary>
        /// <param name="content">The file content</param>
        /// <param name="variable">The variable name to extract</param>
        /// <returns>The extracted value or empty string if not found</returns>
        private string ExtractMetadataValue(string content, string variable)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(variable))
                return string.Empty;

            var pattern = $@"var\s+{variable}\s*=\s*""([^""]*)"";";
            var match = Regex.Match(content, pattern);
            return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
        }

        /// <summary>
        /// Extracts the summary section from the post content.
        /// </summary>
        /// <param name="content">The file content</param>
        /// <returns>The extracted summary or empty string if not found</returns>
        private string ExtractSummary(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            var pattern = @"<h3>Summary</h3>\s*<p>([^<]*)</p>";
            var match = Regex.Match(content, pattern);
            return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
        }

        /// <summary>
        /// Index action: Displays a list of all blog posts.
        /// </summary>
        /// <returns>View with list of posts</returns>
        public IActionResult Index()
        {
            var posts = new List<Post>
            {
                LoadPostMetadata(1),
                LoadPostMetadata(2)
            };

            return View(posts);
        }

        /// <summary>
        /// Details action: Displays a single blog post by ID.
        /// </summary>
        /// <param name="id">The post ID</param>
        /// <returns>View with post details or 404 if not found</returns>
        public IActionResult Details(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid post ID");

            var post = LoadPostMetadata(id);

            if (string.IsNullOrEmpty(post.Title))
                return NotFound();

            return View(post);
        }
    }
}

