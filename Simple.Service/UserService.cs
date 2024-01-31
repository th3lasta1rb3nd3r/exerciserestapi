using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Simple.Data;
using Simple.Model;
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
            var addedUser = sampleDataDbContext.Users.Add(user);
            await sampleDataDbContext.SaveChangesAsync(cancellationToken);

            if (input.Address is not null)
            {
                var address = new Address();
                SetAddress(address, input.Address);
                var addedAddress = sampleDataDbContext.Addresses.Add(address);
                await sampleDataDbContext.SaveChangesAsync(cancellationToken);
                addedUser.Entity.Address = addedAddress.Entity;
            }

            await AddEmployments(sampleDataDbContext, addedUser.Entity, input.Employments, cancellationToken);

            await trans.CommitAsync(cancellationToken);

            return new(default, addedUser.Entity);
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
                if (address != null)
                {
                    SetAddress(address, input.Address);
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

    public async Task<User?> GetUserAsync(int userId, CancellationToken cancellationToken = default)
    {

        using var sampleDataDbContext = await _sampleDataDbContextFactory.CreateDbContextAsync(cancellationToken);
        var user = await sampleDataDbContext.Users.Where(e => e.Id == userId).SingleOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return null;
        }

        user.Address = await sampleDataDbContext.Addresses.SingleOrDefaultAsync(e => e.Id == userId, cancellationToken);
        user.Employments = await sampleDataDbContext.Employments.Where(e => e.UserId == userId).ToListAsync(cancellationToken);

        return user;
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
                var addedEmployment = sampleDataDbContext.Employments.Add(new()
                {
                    Company = employment.Company,
                    EndDate = employment.EndDate,
                    MonthsOfExperience = employment.MonthsOfExperience,
                    Salary = employment.Salary,
                    StartDate = employment.StartDate,
                    UserId = user.Id,
                });
                await sampleDataDbContext.SaveChangesAsync(cancellationToken);
                user.Employments.Add(addedEmployment.Entity);
            }
        }
    }
}
