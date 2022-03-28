using Grpc.Core;
using GrpcGreeter;

namespace GrpcGreeter.Services
{
    public class PhotoService : Photos.PhotosBase
    {
        private readonly ILogger<PhotoService> _logger;
        public PhotoService(ILogger<PhotoService> logger)
        {
            _logger = logger;
        }

        private const string uploadPath = "Upload";

        public override async Task<UploadPhotosResponse> UploadPhoto(IAsyncStreamReader<UploadPhotoRequest> requestStream, ServerCallContext context)
        {
            //requestStream.MoveNext();
/*            Console.WriteLine("passsseedd");
            var path = Path.Combine(uploadPath, requestStream.Current.FileName);
            var fs = File.Create(path, requestStream.Current.FileSize);*/


/*            do
            {
                fs.Write(requestStream.Current.Image.ToArray());
            } while (requestStream.Current.FileSize == fs.Position);*/




            while (await requestStream.MoveNext())
            {
                var path = Path.Combine(uploadPath, requestStream.Current.FileName);

                var fs = new FileStream(path, FileMode.Append);
                fs.Write(requestStream.Current.Image.ToArray());
                fs.Close();
/*                


                var fs = File.Create(path, requestStream.Current.FileSize);
                Console.WriteLine(path);*/
            }


            return await Task.FromResult(new UploadPhotosResponse
            {
                Message = "Success"
            });

        }
    }
}