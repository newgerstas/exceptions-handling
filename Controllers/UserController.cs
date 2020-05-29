using System.Threading.Tasks;
using ExceptionStrategy.Domain.Users;
using ExceptionStrategy.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionStrategy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo userRepo;

        public UserController(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        [HttpGet]
        public async Task<User> Get(int id)
        {
            return await userRepo.GetExisting(id);
        }

        [HttpPost]
        public async Task<User> Post([FromBody] UserRegisterForm form)
        {
            return await userRepo.CreateNew(form.Username, form.Password);
        }
    }

    public class UserRegisterForm
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}