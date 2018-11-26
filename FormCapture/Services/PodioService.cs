using FormCapture.Classes;
using FormCapture.Controls;
using PodioAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace FormCapture.Services
{
    static class PodioService
    {
        public static async Task GetApplicants(Action<string,string, IEnumerable<PodioAPI.Models.Item>> onComplete)
        {
            var podio = new Podio("nyltformcapture", Secrets.PodioAppSecret);
            var vault = new Windows.Security.Credentials.PasswordVault();
            if (vault.FindAllByResource("nyltformcapture").Count == 0)
            {
                await Utilities.ShowPodioCredentialDialog();
            }
            var credentials = vault.FindAllByResource("nyltformcapture").First();
            credentials.RetrievePassword();
            await podio.AuthenticateWithApp(int.Parse(credentials.UserName), credentials.Password);
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
    }
}
