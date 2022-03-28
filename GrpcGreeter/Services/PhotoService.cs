using Google.Protobuf;
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

            while (await requestStream.MoveNext())
            {
                var path = Path.Combine(uploadPath, requestStream.Current.FileName);

                var fs = new FileStream(path, FileMode.Append);
                fs.Write(requestStream.Current.Image.ToArray());
                fs.Close();
            }


            return await Task.FromResult(new UploadPhotosResponse
            {
                Message = "Success"
            });

        }


        public override async Task GetFileNames(FileNamesRequest request, IServerStreamWriter<FileNamesResponse> responseStream, ServerCallContext context)
        {
            var tmp = new DirectoryInfo(uploadPath);
            FileInfo[] files = tmp.GetFiles();

            foreach (var file in files)
            {
                await responseStream.WriteAsync(new FileNamesResponse { FileName = file.Name });
            }

        }

        public override async Task GetFile(GetFileRequest request, IServerStreamWriter<GetFileResponse> responseStream, ServerCallContext context)
        {
            var path = Path.Combine(uploadPath, request.FileName);

            var byteFile = File.ReadAllBytes(path);

            var arrays = byteFile.Chunk(10).ToList();


            foreach (var bytes in arrays)
            {
                await responseStream.WriteAsync(new GetFileResponse 
                { FileName = request.FileName, FileSize = bytes.Length, 
                    Image= ByteString.CopyFrom(bytes) }
                );
            }

        }

    }
}