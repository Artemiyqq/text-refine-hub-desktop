using TextRefineHubDesktop.Services.Contracts;

namespace TextRefineHubDesktop.Services.Implementations
{
    public class TextProcessingService: ITextProcessingService
    {
        public string ProcessText(string text)
        {
            text = text.Replace("-\r", "");
            text = text.Replace("\r", " ");
            return text;
        }
    }
}
