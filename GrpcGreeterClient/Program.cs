using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcClient;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var user = new Users.UsersClient(channel);

bool flag = true;

while (flag)
{
    Console.WriteLine("Choose an Action from the list:");
    Console.WriteLine("1. Add User" +
        "\n2. Display User (async)" +
        "\n3. Upload photo" +
        "\n4. Display available photos to download" +
        "\n5. Exit");
    Console.Write(">: ");


    int option = int.Parse(Console.ReadLine());


    switch (option)
    {
        case 1:
            Console.WriteLine("Please put data (companyId),(FirstName),(LastName),(Email): ");
            string data = Console.ReadLine();
            var values = data.Split(',');

            if (values.Count() == 4)
            {
                var addUserReply = user.AddUser(
                            new UserAddRequest() { CompanyId = int.Parse(values[0].ToString()), FirstName = values[1].ToString(), LastName = values[2].ToString(), Email = values[3].ToString() }
                            );
                Console.WriteLine("Added: " + addUserReply.CompanyId + " " + addUserReply.FirstName + " " + addUserReply.LastName);
            }
            else
            {
                Console.WriteLine("Input was incorrect, try again");
            }
            break;

        case 2:
            try
            {
                UserRequest req = new UserRequest();
                using (var call = user.GetUsers(req))
                {
                    while (await call.ResponseStream.MoveNext(CancellationToken.None))
                    {
                        var currentUser = call.ResponseStream.Current;
                        Console.WriteLine($"{currentUser.CompanyId}, {currentUser.FirstName}, {currentUser.LastName}, {currentUser.Email} is being fetched from the service");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            break;

        case 3:
            break;

        case 4:
            break;

        case 5:
            flag = false;
            break;

        default:
            Console.WriteLine("Wrong input try again");
            break;
    }


}




Console.WriteLine("Press any key to exit...");
Console.ReadKey();