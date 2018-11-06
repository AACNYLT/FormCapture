using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static FormCapture.Utilities;
// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FormCapture
{
    public sealed partial class InterviewPanel2017 : UserControl
    {
        public Applicant applicant;
        public Interview2017 interview;
        public InterviewPanel2017(Applicant _applicant)
        {
            applicant = _applicant;
            var context = new FormContext();
            var results = context.Interviews.Where(n => n.ApplicantId == applicant.Id).ToList();
            if (results.Count() > 0)
            {
                interview = results.FirstOrDefault();
            }
            else
            {
                interview = new Interview2017 { ApplicantId = applicant.Id };

                context.Interviews.Add(interview);
                context.SaveChanges();
            }
            this.InitializeComponent();
        }

        private async void Save(object sender, RoutedEventArgs e)
        {
            var context = new FormContext();
            context.Interviews.Update(interview);
            await context.SaveChangesAsync();
            await Notify("Interview saved.");
        }
    }
}
