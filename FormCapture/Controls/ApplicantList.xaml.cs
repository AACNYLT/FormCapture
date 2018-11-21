using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using static FormCapture.Utilities;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FormCapture.Controls
{
    public sealed partial class ApplicantList : UserControl
    {
        private List<Applicant> applicantList;
        private List<Applicant> fullApplicantList;

        public event ApplicantClickedHandler ApplicantClicked;
        public delegate void ApplicantClickedHandler(Applicant applicant);

        public ApplicantList()
        {
            InitializeComponent();
        }

        public async void ShowAnimation(Applicant applicant)
        {
            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("applicantback");
            if (animation != null)
            {
                await ApplicantView.TryStartConnectedAnimationAsync(animation, applicant, "ApplicantItem");
            }
        }

        public void loadApplicants()
        {
            fullApplicantList = ApplicantService.getApplicants();
            applicantList = fullApplicantList;
            Bindings.Update();
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
                    loadApplicants();
                }
                catch
                {
                    await Notify("An error occurred - check the format of your CSV file.");
                }
            }
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
                }
                catch
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

        private async void AddApplicant(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog();
            var dialogContent = new NewApplicant();
            dialogContent.ApplicantCreated += delegate
            {
                dialog.Hide();
                loadApplicants();
            };
            dialog.Content = dialogContent;
            await dialog.ShowAsync();
        }

        private async void ShowPrivacyPolicy(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                CloseButtonText = "Close",
                Content = new TextBlock { Text = "This app won't share any information transmitted or stored with it, nor will that data be used for any other purpose beyond the services the app provides. The data will furthermore not be retained after it is deleted by the user.", TextWrapping = TextWrapping.WrapWholeWords }
            };
            await dialog.ShowAsync();
        }

        private void ApplicantSelected(object sender, ItemClickEventArgs e)
        {
            ApplicantView.PrepareConnectedAnimation("applicant", e.ClickedItem, "ApplicantItem");
            ApplicantClicked((Applicant)e.ClickedItem);
        }
    }
}
