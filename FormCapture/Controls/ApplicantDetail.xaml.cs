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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FormCapture.Controls
{
    public sealed partial class ApplicantDetail : UserControl
    {
        private Applicant applicant;

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
            var animation = ConnectedAnimationService.GetForCurrentView().GetAnimation("applicant");
            if (animation != null)
            {
                animation.TryStart(ApplicantHeader);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("applicantback", ApplicantHeader);
            GoBack(applicant);
        }
    }
}
