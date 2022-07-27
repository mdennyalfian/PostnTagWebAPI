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

        public Post GetPost(string title)
        {
            return _context.Posts.Where(p => p.Title == title).FirstOrDefault();
        }        

        public ICollection<Tag> GetTagByPostId(int postId)
        {
            return _context.PostTags.Where(e => e.PostId == postId).Select(c => c.Tag).ToList();
        }

        public bool UpdatePost(int postId, int tagId, Post post)
        {
            //var postNew = new Post()
            //{
            //    Id = postId,
            //    Title = post.Title,
            //    Content = post.Content,
            //};

            var postNew = _context.Posts.Where(a => a.Id == postId).FirstOrDefault();

            if (postId > 0)
            {
                if (postNew != null)
                {
                    postNew.Title = post.Title;
                    postNew.Content = post.Content;
                }
            }
                       

            if (tagId > 0)
            {
                var tag = _context.Tags.Where(a => a.Id == tagId).FirstOrDefault();
                var postTag = _context.PostTags.Where(a => a.PostId == postId).FirstOrDefault();
                postTag.Post = postNew;
                postTag.Tag = tag;
            }            
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
