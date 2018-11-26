using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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

        private void SavePodioCredentials(object sender, RoutedEventArgs e)
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            var existingCredentials = vault.FindAllByResource("nyltformcapture");
            foreach (var credential in existingCredentials)
            {
                vault.Remove(credential);
            }
            vault.Add(new Windows.Security.Credentials.PasswordCredential(
                "nyltformcapture", AppID.Text, AppSecret.Text));
            CredentialsSaved();
        }
    }
}
