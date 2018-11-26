using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using FormCapture.Classes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FormCapture
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
            this.applicantList.loadApplicants();
        }

        private void applicantClicked(Applicant applicant)
        {
            ExpandPanel.Begin();
            ExpandPanel.Completed += delegate
            {
                ContentGrid.Opacity = 0;
                ContentGrid.Content = new InterviewPanel2017(applicant);
                LoadContentGrid.Begin();
            };
            applicantList.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            applicantDetail.Visibility = Windows.UI.Xaml.Visibility.Visible;
            applicantDetail.SetApplicant(applicant);
        }

        private void goBack(Applicant applicant)
        {
            ((IPanel)ContentGrid.Content).Save(Enumerations.SaveFormOptions.SuppressDialog);
            UnloadContentGrid.Begin();
            UnloadContentGrid.Completed += delegate
            {
                ContentGrid.Content = null;
            };
            ShrinkPanel.Begin();
            applicantList.Visibility = Windows.UI.Xaml.Visibility.Visible;
            applicantDetail.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            applicantList.ShowAnimation(applicant);
        }

        private void MenuButtonClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MainView.IsPaneOpen = !MainView.IsPaneOpen;
        }
    }
}
