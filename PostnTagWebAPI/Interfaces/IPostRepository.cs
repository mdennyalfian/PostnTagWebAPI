using PostnTagWebAPI.Models;

namespace PostnTagWebAPI.Interfaces
{
    public interface IPostRepository
    {
        IQueryable GetAllPosts();
        ICollection<Post> GetPosts();
        Post GetPost(int id);
        ICollection<Post> GetPostByTitle(string title);
        ICollection<Post> GetPostByContent(string content);
        ICollection<Tag> GetTagByPostId(int postId);
        bool PostExists(int postId);
        bool CreatePost(int tagId, Post post);
        bool CreateNewPost(Post post);
        bool UpdatePost(int postId, Post post);
        bool DeletePost(Post post);
        bool Save();
    }
}
