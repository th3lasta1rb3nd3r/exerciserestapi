using Microsoft.AspNetCore.Mvc;
using Simple.Data;
using Simple.Model;
using Simple.Model.Inputs;
using Simple.Service.Interfaces;

namespace SimpleApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet(Name = nameof(GetUser))]        
        public async Task<GenericOutput<User?>> GetUser([FromQuery] int userId, CancellationToken cancellationToken)
                => await _userService.GetUserAsync(userId, cancellationToken);

        [HttpPost(Name = nameof(AddUser))]
        public async Task<GenericOutput<User?>> AddUser([FromBody] AddUserInput input, CancellationToken cancellationToken)
            => await _userService.AddUserAsync(input, cancellationToken);

        [HttpPut(Name = nameof(UpdateUser))]
        public async Task<GenericOutput<User?>> UpdateUser([FromBody] UpdateUserInput input, CancellationToken cancellationToken)
            => await _userService.UpdateUserAsync(input, cancellationToken);
    }
}
