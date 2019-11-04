using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FormCapture.Classes;

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
            if (FirstName.Text != "" && LastName.Text != "" && ApplicantID.Text != "")
            {
                var applicant = new Applicant(FirstName.Text, LastName.Text, int.Parse(ApplicantID.Text));
                var context = new FormContext();
                context.Applicants.Add(applicant);
                await context.SaveChangesAsync();
                ApplicantCreated();
            }
        }
    }
}
