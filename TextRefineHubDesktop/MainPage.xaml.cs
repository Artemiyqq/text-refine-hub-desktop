using TextRefineHubDesktop.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TextRefineHubDesktop
{
    /// <summary>
    /// Main page container.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            contentFrame.Navigate(typeof(TextProcessingPage));
        }

        private void SwitchToTextMode(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(TextProcessingPage));
            // Highlight the active button or update the UI accordingly
            btnTextMode.IsEnabled = false;
            btnDocxMode.IsEnabled = true;
        }

        private void SwitchToDocxMode(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(DocxProcessingPage));
            // Highlight the active button or update the UI accordingly
            btnTextMode.IsEnabled = true;
            btnDocxMode.IsEnabled = false;
        }
    }
}
