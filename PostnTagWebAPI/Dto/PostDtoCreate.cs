namespace PostnTagWebAPI.Dto
{
    public class PostDtoCreate
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<TagDtoCreate> Tags { get; set; }
    }
}
