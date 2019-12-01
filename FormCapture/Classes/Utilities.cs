using FormCapture.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace FormCapture.Classes
{
    public static class Utilities
    {
        public static async Task ProcessCSV(StorageFile file)
        {
            using (var fileStream = await file.OpenStreamForReadAsync())
            {
                using (var csv = new CsvHelper.CsvReader(new StreamReader(fileStream)))
                {
                    csv.Configuration.RegisterClassMap<ApplicantMap>();
                    var results = csv.GetRecords<Applicant>().ToList();
                    await ProcessApplicants(results);
                    return;
                }
            }
        }

        public static async Task ProcessApplicants(List<Applicant> applicants)
        {
            using (var context = new FormContext())
            {
                context.Applicants.RemoveRange(context.Applicants.ToList());
                context.Interviews.RemoveRange(context.Interviews.ToList());
                await context.SaveChangesAsync();
                await context.Applicants.AddRangeAsync(applicants);
                await context.SaveChangesAsync();
            return;
            }
        }

        public static async Task Notify(string message)
        {
            var Notifier = new ContentDialog { Content = message, CloseButtonText = "Close"};
            await Notifier.ShowAsync();
            return;
        }

        public static async Task Notify(string message, string title)
        {
            var Notifier = new ContentDialog { Content = message, Title = title, CloseButtonText = "Close" };
            await Notifier.ShowAsync();
            return;
        }

        public static async Task<Boolean> NotifyYesNo(string message, string title)
        {
            var Notifier = new ContentDialog { Content = message, Title = title , PrimaryButtonText = "Yes", CloseButtonText = "No"};
            return await Notifier.ShowAsync() == ContentDialogResult.Primary;
        }

        public static async Task ShowPodioCredentialDialog()
        {
            var dialog = new ContentDialog();
            var dialogContent = new PodioCredentialDialog();
            dialogContent.CredentialsSaved += delegate
            {
                dialog.Hide();
                return;
            };
            dialog.Content = dialogContent;
            await dialog.ShowAsync();
        }

        public static async Task<string> ShowURLDialog(string Title, string PrimaryButtonText)
        {
            var urlScope = new Windows.UI.Xaml.Input.InputScope();
            urlScope.Names.Add(new Windows.UI.Xaml.Input.InputScopeName(Windows.UI.Xaml.Input.InputScopeNameValue.Url));
            var urlTextBox = new TextBox
            {
                AcceptsReturn = false,
                Height = 32,
                InputScope = urlScope,
                PlaceholderText = "Server URL"
            };
            var dialog = new ContentDialog()
            {
                CloseButtonText = "Cancel",
                PrimaryButtonText = PrimaryButtonText,
                Content = urlTextBox,
                Title = Title
            };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                return urlTextBox.Text;
            } else
            {
                return null;
            }
        }
        
        public static async Task<List<string>> ShowDualURLDialog(string Title, string PrimaryButtonText, string FirstPlaceholder, string SecondPlaceholder)
        {
            var urlScope1 = new Windows.UI.Xaml.Input.InputScope();
            urlScope1.Names.Add(new Windows.UI.Xaml.Input.InputScopeName(Windows.UI.Xaml.Input.InputScopeNameValue.Url));
            var urlScope2 = new Windows.UI.Xaml.Input.InputScope();
            urlScope2.Names.Add(new Windows.UI.Xaml.Input.InputScopeName(Windows.UI.Xaml.Input.InputScopeNameValue.Url));
            var url1TextBox = new TextBox
            {
                AcceptsReturn = false,
                Height = 32,
                InputScope = urlScope1,
                PlaceholderText = FirstPlaceholder,
                Margin = new Windows.UI.Xaml.Thickness(0,0,0,10)
            };
            var url2TextBox = new TextBox
            {
                AcceptsReturn = false,
                Height = 32,
                InputScope = urlScope2,
                PlaceholderText = SecondPlaceholder
            };

            var urlStackPanel = new StackPanel();
            urlStackPanel.Children.Add(url1TextBox);
            urlStackPanel.Children.Add(url2TextBox);
            var dialog = new ContentDialog()
            {
                CloseButtonText = "Cancel",
                PrimaryButtonText = PrimaryButtonText,
                Content = urlStackPanel,
                Title = Title
            };
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                return new List<String> { url1TextBox.Text, url2TextBox.Text };
            } else
            {
                return null;
            }
        }

        public static string URLifyString(string url)
        {
            if (!url.StartsWith("http://"))
            {
                url = string.Concat("http://", url);
            }
            return new Uri(url).ToString();
        }

        public static async Task SaveCSV(StorageFile file)
        {
            using (var csv = new CsvHelper.CsvWriter(new StreamWriter(await file.OpenStreamForWriteAsync())))
            {
                csv.WriteField("Applicant ID");
                csv.WriteField("First Name");
                csv.WriteField("Last Name");
                csv.WriteField("Uniform");
                csv.WriteField("Spirit");
                csv.WriteField("Presentation");
                csv.WriteField("Preparation");
                csv.WriteField("Attitude");
                csv.WriteField("Understanding");
                csv.WriteField("Comments");
                csv.WriteField("Recommend for Staff");
                csv.WriteField("Recommended Position");
                csv.WriteField("Interview Team");
                csv.NextRecord();

                var context = new FormContext();
                var applicants = context.Applicants.ToList();
                var interviews = context.Interviews.ToList();

                foreach (var interview in interviews)
                {
                    var applicant = applicants.Where(n => n.Id == interview.ApplicantId).FirstOrDefault();
                    if (applicant != null)
                    {
                        csv.WriteField(interview.ApplicantId);
                        csv.WriteField(applicant.FirstName);
                        csv.WriteField(applicant.LastName);
                        csv.WriteField(interview.Uniform);
                        csv.WriteField(interview.Spirit);
                        csv.WriteField(interview.Presentation);
                        csv.WriteField(interview.Preparation);
                        csv.WriteField(interview.Attitude);
                        csv.WriteField(interview.Understanding);
                        csv.WriteField(interview.Comments);
                        csv.WriteField(interview.Recommend);
                        csv.WriteField(interview.RecommendedPosition);
                        csv.WriteField(interview.Team);
                        csv.NextRecord();
                    }
                }
                return;
            }
        }
    }
}
