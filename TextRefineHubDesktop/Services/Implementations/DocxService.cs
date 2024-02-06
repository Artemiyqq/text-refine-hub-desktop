using TextRefineHubDesktop.Services.Contracts;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using Windows.Storage;
using System.IO.Pipes;
using Windows.Storage.Pickers;

namespace TextRefineHubDesktop.Services.Implementations
{
    public class DocxService : IDocxService
    {
        private readonly IStreamService _streamService;
        private readonly ITextProcessingService _textProcessingService;
        public DocxService(IStreamService streamService, ITextProcessingService textProcessingService)
        {
            _streamService = streamService;
            _textProcessingService = textProcessingService;
        }

        public async Task<byte[]> ProcessDocxAsync(StorageFile file)
        {
            using (var stream = await file.OpenStreamForReadAsync())
            {
                byte[] fileBytes = await _streamService.CopyReadOnlyStreamToMemory(stream);

                var document = OpenWordDocument(fileBytes);

                if (IsMainDocumentPartNull(document))
                {
                    throw new InvalidOperationException("The document is empty and cannot be processed.");
                }

                IEnumerable<string> paragraphsText = GetParagraphsText(document);
                string joinedText = string.Join(" ", paragraphsText);
                string fontForNewDocument = GetFirstFontInDocument(document);

                using (var newMemoryStream = new MemoryStream())
                {
                    using (var newDocument = WordprocessingDocument.Create(newMemoryStream, WordprocessingDocumentType.Document))
                    {
                        FillInNewDocument(newDocument, joinedText, fontForNewDocument);
                    }

                    newMemoryStream.Seek(0, SeekOrigin.Begin);

                    return newMemoryStream.ToArray();
                }
            }
        }

        public WordprocessingDocument OpenWordDocument(byte[] fileBytes)
        {
            var memoryStream = new MemoryStream(fileBytes);
            return WordprocessingDocument.Open(memoryStream, false);
        }

        public IEnumerable<string> GetParagraphsText(WordprocessingDocument document)
        {
            Body body = document.MainDocumentPart.Document.Body;

            return body.Elements<Paragraph>().Select(p => p.InnerText);
        }

        public string GetFirstFontInDocument(WordprocessingDocument document)
        {
            RunFonts font = document.MainDocumentPart.Document.Descendants<RunFonts>().FirstOrDefault();
            if (IsFontFound(font)) return font.Ascii;
            else return "Times New Roman";
        }

        public bool IsMainDocumentPartNull(WordprocessingDocument document)
        {
            if (document == null || document.MainDocumentPart == null) return true;
            else return false;
        }

        public bool IsFontFound(RunFonts font)
        {
            if (font == null || font.Ascii == null) return false;
            else return true;
        }

        public WordprocessingDocument FillInNewDocument(WordprocessingDocument newDocument, string text, string font)
        {
            MainDocumentPart newMainPart = newDocument.AddMainDocumentPart();
            newMainPart.Document = new Document();
            Body newBody = newMainPart.Document.AppendChild(new Body());

            Paragraph newParagraph = new Paragraph(
            new Run(
                new RunProperties(
                    new RunFonts() { Ascii = font },
                    new FontSize() { Val = "28" }
                ),
                new Text(_textProcessingService.ProcessText(text))
                )
            );

            newBody.AppendChild(newParagraph);

            return newDocument;
        }

        public async Task SaveProcessedDocxAsync(byte[] processedDocx, string originalFileName)
        {
            FileSavePicker savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.Desktop,
                SuggestedFileName = originalFileName,
                DefaultFileExtension = ".docx"
            };

            savePicker.FileTypeChoices.Add("Word Document", new string[] { ".docx" });

            StorageFile file = await savePicker.PickSaveFileAsync();

            if (file != null)
            {
                await FileIO.WriteBytesAsync(file, processedDocx);
            }
        }
    }
}
