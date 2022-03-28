using Grpc.Core;
using GrpcGreeter;

namespace GrpcGreeter.Services
{
    public class UserService : Users.UsersBase
    {
        private readonly ILogger<UserService> _logger;
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        private static List<UserResponse> _users = new List<UserResponse>();


        private List<UserResponse> getUsers(int companyId)
        {
            _users.Add(new UserResponse() { CompanyId = 1, FirstName = "f1", LastName = "s1", Email = "email@gmail.com" });
            return _users;
        }


        public override Task<UserResponse> AddUser(UserAddRequest request, ServerCallContext context)
        {
            Console.WriteLine("Pierwszy z add");
            _users.Add(new UserResponse() { CompanyId = request.CompanyId, FirstName = request.FirstName, LastName = request.LastName, Email = request.Email });
            return Task.FromResult(new UserResponse(_users.Last()));
        }

        public override async Task GetUsers(UserRequest request, IServerStreamWriter<UserResponse> responseStream, ServerCallContext context)
        {
            Console.WriteLine("Pierwszy z getusers");
            var users = getUsers(request.CompanyId);
            Console.WriteLine(users.Count);
            foreach (var user in users)
            {
                await Task.Delay(1000);
                await responseStream.WriteAsync(user);
            }
        }

    }
}