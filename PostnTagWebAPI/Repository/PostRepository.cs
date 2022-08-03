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

        public bool CreateNewPost(Post post)
        {
            var postlist = post.Tags.ToList();

            //var getpost = new Post()
            //{
            //    Id = post.Id,
            //    Title = post.Title,
            //    Content = post.Content,
            //    Tags = post.Tags,
            //    PostTags = post.PostTags
            //};

            //var getpostlist = getpost.Tags.ToList();

            //foreach (var getlabel in getpostlist)
            //{
            //    var existTag = _context.Tags.Any(c => c.Label.Replace(" ", string.Empty).ToUpper() == getlabel.Label.Replace(" ", string.Empty).ToUpper());

            //    if(existTag)
            //    {
            //        getpost.Tags.Remove(getlabel);
            //    }
            //}

            _context.Add(post);
            _context.SaveChanges();

            foreach (var labels in postlist)
            {               
                var tag = _context.Tags.Where(s => s.Label == labels.Label.ToString()).FirstOrDefault();

                var postTag = new PostTag()
                {
                    Tag = tag,
                    Post = post,
                };

                _context.Add(postTag);
                _context.SaveChanges();
            }

            return true;
        }

        public IQueryable GetAllPosts()
        {
            var data = _context.Posts.Include(x => x.PostTags).ThenInclude(x => x.Tag)
                .Select(x => new
                {
                    PostId = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    Tags = x.PostTags.Select(k => k.Tag.Label).ToList()
                });

            return data;
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
