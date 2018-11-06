using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;

namespace FormCapture
{
    public static class Utilities
    {
        public static async Task ProcessCSV(StorageFile file)
        {
            using (var fileStream = await file.OpenStreamForReadAsync())
            {
                var csv = new CsvHelper.CsvReader(new StreamReader(fileStream));
                csv.Configuration.RegisterClassMap<ApplicantMap>();
                var results = csv.GetRecords<Applicant>().ToList();
                var context = new FormContext();
                context.Applicants.RemoveRange(context.Applicants.ToList());
                context.Interviews.RemoveRange(context.Interviews.ToList());
                await context.SaveChangesAsync();
                await context.Applicants.AddRangeAsync(results);
                await context.SaveChangesAsync();
                return;
            }
        }

        public static async Task Notify(string message)
        {
            var Notifier = new MessageDialog(message);
            await Notifier.ShowAsync();
            return;
        }

        public static async Task Notify(string message, string title)
        {
            var Notifier = new MessageDialog(message, title);
            await Notifier.ShowAsync();
            return;
        }

        public static async Task SaveCSV(StorageFile file)
        {
            using (var csv = new CsvHelper.CsvWriter(new StreamWriter(await file.OpenStreamForWriteAsync())))
            {
                csv.WriteField("Applicant ID");
                csv.WriteField("First Name");
                csv.WriteField("Last Name");
                csv.WriteField("Uniform");
                csv.WriteField("Spirit");
                csv.WriteField("Presentation");
                csv.WriteField("Preparation");
                csv.WriteField("Attitude");
                csv.WriteField("Understanding");
                csv.WriteField("Comments");
                csv.WriteField("Recommend for Staff");
                csv.WriteField("Recommended Position");
                csv.WriteField("Interview Team");
                csv.NextRecord();

                var context = new FormContext();
                var applicants = context.Applicants.ToList();
                var interviews = context.Interviews.ToList();

                foreach (var interview in interviews)
                {
                    var applicant = applicants.Where(n => n.Id == interview.ApplicantId).FirstOrDefault();
                    if (applicant != null)
                    {
                        csv.WriteField(interview.ApplicantId);
                        csv.WriteField(applicant.FirstName);
                        csv.WriteField(applicant.LastName);
                        csv.WriteField(interview.Uniform);
                        csv.WriteField(interview.Spirit);
                        csv.WriteField(interview.Presentation);
                        csv.WriteField(interview.Preparation);
                        csv.WriteField(interview.Attitude);
                        csv.WriteField(interview.Understanding);
                        csv.WriteField(interview.Comments);
                        csv.WriteField(interview.Recommend);
                        csv.WriteField(interview.RecommendedPosition);
                        csv.WriteField(interview.Team);
                        csv.NextRecord();
                    }
                }
                return;
            }
        }
    }
}
