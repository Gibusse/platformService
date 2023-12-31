using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPlatformRepository _repository;
        public PlatformsController(IPlatformRepository repository, IMapper mapper)
        {
           _repository = repository;
           _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("---> Getting Platforms....");

            var platformItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var platformItem = _repository.GetPlatformById(id);

            if (platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDto>(platformItem));
            }  

            return NotFound();
        }

        [HttpPost]
        public ActionResult<PlatformCreateDto> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platformModel = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatfrom(platformModel);
            _repository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(platformModel);

            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id }, platformReadDto);
        }
    }
}