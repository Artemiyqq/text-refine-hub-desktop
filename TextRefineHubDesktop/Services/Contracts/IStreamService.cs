using System.IO;
using System.Threading.Tasks;

namespace TextRefineHubDesktop.Services.Contracts
{
    public interface IStreamService
    {
        Task<byte[]> CopyReadOnlyStreamToMemory(Stream readOnlyStream);
    }
}