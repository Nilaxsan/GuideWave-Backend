using AutoMapper;
using GuideWave.DTO.Review;
using GuideWave.Models;
using GuideWave.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace GuideWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;

        private readonly IMapper _mapper;

        private readonly ILogger<ReviewController> _logger;

        public ReviewController(IReviewRepository reviewRepository,IMapper mapper,ILogger<ReviewController> logger)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAll()
        {
            var reviews = await _reviewRepository.GetAll();  

            var reviewsDto = _mapper.Map<List<ReviewDto>>(reviews);

            if (reviews == null)
            {
                return NoContent();
            }

            return Ok(reviewsDto);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<ReviewDto>> GetById(int id)
        {


            var review = await _reviewRepository.Get(id);



            if (review == null)
            {
                _logger.LogError($"Error when get from the id:{id}");
                return NoContent();
            }
            var countryDto = _mapper.Map<ReviewDto>(review);

            return Ok(countryDto);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<ActionResult<CreateReviewDto>> Create([FromBody] CreateReviewDto reviewDto)
        {
           

            var review = _mapper.Map<Review>(reviewDto);  
            await _reviewRepository.Create(review);
            return CreatedAtAction("GetById", new { id = review.ReviewId }, review);

        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Review>> Update(int id, [FromBody] ReviewDto reviewDto)
        {
            if (reviewDto == null || id != reviewDto.ReviewId)
            {
                return BadRequest();
            }
            var review = _mapper.Map<Review>(reviewDto);

            await _reviewRepository.Update(review);
            return NoContent();

        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult> DeleteById(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var review = await _reviewRepository.Get(id);


            if (review == null)
            {
                return NotFound();
            }
            await _reviewRepository.Delete(review);
            return NoContent();
        }

    }
}
