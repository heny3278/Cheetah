using CeetahDAL;
using Cheetah;
using CheetahDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CheeetahBLL
{
    public class UserService
    {
        private readonly BaseRepository<User> _User;


        public UserService(BaseRepository<User> User)
        {
            _User = User; 
        }

        public async Task<User> AddUser(User user)
        {
            EntityEntry<User> result = _User.Add(user);
            await _User.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<User> GetUserByID(int id)
        {
            return await _User.Items.FindAsync(id);
        }

        public async Task<User> GetUserByAccountAndUserID(int accountId, int userId)
        {
            return await _User.Items.FirstAsync(u=>u.AccountId == accountId && u.UserId == userId);
        }

        public async Task<List<User>> GetUsersByAccountId(int accountId)
        {
            return await _User.Items.Where(a => a.AccountId == accountId).ToListAsync();

        }


        public async Task<List<User>> GetUsers()
        {
            var result = await _User.Items.ToListAsync();
            return result;
        }
        public async Task<User> UpdateAsync(int id, User entity)
        {
            entity.UserId = id;
            var saveTaskScheduling = _User.Update(entity);
            await _User.SaveChangesAsync();
            return saveTaskScheduling.Entity;
        }
        public async Task DeleteAsync(int id)
        {
            _User.RemoveById(id);
            await _User.SaveChangesAsync();
        }
    }
}
