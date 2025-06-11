using Moq;
using MyDocs.Features.Users.GetUser;
using MyDocs.Models;
using MyDocs.Shared.Services.UserService;
using MyDocs.Tests.Shared;

namespace MyDocs.Tests.Users.GetUser
{
    public class GetUserServiceTests
    {
        [Fact]
        public async Task GetById_ShouldReturnUserResponse_WhenUserExists()
        {
            int idUser = 2;
            var user = GenerateModelsService.CreateUser(idUser, "Test GetById", "57928758040", new DateTime(1995, 1, 1), "11999999999");

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUser(idUser))
                .ReturnsAsync(user);
            var service = new GetUserService(userServiceMock.Object);

            var result = await service.GetById(2);

            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.Name, result.Name);
            Assert.Equal(user.CPF, result.CPF);
            Assert.Equal(user.DateOfBirth, result.DateOfBirth);
            Assert.Equal(user.Phone, result.Phone);
        }

        [Fact]
        public async Task GetById_ShouldThrowArgumentNullException_WhenUserNotFound()
        {
            int userId = 99;

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUser(userId))
                .ThrowsAsync(new ArgumentNullException("Usuário não encontrado"));

            var service = new GetUserService(userServiceMock.Object);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => service.GetById(userId));
        }

    }
}
