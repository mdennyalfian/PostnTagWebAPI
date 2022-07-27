using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PostnTagWebAPI.Dto;
using PostnTagWebAPI.Interfaces;
using PostnTagWebAPI.Models;

namespace PostnTagWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagController(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        public IActionResult GetTags()
        {
            var tags = _mapper.Map<List<TagDto>>(_tagRepository.GetTags());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tags);
        }

        [HttpGet("id/{tagId}")]
        [ProducesResponseType(200, Type = typeof(Tag))]
        [ProducesResponseType(400)]
        public IActionResult GetTagById(int tagId)
        {
            if (!_tagRepository.TagExists(tagId))
                return NotFound();

            var tag = _mapper.Map<TagDto>(_tagRepository.GetTag(tagId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tag);
        }

        [HttpGet("label{label}")]
        [ProducesResponseType(200, Type = typeof(Tag))]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        public IActionResult GetTagByLabel(string label)
        {
            var tag = _mapper.Map<List<TagDto>>(
                _tagRepository.GetTagByLabel(label));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tag);
        }

        [HttpGet("post/{tagId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Post>))]
        [ProducesResponseType(400)]

        public IActionResult GetPostByTagId(int tagId)
        {
            var posts = _mapper.Map<List<PostDto>>(
                _tagRepository.GetPostByTagId(tagId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(posts);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTag([FromBody] TagDtoCreate tagCreate)
        {
            if (tagCreate == null)
                return BadRequest(ModelState);

            var tag = _tagRepository.GetTags().FirstOrDefault(c => c.Label.Replace(" ",string.Empty).ToUpper() == tagCreate.Label.Replace(" ", string.Empty).ToUpper());

            if (tag != null)
            {
                ModelState.AddModelError("", "Tag already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tagMap = _mapper.Map<Tag>(tagCreate);

            if (!_tagRepository.CreateTag(tagMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving Tag");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{tagId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTag(int tagId, [FromBody] TagDtoCreate updatedTag)
        {
            if (updatedTag == null)
                return BadRequest(ModelState);

            if (!_tagRepository.TagExists(tagId))
                return NotFound();

            var tag = _tagRepository.GetTags().FirstOrDefault(c => c.Label.Replace(" ", string.Empty).ToUpper() == updatedTag.Label.Replace(" ", string.Empty).ToUpper());

            if (tag != null)
            {
                ModelState.AddModelError("", "Tag already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var tagMap = _mapper.Map<Tag>(updatedTag);

            if (!_tagRepository.UpdateTag(tagId, tagMap))
            {
                ModelState.AddModelError("", "Something went wrong updating tag");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("{tagId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTag(int tagId)
        {
            if (!_tagRepository.TagExists(tagId))
                return NotFound();

            var tagToDelete = _tagRepository.GetTag(tagId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_tagRepository.DeleteTag(tagToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting tag");
            }

            return Ok("Successfully deleted");
        }
    }
}
