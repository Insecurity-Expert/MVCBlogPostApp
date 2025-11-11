namespace SimpleBlogApp.Models
{
    /// <summary>
    /// Represents a blog post with metadata and content.
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Gets or sets the unique identifier for the post.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the blog post.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a brief summary of the blog post displayed on the index page.
        /// </summary>
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the full content of the blog post (not typically used in this implementation).
        /// Content is rendered from the individual post Razor files.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the author name of the blog post.
        /// </summary>
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the publication date and time of the blog post.
        /// </summary>
        public DateTime PublishedDate { get; set; }

        /// <summary>
        /// Gets or sets the filename of the featured image displayed with the post.
        /// Images are stored in wwwroot/images/ folder.
        /// </summary>
        public string? ImageUrl { get; set; }
    }
}
