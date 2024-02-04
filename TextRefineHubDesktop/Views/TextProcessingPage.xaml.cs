using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TextRefineHubDesktop.Views
{
    /// <summary>
    /// Page for processing text.
    /// </summary>
    public sealed partial class TextProcessingPage : Page
    {
        public TextProcessingPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBox textBox = TextBoxControl;

            if (textBox != null)
            {
                string inputText = textBox.Text;
                string processedText = App.TextProcessingService.ProcessText(inputText);

                var dataPackage = new Windows.ApplicationModel.DataTransfer.DataPackage();
                dataPackage.SetText(processedText);
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);

                var flyout = new Flyout();
                var flyoutContent = new TextBlock { Text = "Result copied to clipboard!" };

                flyout.Content = flyoutContent;

                flyout.ShowAt((FrameworkElement)sender);
            }
        }
    }
}
