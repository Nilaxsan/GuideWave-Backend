using AutoMapper;
using GuideWave.DTO.Guides;
using GuideWave.Models;
using GuideWave.Repository;
using GuideWave.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuideWave.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuideController : ControllerBase
    {
        private readonly IGuideRepository _guidesRepository;

        private readonly IMapper _mapper;

        public GuideController(IGuideRepository guidesRepository, IMapper mapper)
        {
            _guidesRepository = guidesRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public async Task<ActionResult<IEnumerable<GuidesDto>>> GetAll()
        {
            var guides = await _guidesRepository.GetAll();

            var guidesDto = _mapper.Map<List<GuidesDto>>(guides);

            if (guidesDto == null)
            {
                return NoContent();
            }

            return Ok(guidesDto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<GuidesDto>> GetById(int id)
        {


            var guides = await _guidesRepository.Get(id);


            var guidesDto = _mapper.Map<GuidesDto>(guides);
            if (guides == null)
            {
                return NoContent();
            }
            return Ok(guidesDto);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]

        public async Task<ActionResult<CreateGuidesDto>> Create([FromBody] CreateGuidesDto createguidesDto)
        {
            var result = _guidesRepository.IsRecordsExists(x => x.Email == createguidesDto.Email);
            if (result)
            {

                return Conflict("State already exists in database");
            }


            var guide = _mapper.Map<Guide>(createguidesDto);


            await _guidesRepository.Create(guide);
            return CreatedAtAction("GetById", new { id = guide.UserId }, guide);

        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Guide>> Update(int id, [FromBody] GuidesDto guidesDto)
        {
            if (guidesDto == null || id != guidesDto.UserId)
            {
                return BadRequest();
            }

            var guide = _mapper.Map<Guide>(guidesDto);

            await _guidesRepository.Update(guide);
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

            var guide = await _guidesRepository.Get(id);


            if (guide == null)
            {
                return NotFound();
            }
            await _guidesRepository.Delete(guide);
            return NoContent();
        }
    }
}
