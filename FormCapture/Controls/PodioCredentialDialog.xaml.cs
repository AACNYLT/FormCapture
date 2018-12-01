using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static FormCapture.Classes.Utilities;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FormCapture.Controls
{
    public sealed partial class PodioCredentialDialog : UserControl
    {
        public event CredentialsSavedHandler CredentialsSaved;
        public delegate void CredentialsSavedHandler();

        public PodioCredentialDialog()
        {
            this.InitializeComponent();
        }

        private async void SavePodioCredentials(object sender, RoutedEventArgs e)
        {
            try
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                try
                {
                    var existingCredentials = vault.FindAllByResource("nyltformcapture");
                    foreach (var credential in existingCredentials)
                    {
                        vault.Remove(credential);
                    }
                }
                catch { }
                vault.Add(new Windows.Security.Credentials.PasswordCredential(
                    "nyltformcapture", AppID.Text, AppSecret.Password));
                CredentialsSaved();
            }
            catch (Exception ex)
            {
                await Notify(ex.Message);
            }
        }
    }
}
