using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthNi.Core.Abstractions;
using AuthNi.Core.Entities;

namespace AuthNi.Core.Repository
{
    public class UserService : IRepository<User>
    {
        // Здесь может быть хранилище пользователей (например, в памяти или в базе данных)
        private readonly Dictionary<Guid, User> _users = new Dictionary<Guid, User>();

        public async Task<User> GetByIdAsync(Guid id)
        {
            // Логика получения пользователя по ID
            if (_users.TryGetValue(id, out var user))
            {
                return await Task.FromResult(user);
            }
            return await Task.FromResult<User>(null);
        }

        public async Task<Guid> AddAsync(User entity)
        {
            // Логика добавления пользователя
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _users[entity.Id] = entity;
            return await Task.FromResult(entity.Id);
        }

        public async Task<Guid> UpdateAsync(Guid id, User entity)
        {
            // Логика обновления данных пользователя
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (_users.ContainsKey(id))
            {
                _users[id] = entity;
                return await Task.FromResult(id);
            }
            return await Task.FromResult<Guid>(Guid.Empty);
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            // Логика удаления пользователя
            return await Task.FromResult(_users.Remove(id));
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            // Логика аутентификации пользователя
            foreach (var user in _users.Values)
            {
                if (user.Username == username && User.VerifyPassword(password, user.PasswordHash))
                {
                    return await Task.FromResult(user);
                }
            }
            return await Task.FromResult<User>(null);
        }
    }
}
