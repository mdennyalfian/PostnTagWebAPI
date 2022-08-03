
namespace PostnTagWebAPI.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public ICollection<PostTag> PostTags { get; set; }

    }
}
