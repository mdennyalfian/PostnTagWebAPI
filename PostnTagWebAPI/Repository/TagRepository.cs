using Microsoft.EntityFrameworkCore;
using PostnTagWebAPI.Data;
using PostnTagWebAPI.Interfaces;
using PostnTagWebAPI.Models;

namespace PostnTagWebAPI.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly DataContext _context;

        public TagRepository(DataContext context)
        {
            _context = context;
        }

        public bool TagExists(int id)
        {
            return _context.Tags.Any(c => c.Id == id);
        }

        public bool CreateTag(Tag tag)
        {
            var getlabel = tag.Label.Replace(" ", String.Empty);

            var tagNew = new Tag()
            {
                Label = getlabel
            };

            _context.Add(tagNew);
            return Save();
        }

        public ICollection<Tag> GetTags()
        {
            return _context.Tags.ToList();
        }

        public Tag GetTag(int id)
        {
            return _context.Tags.Where(e => e.Id == id).FirstOrDefault();
        }

        public Tag GetTag(string label)
        {
            return _context.Tags.Where(p => p.Label == label).FirstOrDefault();
        }

        public ICollection<Tag> GetTagByLabel(string label)
        {
            string pattern = $"%{label}%";
            return _context.Tags.Where(c => EF.Functions.Like(c.Label, pattern)).ToList(); // Version B

            //return _context.Tags.Where(e => e.Label == label).ToList();
        }

        public ICollection<Post> GetPostByTagId(int tagId)
        {
            return _context.PostTags.Where(e => e.TagId == tagId).Select(c => c.Post).ToList();
        }

        public bool UpdateTag(int tagId, Tag tag)
        {
            var getlabel = tag.Label.Replace(" ", String.Empty);

            var updateTagLabel = _context.Tags.First(b => b.Id == tagId);
            updateTagLabel.Label = getlabel;

            return Save();
        }

        public bool DeleteTag(Tag tag)
        {
            _context.Remove(tag);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }        
    }
}
