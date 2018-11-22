using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.System.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using static FormCapture.Utilities;
using System;
using Windows.UI.Core;
using Windows.Devices.Enumeration;
using Windows.Storage;
using Windows.Media.MediaProperties;
using Windows.UI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FormCapture.Controls
{
    public sealed partial class ApplicantDetail : UserControl
    {
        private Applicant applicant;
        MediaCapture mediaCapture;
        bool isPreviewing;
        bool isRecording;
        DisplayRequest displayRequest = new DisplayRequest();
        private int deviceIndex = 0;

        public event GoBackHandler GoBack;
        public delegate void GoBackHandler(Applicant applicant);

        public ApplicantDetail()
        {
            this.InitializeComponent();
        }

        public void SetApplicant(Applicant applicant)
        {
            this.applicant = applicant;
            Bindings.Update();
            var nameAnimation = ConnectedAnimationService.GetForCurrentView().GetAnimation("applicant");
            if (nameAnimation != null)
            {
                nameAnimation.TryStart(ApplicantHeader);
            }
            StartPreviewAsync();
        }

        private async void Back_Click(object sender, RoutedEventArgs e)
        {
            await StopPreviewAsync();
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("applicantback", ApplicantHeader);
            GoBack(applicant);
        }

        private async void StartPreviewAsync()
        {
            try
            {
                mediaCapture = new MediaCapture();
                var devices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
                var settings = new MediaCaptureInitializationSettings { VideoDeviceId = devices[deviceIndex % devices.Count].Id };
                await mediaCapture.InitializeAsync(settings);
                displayRequest.RequestActive();
                captureElement.Source = mediaCapture;
                var properties = (VideoEncodingProperties)mediaCapture.VideoDeviceController.GetMediaStreamProperties(MediaStreamType.VideoPreview);
                double aspectRatio = properties.Width / (double)properties.Height;
                captureContainer.Height = captureContainer.ActualWidth / aspectRatio;
                await mediaCapture.StartPreviewAsync();
                isPreviewing = true;
                return;
            }
            catch
            {
                await Notify("We couldn't access the camera.", "Error");
                return;
            }

        }

        private async Task StopPreviewAsync()
        {
            if (mediaCapture != null)
            {
                if (isRecording)
                {
                    await mediaCapture.StopRecordAsync();
                }

                if (isPreviewing)
                {
                    await mediaCapture.StopPreviewAsync();
                }

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    captureElement.Source = null;
                    if (displayRequest != null)
                    {
                        displayRequest.RequestRelease();
                    }
                    mediaCapture.Dispose();
                    mediaCapture = null;
                });
            }
            return;
        }

        private void SwitchCameras(object sender, RoutedEventArgs e)
        {
            deviceIndex++;
            StartPreviewAsync();
        }

        private async void TakePhoto(object sender, RoutedEventArgs e)
        {
            CameraButton.IsEnabled = false;
            captureRing.IsActive = true;
            var folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("NYLT Form Capture", CreationCollisionOption.OpenIfExists);
            var file = await folder.CreateFileAsync(applicant.FileName + ".jpg", CreationCollisionOption.ReplaceExisting);
            await mediaCapture.CapturePhotoToStorageFileAsync(ImageEncodingProperties.CreateJpeg(), file);
            captureRing.IsActive = false;
            CameraButton.IsEnabled = true;
            CameraLabel.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Black);
            CameraLabel.Text = "photo taken";
        }

        private async void ToggleRecording(object sender, RoutedEventArgs e)
        {
            if (VideoButton.IsChecked.Value)
            {
                CameraLabel.Text = "wait for it...";
                SwitchCameraButton.IsEnabled = false;
                CameraButton.IsEnabled = false;
                VideoButton.IsEnabled = false;
                var folder = await KnownFolders.VideosLibrary.CreateFolderAsync("NYLT Form Capture", CreationCollisionOption.OpenIfExists);
                var file = await folder.CreateFileAsync(applicant.FileName + ".mp4", CreationCollisionOption.ReplaceExisting);
                await mediaCapture.StartRecordToStorageFileAsync(MediaEncodingProfile.CreateMp4(VideoEncodingQuality.HD720p), file);
                CameraLabel.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Red);
                CameraLabel.Text = "recording";
                isRecording = true;
                VideoButton.IsEnabled = true;
            }
            else
            {
                CameraLabel.Foreground = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.Black);
                CameraLabel.Text = "saving";
                await mediaCapture.StopRecordAsync();
                isRecording = false;
                CameraLabel.Text = "";
                SwitchCameraButton.IsEnabled = true;
                CameraButton.IsEnabled = true;
            }
        }
    }
}
