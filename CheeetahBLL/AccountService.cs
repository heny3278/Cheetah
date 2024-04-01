using CeetahDAL;
using Cheetah;
using CheetahDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheeetahBLL
{
    public class AccountService
    {
        private readonly BaseRepository<Account> _Account;

        public AccountService(BaseRepository<Account> Account)
        {
            _Account = Account;
        }

        public async Task<Account> AddAccount(Account account)
        {
            EntityEntry<Account> result = _Account.Add(account);
            await _Account.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Account> GetAccountByID(int id)
        {
            return await _Account.Items.FindAsync(id);
        }


        public async Task<List<Account>> GetAccounts()
        {
            var result = await _Account.Items.ToListAsync();
            return result;
        }

        public async Task<Account> UpdateAccountAsync(int id, Account entity)
        {
            entity.AccountId = id;
            var saveTaskScheduling = _Account.Update(entity);
            await _Account.SaveChangesAsync();
            return saveTaskScheduling.Entity;
        }

        public async Task DeleteAccountAsync(int id)
        {
            _Account.RemoveById(id);
            await _Account.SaveChangesAsync();
        }
    }
}

