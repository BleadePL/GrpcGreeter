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
            Console.Write("Put filepath: ");

            //B:\\PWR\\Semestr 6\\Rozproszone systemy informatyczne\\Laboratorium\\Zadania\\Zadanie3\\test.png
            //B:\\PWR\\Semestr 6\\Rozproszone systemy informatyczne\\Laboratorium\\Zadania\\Zadanie3\\otter.jpg

            string filePath = Console.ReadLine();

            Console.WriteLine("input fileName: ");
            string fileTitle = Console.ReadLine();

            var filetest = File.ReadAllBytes(filePath);

            var photo = new Photos.PhotosClient(channel);

            Console.WriteLine("pass");
            var tmp = 0;

            var arrays = filetest.Chunk(10).ToList();

            using (var call = photo.UploadPhoto())
            {
                foreach (var item in arrays)
                {
                    await call.RequestStream.WriteAsync(new UploadPhotoRequest() { Image = ByteString.CopyFrom(item), FileName = fileTitle, FileSize= filetest.Length});
                    ProgressBar.draw(tmp+=10, filetest.Length);
                }
                await call.RequestStream.CompleteAsync();
                var summary = await call.ResponseAsync;
            }

            break;

        case 4:
            var photo2 = new Photos.PhotosClient(channel);
            try
            {

                Console.WriteLine("Availible files:");

                FileNamesRequest req = new FileNamesRequest();
                using (var call = photo2.GetFileNames(req))
                {
                    while (await call.ResponseStream.MoveNext(CancellationToken.None))
                    {
                        var currentTitle = call.ResponseStream.Current;
                        Console.WriteLine($"{currentTitle.FileName}");
                    }
                }

                Console.Write("Chosen file: ");
                string fileName = Console.ReadLine();

                GetFileRequest fileReq = new GetFileRequest() { FileName = fileName };

                int counter = 0;
                
                using(var call2 = photo2.GetFile(fileReq))
                {
                    var path = Path.Combine("Download", fileName);
                    while (await call2.ResponseStream.MoveNext(CancellationToken.None))
                    {
                        var currentBytes = call2.ResponseStream.Current.Image;
                        var fs = new FileStream(path, FileMode.Append);

             

                        fs.Write(currentBytes.ToByteArray());
                        fs.Close();
                    }
                       
                }





            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

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