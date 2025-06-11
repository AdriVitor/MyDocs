using Moq;
using MyDocs.Features.Users.Create;
using MyDocs.Infraestructure.ExternalServices.Email;
using MyDocs.Tests.Shared;

namespace MyDocs.Tests.Users.CreateUser
{
    public class CreateUserServiceTests
    {
        [Fact]
        public async Task AddUser_ShouldCreateUserAndCredentialsAndSendEmail_WhenRequestIsValid()
        {
            using var context = MemoryDatabase.Create();

            var emailServiceMock = new Mock<IEmailService>();

            var service = new CreateUserService(context, emailServiceMock.Object);

            var request = new CreateUserRequest
            {
                Name = "Test Create User",
                CPF = "83590820063",
                DateOfBirth = new DateTime(1995, 1, 1),
                Phone = "11988991166",
                Email = "testUser@email.com",
                Password = "password123"
            };

            await service.AddUser(request);

            var user = context.Users.FirstOrDefault(u => u.CPF == request.CPF);
            Assert.NotNull(user);
            Assert.Equal(request.Name, user.Name);
            Assert.Equal(request.CPF, user.CPF);

            var credentials = context.UsersCredentials.FirstOrDefault(c => c.IdUser == user.Id);
            Assert.NotNull(credentials);
            Assert.Equal(request.Email, credentials.Email);
            Assert.Equal(request.Password, credentials.Password);
            Assert.Equal(user.Id, credentials.IdUser);

            emailServiceMock.Verify(e => e.SendEmail(
                request.Email,
                "Criação de conta MyDocs",
                "<h1> Parabéns pela sua criação de conta no MyDocs </h1>"), Times.Once);
        }

    }
}
