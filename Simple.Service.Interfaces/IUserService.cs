using Simple.Data;
using Simple.Model;
using Simple.Model.Inputs;

namespace Simple.Service.Interfaces;

public interface IUserService
{
    Task<GenericOutput<User?>> AddUserAsync(AddUserInput input, CancellationToken cancellationToken = default);
    Task<GenericOutput<User?>> UpdateUserAsync(UpdateUserInput input, CancellationToken cancellationToken = default);
    Task<GenericOutput<User?>> GetUserAsync(int userId, CancellationToken cancellationToken = default);
}
