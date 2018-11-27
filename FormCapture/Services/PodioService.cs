using FormCapture.Classes;
using FormCapture.Controls;
using PodioAPI;
using PodioAPI.Models;
using PodioAPI.Utils.ItemFields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace FormCapture.Services
{
    static class PodioService
    {
        private static async Task<Podio> AuthenticatePodio()
        {
            var podio = new Podio("nyltformcapture", Secrets.PodioAppSecret);
            var vault = new Windows.Security.Credentials.PasswordVault();
            if (vault.FindAllByResource("nyltformcapture").Count == 0)
            {
                await Utilities.ShowPodioCredentialDialog();
            }
            var credentials = vault.FindAllByResource("nyltformcapture").First();
            credentials.RetrievePassword();
            await podio.AuthenticateWithPassword(credentials.UserName, credentials.Password);
            return podio;
        }

        public static async System.Threading.Tasks.Task GetApplicants(Action<string,string, IEnumerable<PodioAPI.Models.Item>> onComplete)
        {
            var podio = await AuthenticatePodio();
            var itemResults = (await podio.ItemService.FilterItems(21458112, new PodioAPI.Models.Request.FilterOptions() {
                Limit = 200
            })).Items;
            var dialog = new ContentDialog();
            var dialogContent = new PodioConfigDialog();
            dialogContent.FieldMappingComplete += (string FirstNameMapping, string LastNameMapping) =>
            {
                dialog.Hide();
                onComplete(FirstNameMapping, LastNameMapping, itemResults);
            };
            dialog.Content = dialogContent;
            await dialog.ShowAsync();
            return;
        }

        public static async System.Threading.Tasks.Task Upload2017Interviews(List<Interview2017> interviews)
        {
            var podio = await AuthenticatePodio();
            foreach (var interview in interviews)
            {
                var item = new Item();
                item.Field<AppItemField>("scout").ItemId = interview.ApplicantId;
                item.Field<NumericItemField>("number").Value = interview.Uniform;
                item.Field<NumericItemField>("spirit-enthusiasm").Value = interview.Spirit;
                item.Field<NumericItemField>("presentation-speaking-voice").Value = interview.Presentation;
                item.Field<NumericItemField>("preparation").Value = interview.Preparation;
                item.Field<NumericItemField>("attitude").Value = interview.Attitude;
                item.Field<NumericItemField>("understanding-of-program").Value = interview.Understanding;
                item.Field<TextItemField>("comments").Value = interview.Comments ?? " ";
                item.Field<CategoryItemField>("recommended-for-staff").OptionId = interview.Recommend ? 1 : 2;
                item.Field<TextItemField>("recommended-position").Value = interview.RecommendedPosition ?? " ";
                item.Field<TextItemField>("interview-team").Value = interview.Team ?? " ";
                var context = new FormContext();
                var applicant = context.Applicants.Where(n => n.Id == interview.ApplicantId).First();
                var folder = (StorageFolder) await KnownFolders.PicturesLibrary.TryGetItemAsync("NYLT Form Capture");
                if (folder != null)
                {
                    var file = (StorageFile)await folder.TryGetItemAsync(applicant.FileName + ".jpg");
                    if (file != null)
                    {
                        using (var stream = await file.OpenStreamForReadAsync())
                        {
                            var bytes = new byte[(int)stream.Length];
                            stream.Read(bytes, 0, (int)stream.Length);
                            var uploadedFile = await podio.FileService.UploadFile(file.Name, bytes,"image/jpeg");
                            item.FileIds = new List<int> {
                                uploadedFile.FileId
                            };
                        }
                    }
                }
                await podio.ItemService.AddNewItem(21935017, item);
            }
            return;
        }
    }
}
