namespace PostnTagWebAPI.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public ICollection<PostTag> PostTags { get; set; }
    }
}
