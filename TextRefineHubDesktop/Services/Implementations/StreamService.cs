using TextRefineHubDesktop.Services.Contracts;
using System.IO;
using System.Threading.Tasks;

namespace TextRefineHubDesktop.Services.Implementations
{
    public class StreamService : IStreamService
    {
        public async Task<byte[]> CopyReadOnlyStreamToMemory(Stream readOnlyStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                await readOnlyStream.CopyToAsync(memoryStream);

                readOnlyStream.Seek(0, SeekOrigin.Begin);

                byte[] resultBytes = memoryStream.ToArray();

                return resultBytes;
            }
        }
    }
}
