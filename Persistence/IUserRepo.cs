
using System.Threading.Tasks;
using ExceptionStrategy.Domain.Users;

namespace ExceptionStrategy.Persistence
{
    public interface IUserRepo
    {
        Task<User> GetExisting(int id);
        Task<User> CreateNew(string name, string password);
    }
}