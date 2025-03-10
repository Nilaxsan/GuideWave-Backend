using AutoMapper;
using GuideWave.DTO.Guides;
using GuideWave.DTO.Tourists;
using GuideWave.Models;
using GuideWave.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuideWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristsController : ControllerBase
    {
        private readonly ITouristsRepository _touristsRepository;

        private readonly IMapper _mapper;

        public TouristsController(ITouristsRepository touristsRepository, IMapper mapper)
        {
            _touristsRepository = touristsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<IEnumerable<TouristsDto>>> GetAll()
        {
            var tourists = await _touristsRepository.GetAll();

            var touristsDto = _mapper.Map<List<GuidesDto>>(tourists);

            if (touristsDto == null)
            {
                return NoContent();
            }

            return Ok(touristsDto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<GuidesDto>> GetById(int id)
        {


            var tourists = await _touristsRepository.Get(id);


            var touristsDto = _mapper.Map<GuidesDto>(tourists);
            if (tourists == null)
            {
                return NoContent();
            }
            return Ok(touristsDto);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<ActionResult<CreateTouristsDto>> Create([FromBody] CreateTouristsDto createtouristsDto)
        {
            var result = _touristsRepository.IsRecordsExists(x => x.Email == createtouristsDto.Email);
            if (result)
            {

                return Conflict("Tourist with this email already exists in database");
            }


            var tourist = _mapper.Map<Tourists>(createtouristsDto);


            await _touristsRepository.Create(tourist);
            return CreatedAtAction("GetById", new { id = tourist.UserId }, tourist);

        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Tourists>> Update(int id, [FromBody] TouristsDto touristsDto)
        {
            if (touristsDto == null || id != touristsDto.UserId)
            {
                return BadRequest();
            }

            var tourist = _mapper.Map<Tourists>(touristsDto);

            await _touristsRepository.Update(tourist);
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

            var tourist = await _touristsRepository.Get(id);


            if (tourist == null)
            {
                return NotFound();
            }
            await _touristsRepository.Delete(tourist);
            return NoContent();
        }
    }
}
