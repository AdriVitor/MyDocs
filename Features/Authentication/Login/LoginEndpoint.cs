using FastEndpoints;
using MyDocs.Features.Documents.DeleteDocument;

namespace MyDocs.Features.Authentication.Login
{
    public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
    {
        private readonly LoginService _service;
        public LoginEndpoint(LoginService service)
        {
            _service = service;
        }

        public override void Configure()
        {
            Post("Authentication");
            AllowAnonymous();
        }

        public override async Task HandleAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            try
            {
                LoginResponse response = await _service.GenerateToken(request);

                await SendAsync(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
