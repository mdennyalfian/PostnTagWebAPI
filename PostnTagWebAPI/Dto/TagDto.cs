using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace PostnTagWebAPI.Dto
{
    public class TagDto
    {
        public int Id { get; set; }

        public string Label { get; set; }

    }
}
