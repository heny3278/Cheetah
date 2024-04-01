using CeetahDAL;
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
