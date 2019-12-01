using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static FormCapture.Classes.Enumerations;
using static FormCapture.Classes.Utilities;
using FormCapture.Classes;
using FormCapture.Services;
// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FormCapture
{
    public sealed partial class InterviewPanel2017 : UserControl, IPanel
    {
        public Applicant applicant;
        public Interview2017 interview;
        private bool newInterview;
        public InterviewPanel2017(Applicant _applicant)
        {
            applicant = _applicant;
            var results = InterviewService.getInterviews(applicant.Id);
            if (results.Count() > 0)
            {
                interview = results.FirstOrDefault();
                newInterview = false;
            }
            else
            {
                interview = new Interview2017 { ApplicantId = applicant.Id };
                newInterview = true;
            }
            this.InitializeComponent();
        }

        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            Save();
        }

        public async void Save(SaveFormOptions saveBehavior = SaveFormOptions.ShowDialog)
        {
            if (newInterview)
            {
                if (saveBehavior != SaveFormOptions.SuppressDialog)
                {
                    await InterviewService.saveInterview(interview);
                    newInterview = false;
                }
            }
            else { await InterviewService.updateInterview(interview); }
            if (saveBehavior == SaveFormOptions.ShowDialog)
            {
                await Notify("Interview saved.");
            }
        }
    }
}
