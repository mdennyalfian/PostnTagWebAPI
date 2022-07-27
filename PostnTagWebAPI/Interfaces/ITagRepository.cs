using PostnTagWebAPI.Models;

namespace PostnTagWebAPI.Interfaces
{
    public interface ITagRepository
    {
        ICollection<Tag> GetTags();
        Tag GetTag(int id);
        Tag GetTag(string label);
        ICollection<Tag> GetTagByLabel(string label);
        ICollection<Post> GetPostByTagId(int tagId);
        bool TagExists(int id);
        bool CreateTag(Tag tag);
        bool UpdateTag(int tagId, Tag tag);
        bool DeleteTag(Tag tag);
        bool Save();
    }
}
