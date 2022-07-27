using PostnTagWebAPI.Models;

namespace PostnTagWebAPI.Interfaces
{
    public interface IPostRepository
    {
        ICollection<Post> GetPosts();
        Post GetPost(int id);
        Post GetPost(string title);
        ICollection<Tag> GetTagByPostId(int postId);
        bool PostExists(int postId);
        bool CreatePost(int tagId, Post post);
        bool UpdatePost(int postId, int tagId, Post post);
        bool DeletePost(Post post);
        bool Save();
    }
}
