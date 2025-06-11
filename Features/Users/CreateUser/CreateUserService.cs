using MyDocs.Features.Users.CreateUser;
using MyDocs.Infraestructure.ExternalServices.Email;
using MyDocs.Infraestructure.Persistence;
using MyDocs.Models;

namespace MyDocs.Features.Users.Create
{
    public class CreateUserService : ICreateUserService
    {
        private readonly Context _context;
        private readonly IEmailService _emailService;
        public CreateUserService(Context context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task AddUser(CreateUserRequest request)
        {
            int idUser = await CreateUser(request);

            await CreateUserCredential(request.Email, request.Password, idUser);

            await _emailService.SendEmail(request.Email, "Criação de conta MyDocs", "<h1> Parabéns pela sua criação de conta no MyDocs </h1>");
        }

        private async Task<int> CreateUser(CreateUserRequest request)
        {
            User user = new User
            {
                Name = request.Name,
                CPF = request.CPF,
                DateOfBirth = request.DateOfBirth,
                Phone = request.Phone,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        private async Task CreateUserCredential(string email, string password, int userId)
        {
            UserCredentials userCredentials = new UserCredentials
            {
                Email = email,
                Password = password,
                IdUser = userId,
            };

            _context.UsersCredentials.Add(userCredentials);
            await _context.SaveChangesAsync();
        }
    }
}
