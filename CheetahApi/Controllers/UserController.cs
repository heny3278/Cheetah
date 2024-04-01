using AutoMapper;
using CheeetahBLL;
using Cheetah;
using CheetahApi.DTO;
using CheetahDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;

namespace CheetahApi.Controllers
{
    // [Route("api/[controller]")]
    // [ApiController]
    public class UserController : Controller
    {
        private readonly UserService _UserService;
        private readonly AccountService _AccountService;

        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;


        public UserController(UserService userService, AccountService accountService, IMapper mapper, ILogger<UserController> logger)
        {

            _UserService = userService;
            _AccountService = accountService;
            _mapper = mapper;
            _logger = logger;

        }

        //  [HttpGet("user")]
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

        //  [HttpGet("user/{userId}")]
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
        public async Task<ActionResult<UserDTO>> Post([FromBody] UserDTO userDTO)
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

        //[HttpPut("{id}")]
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

        // [HttpDelete("{id}")]
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

        //

        [HttpGet("{accountId}/users/{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserAccountById(int accountId, int userId)
        {
            try
            {
                var result = _mapper.Map<UserDTO>(await _UserService.GetUserByAccountAndUserID(accountId, userId));
                if (result == null)
                {
                    return NotFound();
                }
                //  var result2 = result1.FirstOrDefault(y => y.UserId == userId);
                return _mapper.Map<UserDTO>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("{accountId}/user")]
        public async Task<ActionResult<List<UserDTO>>> GetUsersByAccountId(int accountId)
        {
            try
            {
                return  _mapper.Map<List<UserDTO>>(await _UserService.GetUsersByAccountId(accountId));
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
        [HttpPost("{accountId}/user")]
        public async Task<ActionResult<UserDTO>> AddUserToAccount(int accountId, [FromBody] UserDTO userToAdd)
        {
            // Retrieve the account from the database
            var account = _mapper.Map<AccountDTO>(await _AccountService.GetAccountByID(accountId));

            if (account == null)
            {
                return NotFound(); // Handle case where account is not found
            }

            // Associate the user with the account
            userToAdd.AccountId = accountId; // Assuming AccountId is a property in UserDTO

            // Add the user to the context and save changes
            await Post(userToAdd);
            //db.Users.Add(userToAdd);
            //await db.SaveChangesAsync();

            return Ok(userToAdd);
        }

        //[HttpPost("{accountId}/user")]
        //public async Task<ActionResult<UserDTO>> Post(int accountId,[FromBody] UserDTO userDTO)
        //{
        //    try
        //    {
        //        userDTO.accountId = accountId;
        //        // _UserService.AddUser(userDTO)

        //        var NewAccount = _mapper.Map < AccountDTO > (_AccountService.GetAccountByID(accountId));
        //        if (NewAccount is null)
        //            return NotFound();



        //        NewAccount.users.Add(userDTO);
        //        await
        //        return userDTO;

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return BadRequest();
        //    }
        //}
        [HttpDelete("{accountId}/user/{userId}")]
        public async Task<ActionResult<UserDTO>> DeleteUserFromAccount(int accountId, int userId)
        {
            try
            {
                var Users = _mapper.Map<AccountDTO>(_AccountService.GetAccountByID(accountId)).users;
                if (Users is null)
                    return NotFound();

                return _mapper.Map<UserDTO>(Users.Remove(Users.Find(x => x.UserId == userId)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPut("{accountId}/user/{userId}")]
        public async Task<ActionResult<UserDTO>> UpdateUserByAccount(int accountId, int userId, [FromBody] UserDTO userToUpdate)
        {
            var account = _mapper.Map<AccountDTO>(await _AccountService.GetAccountByID(accountId));
            if (account == null)
            {
                return NotFound(); // Handle case where account is not found
            }
            userToUpdate.AccountId = accountId; 
            Put(userId, userToUpdate);

            return Ok(userToUpdate);

        }

        


    }
}
