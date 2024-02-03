using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TextRefineHubDesktop.Views
{
    /// <summary>
    /// Page for processing DOCX.
    /// </summary>
    public sealed partial class DocxProcessingPage : Page
    {
        public DocxProcessingPage()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await chooseFile();
        }

        private async Task chooseFile()
        {
            FileOpenPicker openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };

            openPicker.FileTypeFilter.Add(".docx");

            StorageFile file = await openPicker.PickSingleFileAsync();

            if (file != null)
            {
                var confirmationDialog = new ContentDialog
                {
                    Title = "Confirmation",
                    Content = $"Are you sure you want to process the file: {file.Name}?",
                    PrimaryButtonText = "Yes",
                    SecondaryButtonText = "No"
                };

                var confirmationResult = await confirmationDialog.ShowAsync();

                if (confirmationResult == ContentDialogResult.Primary)
                {
                    // User confirmed, you can process the selected file
                    // Implement your logic here
                }
                else
                {
                    var chooseAnotherFileDialog = new ContentDialog
                    {
                        Title = "Choose Another File",
                        Content = "Do you want to choose another file?",
                        PrimaryButtonText = "Yes",
                        SecondaryButtonText = "No"
                    };

                    var chooseAnotherFileDialogResult = await chooseAnotherFileDialog.ShowAsync();

                    if (chooseAnotherFileDialogResult == ContentDialogResult.Primary)
                    {
                        await chooseFile();
                    }
                }
            }
        }


    }
}
