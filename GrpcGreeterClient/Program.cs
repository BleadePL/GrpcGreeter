using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcClient;
using GrpcGreeter;
using GrpcGreeterClient;

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
            Console.WriteLine("saving Data");
            Console.WriteLine("Put filepath: ");
            

            //string filePath = Console.ReadLine();

           // FileStream stream = File.OpenRead("B:\\PWR\\Semestr 6\\Rozproszone systemy informatyczne\\Laboratorium\\Zadania\\Zadanie3\\test.png");

            var filetest = File.ReadAllBytes("B:\\PWR\\Semestr 6\\Rozproszone systemy informatyczne\\Laboratorium\\Zadania\\Zadanie3\\test.png");


            var photo = new Photos.PhotosClient(channel);

            Console.WriteLine("pass");
            var tmp = 1;

            using (var call = photo.UploadPhoto())
            {
                foreach (var item in filetest)
                {
                    await call.RequestStream.WriteAsync(new UploadPhotoRequest() { Image = ByteString.CopyFrom(item), FileName = "test.png", FileSize= filetest.Length});
                    ProgressBar.draw(tmp++, filetest.Length);
                }
                await call.RequestStream.CompleteAsync();
                var summary = await call.ResponseAsync;
            }



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