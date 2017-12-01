using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using static FormCapture.Utilities;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FormCapture
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private List<Applicant> applicantList;
        private List<Applicant> fullApplicantList;

        public MainPage()
        {
            this.InitializeComponent();
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
            loadApplicants();
        }

        private void ApplicantSelected(object sender, SelectionChangedEventArgs e)
        {
            if (ApplicantView.SelectedItem != null)
            {
                ContentGrid.Content = new InterviewPanel2017((Applicant)ApplicantView.SelectedItem);
            }
        }

        private async void OpenCSV(object sender, RoutedEventArgs e)
        {
            var openFilePicker = new FileOpenPicker();
            openFilePicker.FileTypeFilter.Add(".csv");
            var file = await openFilePicker.PickSingleFileAsync();
            if (file != null)
            {
                try
                {
                    await ProcessCSV(file);
                    await Notify("Import complete.");
                } catch
                {
                    await Notify("An error occurred - check the format of your CSV file.");
                }
                loadApplicants();
            }
        }

        private void loadApplicants()
        {
            fullApplicantList = ApplicantService.getApplicants();
            applicantList = fullApplicantList;
        }

        private void ExecuteSearch(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            applicantList = fullApplicantList.Where(n => n.FullName.ToLower().Contains(Search.Text.ToLower()) || n.Id.ToString().Contains(Search.Text)).ToList();
            Bindings.Update();
        }

        private async void SaveResults(object sender, RoutedEventArgs e)
        {
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("Comma-Separated Values (CSV)", new List<String> { ".csv" });
            var file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                try
                {
                    await SaveCSV(file);
                    await Notify("Save successful.");
                } catch
                {
                    await Notify("An error occurred during the save process.");
                }
            }
        }

        private async void SaveTemplate(object sender, RoutedEventArgs e)
        {
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("Comma-Separated Values (CSV)", new List<String> { ".csv" });
            var file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                var template = await StorageFile.GetFileFromApplicationUriAsync(new Uri(this.BaseUri, "/Assets/template.csv"));
                await template.CopyAndReplaceAsync(file);
            }
        }
    }
}
