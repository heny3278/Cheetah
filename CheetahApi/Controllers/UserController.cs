using AutoMapper;
using CheeetahBLL;
using CheetahApi.DTO;
using CheetahDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheetahApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserService _UserService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;


        public UserController(UserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            _UserService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("user")]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            try
            {
                return _mapper.Map<List<UserDTO>>(await _UserService.GetUsers());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<UserDTO>>> GetUsersById(int userID)
        {
            try
            {
                return _mapper.Map<List<UserDTO>>(await _UserService.GetUserByID(userID));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Post([FromBody]UserDTO userDTO)
        {
            try
            {
                var NewUser = await _UserService.AddUser(_mapper.Map<User>(userDTO));
                if (NewUser is null)
                    return NotFound();
                return _mapper.Map<UserDTO>(NewUser);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> Put(int id, UserDTO userDTO)
        {
            try
            {
                var user = await _UserService.UpdateAsync(id, _mapper.Map<User>(userDTO));
                if (user is null)
                    return NotFound();
                return _mapper.Map<UserDTO>(await _UserService.GetUserByID(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDTO>> Delete(int id)
        {
            try
            {
                var time = _UserService.DeleteAsync(id);
                if (time is null)
                    return NotFound();
                return _mapper.Map<UserDTO>(await _UserService.GetUserByID(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }



    }
}
