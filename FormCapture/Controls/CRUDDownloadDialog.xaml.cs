using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NYLTRestController;
using FormCapture.Classes;
using System.Collections.Generic;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FormCapture.Controls
{
    public sealed partial class CRUDDownloadDialog : UserControl
    {
        public CRUDDownloadDialog()
        {
            this.InitializeComponent();
        }

        private async void DownloadClick(object sender, RoutedEventArgs e)
        {
            var restController = new RestController(DownloadURL.Text);
            var applicantList = await restController.GetObjectAsync<List<Applicant>>("/");
        }
    }
}
