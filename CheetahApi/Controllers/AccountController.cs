using AutoMapper;
using CheeetahBLL;
using Cheetah;
using CheetahApi.DTO;
using CheetahDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;

namespace CheetahApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly AccountService _AccountService;
        private readonly UserService _UserService;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;


        public AccountController(UserService UserService, AccountService AccountService, IMapper mapper, ILogger<AccountController> logger)
        {
            _AccountService = AccountService;
            _UserService = UserService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccountDTO>>> GetAccounts()
        {
            try
            {
                return _mapper.Map<List<AccountDTO>>(await _AccountService.GetAccounts());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("accountId")]
        public async Task<ActionResult<AccountDTO>> GetAccountById(int accountId)
        {
            try
            {
                AccountDTO accout = _mapper.Map<AccountDTO>(await _AccountService.GetAccountByID(accountId));
                accout.users = _mapper.Map<List<UserDTO>>(await _UserService.GetUsersByAccountId(accountId));
                return accout;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }



        [HttpPost]
        public async Task<ActionResult<AccountDTO>> Post([FromBody] AccountDTO AccountDTO)
        {
            try
            {
                var NewAccount = _AccountService.AddAccount(_mapper.Map<Account>(AccountDTO)).Result;
                if (NewAccount is null)
                    return NotFound();
                return _mapper.Map<AccountDTO>(NewAccount);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AccountDTO>> Put(int id, [FromBody] AccountDTO AccountDTO)
        {
            //try
            //{
                var user = await _AccountService.UpdateAccountAsync(id, _mapper.Map<Account>(AccountDTO));
                if (user is null)
                    return NotFound();
                return _mapper.Map<AccountDTO>(await _AccountService.GetAccountByID(id));
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex.Message);
            //    return BadRequest();
            //}
        }

       

        //[HttpPut("{id}")]
        //public async Task<AccountDTO> Put(int id, [FromBody] AccountDTO AccountDTO)
        //{
        //    return _mapper.Map<AccountDTO>(await _AccountService.UpdateAccountAsync(id, new Account
        //    {
        //         AccountId = AccountDTO.AccountId;

        //    }
        //}




            [HttpDelete("{id}")]
        public async Task<ActionResult<AccountDTO>> Delete(int id)
        {
            try
            {
                var time = _AccountService.DeleteAccountAsync(id);
                if (time is null)
                    return NotFound();
                return _mapper.Map<AccountDTO>(await _AccountService.GetAccountByID(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}


        //



