using Microsoft.EntityFrameworkCore;
using PostnTagWebAPI.Data;
using PostnTagWebAPI.Interfaces;
using PostnTagWebAPI.Models;

namespace PostnTagWebAPI.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _context;

        public PostRepository(DataContext context)
        {
            _context = context;
        }

        public bool PostExists(int postId)
        {
            return _context.Posts.Any(p => p.Id == postId);
        }

        public bool CreatePost(int tagId, Post post)
        {
            var tag = _context.Tags.Where(a => a.Id == tagId).FirstOrDefault();

            var postTag = new PostTag()
            {
                Tag = tag,
                Post = post,
            };

            _context.Add(postTag);

            _context.Add(post);

            return Save();
        }

        public ICollection<Post> GetPosts()
        {
            return _context.Posts.OrderBy(p => p.Id).ToList();
        }

        public Post GetPost(int id)
        {
            return _context.Posts.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Post> GetPostByTitle(string title)
        {
            string pattern = $"%{title}%";
            return _context.Posts.Where(c => EF.Functions.Like(c.Title, pattern)).ToList();
        }

        public ICollection<Post> GetPostByContent(string content)
        {
            string pattern = $"%{content}%";
            return _context.Posts.Where(c => EF.Functions.Like(c.Content, pattern)).ToList();
        }

        public ICollection<Tag> GetTagByPostId(int postId)
        {
            return _context.PostTags.Where(e => e.PostId == postId).Select(c => c.Tag).ToList();
        }

        public bool UpdatePost(int postId, Post post)
        {
            var updatePost = _context.Posts.First(b => b.Id == postId);
            updatePost.Title = post.Title;
            updatePost.Content = post.Content;

            return Save();          
        }

        public bool DeletePost(Post post)
        {
            _context.Remove(post);
            return Save();
        }               

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }        
    }
}
