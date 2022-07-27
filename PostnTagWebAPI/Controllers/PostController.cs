using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PostnTagWebAPI.Dto;
using PostnTagWebAPI.Interfaces;
using PostnTagWebAPI.Models;

namespace PostnTagWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly IPostRepository _postrepository;
        private readonly IMapper _mapper;
        private readonly ITagRepository _tagRepository;

        public PostController(IPostRepository postrepository, IMapper mapper, ITagRepository tagRepository)
        {
            _postrepository = postrepository;
            _mapper = mapper;
            _tagRepository = tagRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Post>))]
        public IActionResult GetPosts()
        {
            var posts = _mapper.Map<List<PostDto>>(_postrepository.GetPosts());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(posts);
        }

        [HttpGet("id/{postId}")]
        [ProducesResponseType(200, Type = typeof(Post))]
        [ProducesResponseType(400)]

        public IActionResult GetPosById(int postId)
        {
            if (!_postrepository.PostExists(postId))
                return NotFound();

            var posts = _mapper.Map<PostDto>(_postrepository.GetPost(postId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(posts);
        }

        [HttpGet("title/{title}")]
        [ProducesResponseType(200, Type = typeof(Post))]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]

        public IActionResult GetPostByTitle(string title)
        {
            var posts = _mapper.Map<PostDto>(_postrepository.GetPost(title));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(posts);
        }

        [HttpGet("tag/{postId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tag>))]
        [ProducesResponseType(400)]

        public IActionResult GetTagByPostId(int postId)
        {
            var tags = _mapper.Map<List<TagDto>>(
                _postrepository.GetTagByPostId(postId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tags);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePost([FromQuery] int tagId, [FromBody] PostDtoCreate postCreate)
        {
            if (postCreate == null)
                return BadRequest(ModelState);

            var posts = _postrepository.GetPosts().FirstOrDefault(c => c.Title.Trim().ToUpper() == postCreate.Title.TrimEnd().ToUpper());

            if (!_tagRepository.TagExists(tagId))
                return NotFound();

            if (posts != null)
            {
                ModelState.AddModelError("", "Title already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var postMap = _mapper.Map<Post>(postCreate);

            if (!_postrepository.CreatePost(tagId, postMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{postId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePost(int postId,
            [FromQuery] int tagId, [FromBody] PostDtoCreate updatedPost)
        {
            if (updatedPost == null)
                return BadRequest(ModelState);

            if (!_postrepository.PostExists(postId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var postMap = _mapper.Map<Post>(updatedPost);

            if (!_postrepository.UpdatePost(postId, tagId, postMap))
            {
                ModelState.AddModelError("", "Something went wrong updating Post");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("{postId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePost(int postId)
        {
            if (!_postrepository.PostExists(postId))
                return NotFound();

            var postToDelete = _postrepository.GetPost(postId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            if (!_postrepository.DeletePost(postToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting post");
            }

            return Ok("Successfully deleted");
        }
    }
}
