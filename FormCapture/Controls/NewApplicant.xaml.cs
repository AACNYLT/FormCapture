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
    public sealed partial class NewApplicant : UserControl
    {
        public NewApplicant()
        {
            this.InitializeComponent();
        }

        public event ApplicantCreatedHandler ApplicantCreated;
        public delegate void ApplicantCreatedHandler();

        private async void AddApplicantClick(object sender, RoutedEventArgs e)
        {
            var applicant = new Applicant(FirstName.Text, LastName.Text, int.Parse(ApplicantID.Text));
            var context = new FormContext();
            context.Applicants.Add(applicant);
            await context.SaveChangesAsync();
            ApplicantCreated();
        }
    }
}
