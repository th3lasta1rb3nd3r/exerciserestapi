using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using FizzWare.NBuilder;
using Simple.Data;
using Simple.Model.Inputs;

namespace Simple.Service.Tests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<SimpleDataDbContext> _simpleDataDbContextMock;

        public UserServiceTests()
        {
            //global prepare
            var simpleDataDbContextFactoryMock = new Mock<IDbContextFactory<SimpleDataDbContext>>();

            var simpleDataDbContextOptionsMock = new Mock<DbContextOptions<SimpleDataDbContext>>();
            simpleDataDbContextOptionsMock.Setup(e => e.ContextType).Returns(typeof(SimpleDataDbContext));

            _simpleDataDbContextMock = new Mock<SimpleDataDbContext>(simpleDataDbContextOptionsMock.Object);
            var databaseFacadeMock = new Mock<DatabaseFacade>(_simpleDataDbContextMock.Object);

            var transactionMock = new Mock<IDbContextTransaction>();

            databaseFacadeMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>())).ReturnsAsync(transactionMock.Object);
            _simpleDataDbContextMock.SetupGet(x => x.Database).Returns(databaseFacadeMock.Object);

            simpleDataDbContextFactoryMock.Setup(e => e.CreateDbContextAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_simpleDataDbContextMock.Object);

            var mockUsers = MockDbSet<int, User>(Builder<User>.CreateListOfSize(1).Build().AsQueryable()).Object;
            var mockAddresses = MockDbSet<int, Address>(Builder<Address>.CreateListOfSize(1).Build().AsQueryable()).Object;
            var mockEmployments = MockDbSet<int, Employment>(Builder<Employment>.CreateListOfSize(1).Build().AsQueryable()).Object;

            _simpleDataDbContextMock.Setup(e => e.Users).Returns(mockUsers);
            _simpleDataDbContextMock.Setup(e => e.Addresses).Returns(mockAddresses);
            _simpleDataDbContextMock.Setup(e => e.Employments).Returns(mockEmployments);

            _userService = new(Mock.Of<ILoggerFactory>(), simpleDataDbContextFactoryMock.Object);
        }

        [Fact]
        public async Task AddUserAsyncSucccessTest()
        {
            //act
            var user = await _userService
                    .AddUserAsync(new AddUserInput(
                            new UserInput("Eugene", "Honor", "eugene.honor@gmail.com"),
                            new AddressInput("Street 1", "City 1", 1),
                            [new EmploymentInput("alight", 124, 432.32, DateTime.Today, default)]));

            //assert
            Assert.NotNull(user);
            Assert.NotNull(user.Value);
            Assert.Null(user.Errors);
        }

        [Fact]
        public async Task AddUserAsyncValidationErrorTest()
        {
            //act
            var user = await _userService
                    .AddUserAsync(new AddUserInput(
                            new UserInput("Eugene", "Honor", "eugene.honor"),
                            new AddressInput(string.Empty, "City 1", 1),
                            null));

            //assert
            Assert.NotNull(user);
            Assert.Null(user.Value);
            Assert.NotNull(user.Errors);
        }

        private static Mock<DbSet<T>> MockDbSet<K, T>(IQueryable<T> data) where T : Entity<K>
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
    }
}