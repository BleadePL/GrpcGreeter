using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcClient;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(
                  new HelloRequest { Name = "GreeterClient" });


var user = new Users.UsersClient(channel);

var addUser = new Users.UsersClient(channel);

var addUserReply = addUser.AddUser(
        new UserAddRequest() { CompanyId = 1, FirstName = "Konrad", LastName = "Więckiewicz", Email = "email@studentgmail.com" }
    );

Console.WriteLine("Added: " + addUserReply.CompanyId + " " + addUserReply.FirstName + " " + addUserReply.LastName);

try
{
    UserRequest req = new UserRequest() { CompanyId = 1 };
    using (var call = user.GetUsers(req))
    {
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            var currentUser = call.ResponseStream.Current;
            Console.WriteLine(currentUser.FirstName + " " + currentUser.LastName);
        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}



Console.WriteLine("Greeting: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();