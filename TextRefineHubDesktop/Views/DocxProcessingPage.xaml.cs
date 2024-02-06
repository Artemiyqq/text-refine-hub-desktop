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
        private readonly ContentDialog confirmationDialog = new ContentDialog
        {
            Title = "Confirmation",
            Content = $"Are you sure you want to process that file?",
            PrimaryButtonText = "Yes",
            SecondaryButtonText = "No"
        };

        private readonly ContentDialog chooseAnotherFileDialog = new ContentDialog
        {
            Title = "Choose Another File",
            Content = "Do you want to choose another file?",
            PrimaryButtonText = "Yes",
            SecondaryButtonText = "No"
        };

        public DocxProcessingPage()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await ChooseFile();
        }

        private async Task ChooseFile()
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
                var confirmationResult = await confirmationDialog.ShowAsync();

                if (confirmationResult == ContentDialogResult.Primary)
                {
                    byte[] processedDocx = await App.DocxService.ProcessDocxAsync(file);

                    await App.DocxService.SaveProcessedDocxAsync(processedDocx, file.Name);
                }
                else
                {
                    var chooseAnotherFileDialogResult = await chooseAnotherFileDialog.ShowAsync();

                    if (chooseAnotherFileDialogResult == ContentDialogResult.Primary)
                    {
                        await ChooseFile();
                    }
                }
            }
        }
    }
}
