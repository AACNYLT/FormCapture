using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.System.Display;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FormCapture.Controls
{
    public sealed partial class CameraPanel : UserControl
    {
        private Applicant applicant;
        private MediaCapture mediaCapture = new MediaCapture();
    private bool isPreviewing = false;
        private DisplayRequest displayRequest = new DisplayRequest();

        public CameraPanel(Applicant applicant)
        {
            this.applicant = applicant;
            this.InitializeComponent();
        }

        public async void StartPreview() {
            try

            mediaCapture = new MediaCapture();

            var settings = new MediaCaptureInitializationSettings()
            {
                VideoDeviceId = videoDeviceList(videoDeviceIndex).Id
            }

            await mediaCapture.InitializeAsync(settings);
            CaptureWindow.Source = mediaCapture;
            await mediaCapture.StartPreviewAsync();
            displayRequest.RequestActive();
            isPreviewing = true;
                }
            catch {
                HandleFailure();
           }
    }

    Public Async Function StopPreview() As Task(Of Boolean)
        If isPreviewing Then
            Await mediaCapture.StopPreviewAsync()
            displayRequest.RequestRelease()
            isPreviewing = False
            mediaCapture.Dispose()
        End If
        CaptureWindow.Source = Nothing
        mediaCapture = Nothing
        Return True
    End Function
    }
}
