using Microsoft.AspNetCore.Mvc;
using SimpleBlogApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBlogApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly List<Post> _posts = new List<Post>
        {
            new Post { Id = 1, Title = "First Post", Content = "This is the content of the first post.", Author = "Author One" },
            new Post { Id = 2, Title = "Second Post", Content = "This is the content of the second post.", Author = "Author Two" },
            new Post { Id = 3, Title = "Third Post", Content = "This is the content of the third post.", Author = "Author Three" }
        };

        // GET: /Blog
        public IActionResult Index()
        {
            return View(_posts);
        }

        // GET: /Blog/Details/1
        public IActionResult Details(int id)
        {
            var post = _posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
    }
}