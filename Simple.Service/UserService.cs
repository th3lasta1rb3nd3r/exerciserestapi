using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Simple.Data;
using Simple.Model;
using Simple.Model.Errors;
using Simple.Model.Inputs;
using Simple.Service.Extensions;
using Simple.Service.Interfaces;

namespace Simple.Service;

public class UserService(
    ILoggerFactory loggerFactory,
    IDbContextFactory<SimpleDataDbContext> sampleDataDbContextFactory) : IUserService
{
    private readonly ILogger<UserService> _logger = loggerFactory.CreateLogger<UserService>();
    private readonly IDbContextFactory<SimpleDataDbContext> _sampleDataDbContextFactory = sampleDataDbContextFactory;

    public async Task<GenericOutput<User?>> AddUserAsync(AddUserInput input, CancellationToken cancellationToken = default)
    {
        using var sampleDataDbContext = await _sampleDataDbContextFactory.CreateDbContextAsync(cancellationToken);
        using var trans = await sampleDataDbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var errors = input.Validate(sampleDataDbContext);
            if (errors.Count > 0)
            {
                return new GenericOutput<User?>(errors, default);
            }

            var user = new User();
            SetUser(user, input.User);
            sampleDataDbContext.Users.Add(user);
            await sampleDataDbContext.SaveChangesAsync(cancellationToken);

            if (input.Address is not null)
            {
                var address = new Address() {  Id = user.Id};
                SetAddress(address, input.Address);
                sampleDataDbContext.Addresses.Add(address);
                await sampleDataDbContext.SaveChangesAsync(cancellationToken);
                user.Address = address;
            }

            await AddEmployments(sampleDataDbContext, user, input.Employments, cancellationToken);

            await trans.CommitAsync(cancellationToken);

            return new(default, user);
        }
        catch (Exception ex)
        {
            await trans.RollbackAsync(cancellationToken);
            _logger.LogError("Error on {MethodName}: {Message}", nameof(AddUserAsync), ex.Message);
            throw;
        }
    }

    public async Task<GenericOutput<User?>> UpdateUserAsync(UpdateUserInput input, CancellationToken cancellationToken = default)
    {
        using var sampleDataDbContext = await _sampleDataDbContextFactory.CreateDbContextAsync(cancellationToken);
        using var trans = await sampleDataDbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var errors = input.Validate(sampleDataDbContext);
            if (errors.Count > 0)
            {
                return new(errors, default);
            }

            var existingUser = sampleDataDbContext.Users.Single(e => e.Id == input.UserId);
            SetUser(existingUser, input.User);

            if (input.Address is not null)
            {
                var address = sampleDataDbContext.Addresses.SingleOrDefault(e => e.Id == input.UserId);
                if (address is null)
                {
                    address = new() {  Id = existingUser.Id};
                    SetAddress(address, input.Address);
                    sampleDataDbContext.Addresses.Add(address);
                    await sampleDataDbContext.SaveChangesAsync(cancellationToken);
                    existingUser.Address = address;
                }
                else
                {
                    SetAddress(address, input.Address);
                    existingUser.Address = address;
                }
            }

            var existingEmployments = sampleDataDbContext.Employments.Where(e => e.UserId == input.UserId);
            sampleDataDbContext.RemoveRange(existingEmployments);
            await sampleDataDbContext.SaveChangesAsync(cancellationToken);
            existingUser.Employments.Clear();

            await AddEmployments(sampleDataDbContext, existingUser, input.Employments, cancellationToken);
            
            await sampleDataDbContext.SaveChangesAsync(cancellationToken);
            await trans.CommitAsync(cancellationToken);

            return new(default, existingUser);
        }
        catch (Exception ex)
        {
            await trans.RollbackAsync(cancellationToken);
            _logger.LogError("Error on {MethodName}: {Message}", nameof(UpdateUserAsync), ex.Message);
            throw;
        }
    }

    public async Task<GenericOutput<User?>> GetUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        using var sampleDataDbContext = await _sampleDataDbContextFactory.CreateDbContextAsync(cancellationToken);
        var user = await sampleDataDbContext.Users.Where(e => e.Id == userId).SingleOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return new([new UserNotFoundError()], null);
        }

        user.Address = await sampleDataDbContext.Addresses.SingleOrDefaultAsync(e => e.Id == userId, cancellationToken);
        user.Employments = await sampleDataDbContext.Employments.Where(e => e.UserId == userId).ToListAsync(cancellationToken);

        return new(null, user);
    }

    private static void SetUser(User user, UserInput input)
    {
        user.FirstName = input.FirstName;
        user.LastName = input.LastName;
        user.EmailAddress = input.EmailAddress;
    }

    private static void SetAddress(Address address, AddressInput input)
    {
        address.Street = input.Street;
        address.City = input.City;
        address.PostCode = input.PostCode;
    }

    private static async Task AddEmployments(SimpleDataDbContext sampleDataDbContext,
        User user, List<EmploymentInput>? employmentInputs, CancellationToken cancellationToken)
    {
        if (employmentInputs is not null)
        {
            foreach (var employment in employmentInputs)
            {
                var newEmployment = new Employment()
                {
                    Company = employment.Company,
                    EndDate = employment.EndDate,
                    MonthsOfExperience = employment.MonthsOfExperience,
                    Salary = employment.Salary,
                    StartDate = employment.StartDate,
                    UserId = user.Id,
                };

                sampleDataDbContext.Employments.Add(newEmployment);
                await sampleDataDbContext.SaveChangesAsync(cancellationToken);
                user.Employments.Add(newEmployment);
            }
        }
    }
}
