using AutoMapper;
using GuideWave.DTO.Guides;
using GuideWave.DTO.Places;
using GuideWave.Models;
using GuideWave.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace GuideWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private readonly IPlaceRepository _placeRepository;

        private readonly IMapper _mapper;

        public PlaceController(IPlaceRepository placeRepository, IMapper mapper)
        {
            _placeRepository = placeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<IEnumerable<PlaceDto>>> GetAll()
        {
            var places = await _placeRepository.GetAll();

            var placesDto = _mapper.Map<List<PlaceDto>>(places);

            if (placesDto == null)
            {
                return NoContent();
            }

            return Ok(placesDto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<PlaceDto>> GetById(int id)
        {


            var places = await _placeRepository.Get(id);


            var placesDto = _mapper.Map<PlaceDto>(places);
            if (places == null)
            {
                return NoContent();
            }
            return Ok(placesDto);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<ActionResult<CreatePlaceDto>> Create([FromBody] CreatePlaceDto createplaceDto)
        {
            var result = _placeRepository.IsRecordsExists(x => x.PlaceName == createplaceDto.PlaceName);
            if (result)
            {

                return Conflict("Place is  already exists in database  for you");
            }


            var place = _mapper.Map<Place>(createplaceDto);


            await _placeRepository.Create(place);
            return CreatedAtAction("GetById", new { id = place.PlaceId }, place );

        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Place>> Update(int id, [FromBody] PlaceDto placeDto)
        {
            if (placeDto == null || id != placeDto.PlaceId)
            {
                return BadRequest();
            }

            var place = _mapper.Map<Place>(placeDto);

            await _placeRepository.Update(place);
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

            var place = await _placeRepository.Get(id);


            if (place == null)
            {
                return NotFound();
            }
            await _placeRepository.Delete(place);
            return NoContent();
        }
    }
}
