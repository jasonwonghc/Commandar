using AutoMapper;
using Commandar.Data;
using Commandar.Dtos;
using Commandar.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Commandar.Controllers
{
    [Route("api/[controller]")] 
    //[Route("api/commands")]
    [ApiController]
    public class CommandsController: ControllerBase
    {
        private readonly ICommandarRepo _repo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandarRepo repo, IMapper mapper) 
        {
            _repo = repo;
            _mapper = mapper;
        }

        //GET api/commands/[action]/
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands() 
        {
            var commandItems = _repo.GetAllCommand();

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        //GET api/commands/[action]/{id}
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        { 
            var commandItem = _repo.GetCommandById(id);
            if (commandItem != null)
            {
                return Ok(_mapper.Map<CommandReadDto>(commandItem));
            }
            else
            { 
                return NotFound();
            }
        }

        //POST api/commands/[action]/
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto) 
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _repo.CreateCommand(commandModel);
            _repo.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);


            //return Ok(commandReadDto);
            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto); // return the location of the url
        }

        //PUT api/commands//[action]/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        { 
            var commandModelFromRepo = _repo.GetCommandById(id);
            if (commandModelFromRepo == null) 
            {
                return NotFound();
            }
            _mapper.Map(commandUpdateDto, commandModelFromRepo);
            _repo.UpdateCommand(commandModelFromRepo); // no need (?)
            _repo.SaveChanges();

            return NoContent();
        }

        //PATCH api/commands//[action]/{id}
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc) 
        {
            var commandModelFromRepo = _repo.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
            patchDoc.ApplyTo(commandToPatch, ModelState);
            if (!TryValidateModel(commandToPatch)) 
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandModelFromRepo);
            _repo.UpdateCommand(commandModelFromRepo);
            _repo.SaveChanges();

            return NoContent();
        }

        //DELETE api/commands//[action]/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id) 
        {
            var commandModelFromRepo = _repo.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }
            _repo.DeleteCommand(commandModelFromRepo);
            _repo.SaveChanges();

            return NoContent();
        }
    }
}
