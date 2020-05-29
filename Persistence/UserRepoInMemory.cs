
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExceptionStrategy.Domain.Users;

namespace ExceptionStrategy.Persistence
{
    public class UserRepoInMemory : IUserRepo
    {
        private static readonly List<User> Users = new List<User>();

        public Task<User> GetExisting(int id)
        {
            var user = Users.SingleOrDefault(p => p.Id == id);

            if (user == null)
                throw new UserNotFoundException(id);

            return Task.FromResult(user);
        }

        public Task<User> CreateNew(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new InvalidUserNameException("EMPTY", userName);

            if (Users.Any(u => u.Name == userName))
                throw new DuplicateUserNameException(userName);

            if (string.IsNullOrEmpty(password))
                throw new InvalidPasswordException("EMPTY", "Password is empty", password);

            if (password.Length < 8)
            {
                throw new WeakPasswordException(password, 8);
            }

            var user = new User
            {
                Id = Users.Count + 1,
                Name = userName,
                PasswordHash = password.GetHashCode().ToString()
            };
            Users.Add(user);

            return Task.FromResult(user);
        }
    }
}