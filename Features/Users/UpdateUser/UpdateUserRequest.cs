using MyDocs.Shared.Abstracts.Users;

namespace MyDocs.Features.Users.UpdateUser
{
    public class UpdateUserRequest : BaseRequestGetUser
    {
        public string Name { get; set; }
        public string CPF { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
    }
}
