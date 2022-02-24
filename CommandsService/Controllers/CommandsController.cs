using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _commandRepository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo commandRepository, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"---> Hit GetCommands for platform: {platformId}");

            if (!_commandRepository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commands = _commandRepository.GetCommandsForPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"---> Hit GetCommand for platform: {platformId} and command: {commandId}");

            if (!_commandRepository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _commandRepository.GetCommandForPlatform(platformId, commandId);

            if (command == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
        {
            Console.WriteLine($"---> Hit CreateCommandForPlatform for platform: {platformId}");

            if (!_commandRepository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandDto);
            _commandRepository.CreateCommand(platformId, command);
            _commandRepository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute(nameof(GetCommandForPlatform),
                        new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
        }
    }
}