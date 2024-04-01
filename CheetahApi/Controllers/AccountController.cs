using AutoMapper;
using CheeetahBLL;
using Cheetah;
using CheetahApi.DTO;
using CheetahDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheetahApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly AccountService _AccountService;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;


        public AccountController(AccountService AccountService, IMapper mapper, ILogger<AccountController> logger)
        {
            _AccountService = AccountService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("account")]
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

        [HttpGet("account/{AccountId}")]
        public async Task<ActionResult<List<AccountDTO>>> GetAccountById(int userID)
        {
            try
            {
                return _mapper.Map<List<AccountDTO>>(await _AccountService.GetAccountByID(userID));
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
                var NewAccount =  _AccountService.AddAccount(_mapper.Map<Account>(AccountDTO)).Result;
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
        public async Task<ActionResult<AccountDTO>> Put(int id, AccountDTO AccountDTO)
        {
            try
            {
                var user = await _AccountService.UpdateAccountAsync(id, _mapper.Map<Account>(AccountDTO));
                if (user is null)
                    return NotFound();
                return _mapper.Map<AccountDTO>(await _AccountService.GetAccountByID(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

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
