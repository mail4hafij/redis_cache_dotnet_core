using Microsoft.AspNetCore.Mvc;
using REST.Integration;

namespace REST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExampleController : ControllerBase
    {
        private readonly IRedisCacheService _redisCacheService;

        public ExampleController(IRedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }

        [HttpGet]
        public UserContract Get(string email)
        {
            // checking redis cache
            var task = _redisCacheService.GetAsync<UserContract>(email);
            if (task.Result != null)
            {
                return task.Result;
            }

            // add redis cache
            var user = new UserContract { 
                Firstname = "Test",
                Lastname = "Redis",
                Email = email
            };

            _redisCacheService.SetAsync(email, user);
            return user;
        }
    }
}