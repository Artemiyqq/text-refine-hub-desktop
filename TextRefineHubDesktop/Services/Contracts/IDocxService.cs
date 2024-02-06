using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using Windows.Storage;

namespace TextRefineHubDesktop.Services.Contracts
{
    public interface IDocxService
    {
        Task<byte[]> ProcessDocxAsync(StorageFile file);
        WordprocessingDocument OpenWordDocument(byte[] fileBytes);
        IEnumerable<string> GetParagraphsText(WordprocessingDocument document);
        string GetFirstFontInDocument(WordprocessingDocument document);
        WordprocessingDocument FillInNewDocument(WordprocessingDocument newDocument, string text, string font);
        Task SaveProcessedDocxAsync(byte[] processedDocx, string originalFileName);
    }
}