using Microsoft.AspNetCore.Mvc;
using Simple.Data;
using Simple.Model;
using Simple.Model.Inputs;
using Simple.Service.Interfaces;

namespace SimpleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet(Name = nameof(GetUser))]
        public Task<User?> GetUser(int userId, CancellationToken cancellationToken)
                => _userService.GetUserAsync(userId, cancellationToken);

        [HttpPost(Name = nameof(AddUser))]
        public async Task<GenericOutput<User?>> AddUser(AddUserInput input, CancellationToken cancellationToken)
            => await _userService.AddUserAsync(input, cancellationToken);

        [HttpPut(Name = nameof(UpdateUser))]
        public async Task<GenericOutput<User?>> UpdateUser(UpdateUserInput input, CancellationToken cancellationToken)
            => await _userService.UpdateUserAsync(input, cancellationToken);
    }
}
